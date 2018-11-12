using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactPlanner.DeckBuilderClasses
{
    class Set
    {
        public readonly string Name;
        public readonly Uri SetSymbol;
        public readonly List<Card> CardContents;

        public Set(string name, Uri setSymbol, List<Card> cardContents)
        {
            Name = name;
            SetSymbol = setSymbol;
            CardContents = cardContents;
        }

        protected bool Equals(Set other)
        {
            return string.Equals(Name, other.Name) && Equals(SetSymbol, other.SetSymbol) && !CardContents.Where((card, index) => !card.Equals(other.CardContents[index])).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SetSymbol != null ? SetSymbol.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CardContents != null ? CardContents.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
