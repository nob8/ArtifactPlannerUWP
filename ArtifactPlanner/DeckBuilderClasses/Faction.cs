using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactPlanner.DeckBuilderClasses
{
    public enum Faction
    {
        Red, Blue, Green, Black, Item
    }

    public enum ItemSlot
    {
        Weapon, Armor, Accessory, Consumable
    }

    public enum AbilityType
    {
        Active, Reactive, Passive, Play
    }

    public enum Rarity
    {
        Rare, Uncommon, Common, Basic
    }
}
