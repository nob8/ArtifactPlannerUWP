using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Gma.DataStructures.StringSearch;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArtifactPlanner.DeckBuilderClasses
{
    class CardDatabase
    {
        // If there are more common words in the future, add them to this list
        private static readonly HashSet<string> CommonWords = new HashSet<string>()
        {
            "the", "for"
        };

        private Dictionary<int, Card> _cardsById;
        private UkkonenTrie<Card> _cardsByNameTokensPrefix;
        private UkkonenTrie<Card> _cardsByRulesTokensPrefix;
        private List<Set> _sets;
        private OfficialArtifactSetManifest _manifest;

        public override bool Equals(object other)
        {
            if (!(other is CardDatabase otherDb)) return false;
            if ((otherDb._sets.Count == 0 && _sets.Count != 0) || _sets.Where((t, i) => !t.Equals(otherDb._sets[i])).Any())
            {
                return false;
            }
            return true;
        }

        public List<Card> GuessCardFromString(string prefix)
        {
            var cards = new List<Card>(_cardsByNameTokensPrefix.Retrieve(prefix));
            cards.AddRange(_cardsByRulesTokensPrefix.Retrieve(prefix));
            return cards;
        }

        public async void InitializeDatabaseFromCachedFile()
        {
            var test = ApplicationData.Current.LocalFolder.Path;
            var cardManifestTask = ApplicationData.Current.LocalFolder.GetFileAsync(@"Assets\CardDefinitions\artifact-base-set.json");
            _sets = new List<Set>();
            _cardsById = new Dictionary<int, Card>();
            _cardsByNameTokensPrefix = new UkkonenTrie<Card>(3);
            _cardsByRulesTokensPrefix = new UkkonenTrie<Card>(3);
            var cardManifest = await cardManifestTask;
            InitializeDatabaseFromInternalJsonFile((await cardManifest.OpenSequentialReadAsync()).AsStreamForRead());
        }

        private void InitializeDatabaseFromInternalJsonFile(Stream inputStream)
        {
            var serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            using (var sr = new StreamReader(inputStream))
            using (var reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    Set savedSet = serializer.Deserialize<Set>(reader);
                    _sets.Add(savedSet);
                    foreach(Card c in savedSet.CardContents)
                    {
                        _addCardToIndexes(c);
                    }
                }
            }
        }

        public async void WriteDatabaseToFile()
        {
            var internalDatabaseFile = (StorageFile) await ApplicationData.Current.LocalFolder.TryGetItemAsync(@"Assets\CardDefinitions\artifact-card-data-internal.json") ??
                                       await ApplicationData.Current.LocalFolder.CreateFileAsync(
                                           @"Assets\CardDefinitions\artifact-card-data-internal.json");
            
            var writeTransaction = await internalDatabaseFile.OpenTransactedWriteAsync();
            writeTransaction.Stream.Size = 0;
            var serializer = new JsonSerializer();
            serializer.TypeNameHandling = TypeNameHandling.All;
            using (var sw = new StreamWriter(writeTransaction.Stream.AsStreamForWrite()))
            using (var writer = new JsonTextWriter(sw))
            {
                foreach (var s in _sets)
                {
                    
                    serializer.Serialize(writer, s);
                }
            }
            await writeTransaction.CommitAsync();
        }

        public void InitializeDatabaseFromExternalJsonFile(Stream jsonInputStream)
        {
            _sets = new List<Set>();
            _cardsByNameTokensPrefix = new UkkonenTrie<Card>(3);
            _cardsByRulesTokensPrefix = new UkkonenTrie<Card>(3);
            _cardsById = new Dictionary<int, Card>();
            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(jsonInputStream))
            using (var reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    if (reader.TokenType != JsonToken.StartObject || !(serializer.Deserialize(reader) is JObject jsonObject)) continue;
                    var interiorObject = (JsonSet)jsonObject.GetValue("Sets")[0].ToObject(typeof (JsonSet));
                    var cardSet = new Set(interiorObject.Name, null, new List<Card>());
                    _sets.Add(cardSet);
                    foreach (var c in interiorObject.Cards)
                    {
                        var realCard = c.ToInternalCard();
                        _addCardToIndexes(realCard);

                        cardSet.CardContents.Add(realCard);
                    }
                    break;
                }
            }
        }

        private void _addCardToIndexes(Card realCard)
        {
            _cardsById.Add(realCard.Id, realCard);
            _tokenizeForSearchTree(realCard.Name)
                ?.ForEach(token => _cardsByNameTokensPrefix.Add(token, realCard));
            _tokenizeForSearchTree(realCard.Text)
                ?.ForEach(token => _cardsByRulesTokensPrefix.Add(token, realCard));
            if (realCard.Abilities == null) return;
            foreach (var realCardAbility in realCard.Abilities)
            {
                _tokenizeForSearchTree(realCardAbility.Text)
                    ?.ForEach(token => _cardsByRulesTokensPrefix.Add(token, realCard));
            }
        }

        private static List<string> _tokenizeForSearchTree(string realCardName)
        {
            return realCardName?.Split(' ')
                       .Where(token => token != null && token.Length > 2 && !CommonWords.Contains(token))
                       .Select(token =>token.ToLower()).ToList() ?? Enumerable.Empty<string>() as List<string>;
        }
    }
}
