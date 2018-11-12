using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactPlanner.DeckBuilderClasses
{
    public class Deck
    {
        public List<DeckCardReference> DeckCards;
        public Guid DeckId;
        public Uri DeckSummaryImage;
        public string DeckTitle;
        public List<Faction> DeckFactions;
        public List<DeckCardReference> SignatureCards;
        public Deck Clone()
        {
            return new Deck()
            {
                DeckCards = DeckCards, DeckId = DeckId, DeckSummaryImage = DeckSummaryImage, DeckTitle = DeckTitle,
                DeckFactions = DeckFactions, SignatureCards = SignatureCards
            };
        }

        public class DeckCardReference
        {
            public Guid CardId;
            public int Quantity;
            public Guid SignatureCardId;

            public override string ToString()
            {
                return "{Id: " + CardId + ", Quantity: " + Quantity + ", SignatureCardId: " + SignatureCardId + "}";
            }
        }

        public override string ToString()
        {
            return "DeckId: " + DeckId + ", DeckTitle: " + DeckTitle + ", DeckCards: {" + string.Join(",", DeckCards) + "}";
        }
    }
}
