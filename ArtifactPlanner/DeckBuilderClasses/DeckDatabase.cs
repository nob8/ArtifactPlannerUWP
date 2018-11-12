using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace ArtifactPlanner.DeckBuilderClasses
{
    public sealed class DeckDatabase
    {
        public static readonly DeckDatabase Decks = new DeckDatabase();

        static DeckDatabase()
        {
            
        }

        public static async void DeckFileUpdated(ApplicationData data, object o)
        {
            var deckDataStream = await (await ApplicationData.Current.RoamingFolder.GetFileAsync("decks.json"))
                .OpenSequentialReadAsync();

            lock (Decks)
            {
                Decks._userDecks =
                    Decks._localUpdateJsonDeserializer.Deserialize<List<Deck>>(
                        new JsonTextReader(new StreamReader(deckDataStream.AsStreamForRead())));
            }
        }

        public static void SyncUpdateToDeckFile()
        {
            List<Deck> deckReference;
            lock (Decks)
            {
                // Make sure we clone all the Deck references so that future updates to the local data don't corrupt our data as we try to read it
                deckReference = Decks._userDecks.ConvertAll((deck) => deck.Clone());
            }

            new Thread(async () =>
            {
                var serializedDecks = new StringBuilder();
                var serializedDecksWriter = new StringWriter(serializedDecks);
                Decks._syncUpdateJsonSerializer.Serialize(serializedDecksWriter, deckReference);
                StorageFile decksFileHandle;
                if (await ApplicationData.Current.RoamingFolder.FileExistsAsync("decks.json"))
                {
                    decksFileHandle =
                        await ApplicationData.Current.RoamingFolder.GetFileAsync("decks.json");
                }
                else
                {
                    decksFileHandle =
                        await ApplicationData.Current.RoamingFolder.CreateFileAsync("decks.json");
                }
                var decksFile = await (decksFileHandle).OpenStreamForWriteAsync();
                await decksFile.WriteAsync(Encoding.UTF8.GetBytes(serializedDecks.ToString()), 0, serializedDecks.Length);
                decksFile.Close();

            }).Start();
        }

        public static void replaceUserDecks(List<Deck> newDecks)
        {
            Decks._userDecks = newDecks;
        }

        private Newtonsoft.Json.JsonSerializer _syncUpdateJsonSerializer = JsonSerializer.Create();
        private Newtonsoft.Json.JsonSerializer _localUpdateJsonDeserializer = JsonSerializer.Create();
        private List<Deck> _userDecks;

        public static string DatabaseContentsToString()
        {
            lock (Decks)
            {
                return "Decks in database: " + string.Join(",", Decks._userDecks);
            }
        }
    }
}
