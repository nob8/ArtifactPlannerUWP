using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactPlanner.DeckBuilderClasses
{
    public class Card
    {
        public int Id;
        public bool Signature;
        // In the future, creeps may be crosslane in addition to spells
        public bool CrossLane;
        public int Cost;
        public Uri CardImage;
        public List<int> RelatedCards;
        public List<Ability> Abilities;
        public string SetName;
        public string Name;
        public string Artist;
        public string Lore;
        public string Text;
        public Faction Faction;
        public Rarity Rarity;
    }

    public class Ability
    {
        public string Name;
        public AbilityType Type;
        public string Text;
        public int? Cooldown;
    }

    public class Creep : Card
    {
        public Uri AbilityImage;
        public int Attack;
        public int Armor;
        public int Health;
    }

    public class Hero : Creep
    {
        public int SignatureCardId;
    }

    public class Spell : Card
    {
        
    }

    public class Item : Card
    {
        public ItemSlot Slot;
    }

    public class Improvement : Card
    {
        // Used for deciding whether to use no spikes, some spikes, or lots of spikes on the display.
        // Not yet known whether there will be improvements with more than one ability.
        public AbilityType Type;
    }
}
