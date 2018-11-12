using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtifactPlanner.DeckBuilderClasses
{
    public class JsonManifest
    {
        public List<JsonSet> contents;
    }
    public class JsonSet
    {
        public string Name;
        public int Count;
        public DateTime RelaseDate;
        public List<JsonCard> Cards;
    }
    /*
         * Item card
         * {
           "Id": 229,
           "Name": "Apotheosis Blade",
           "CardType": "Item",
           "Rarity": "Rare",
           "Color": "Yellow",
           "Text": "Equipped Hero has +8 Attack and +4 Siege. Condemn units equipped hero deals battle damage to. Active 1: Condemn enemy improvements. Condemn each item equipped by equipped hero's combat target",
           "ItemType": "Weapon",
           "GoldCost": 25,
           "Abilities": [
           {
           "Name": "Apotheosis Blade : Effect",
           "Type": "Continuous",
           "Text": "Equipped Hero has +8 Attack and +4 Siege. Condemn units equipped hero deals battle damage to."
           },
           {
           "Name": "Apotheosis Blade : Effect",
           "Type": "Active",
           "Text": "Condemn enemy improvements. Condemn each item equipped by equipped hero's combat target",
           "Cooldown": 1
           }
           ],
           "Artist": "",
           "Lore": ""
           },
         */
    /*
     * Spell Card
     * {
          "Id": 253,
          "RelatedId": [
            250
          ],
          "Name": "Allseeing One's Favor",
          "CardType": "Spell",
          "Rarity": "Rare",
          "Color": "Green",
          "Text": "Modify a green hero with \"Allies have +2 Regeneration.\"",
          "ManaCost": 4,
          "Artist": "",
          "Lore": ""
        },
     */
    /*
     * Improvement card
     * {
          "Id": 1001,
          "Name": "Keenfolk Turret",
          "CardType": "Improvement",
          "Rarity": "Uncommon",
          "Color": "Black",
          "Text": "Active 1: Deal 2 piercing damage to unit.",
          "ManaCost": 4,
          "CrossLane": true,
          "Abilities": [
            {
              "Name": "Keenfolk Turret : Effect",
              "Type": "Active",
              "Text": "Deal 2 piercing damage to unit.",
              "Cooldown": 1
            }
          ],
          "Artist": "Robbie Trevino",
          "Lore": ""
        },
     */
    /*
     * Hero Card
     * {
          "Id": 82,
          "RelatedId": [
            442
          ],
          "Name": "Tidehunter",
          "CardType": "Hero",
          "Rarity": "Uncommon",
          "Color": "Red",
          "Attack": 2,
          "Armor": 0,
          "Health": 18,
          "Abilities": [
            {
              "Name": "Ravage",
              "Type": "Active",
              "Text": "Stun Tidehunter's enemy neighbors this round and each other enemy has a 50% chance of being stunned this round.",
              "Cooldown": 4
            }
          ],
          "Bounty": 5,
          "Artist": "Mike Lim",
          "Lore": ""
        },
     */
    /*
     * Creep Card
     * {
          "Id": 478,
          "Name": "Relentless Zombie",
          "CardType": "Creep",
          "Rarity": "Uncommon",
          "Color": "Blue",
          "Text": "Play Effect: Give Relentless Zombie a Death Shield.",
          "Attack": 2,
          "Armor": 0,
          "Health": 2,
          "Abilities": [
            {
              "Name": "Relentless Zombie : Effect",
              "Type": "Play",
              "Text": "Give Relentless Zombie a Death Shield."
            }
          ],
          "Bounty": 1,
          "Artist": "Lars Grant-West",
          "Lore": ""
        },
     */
    public class JsonCard
    {
        /*
         * Generic card attributes
         */
        public int Id;
        public List<int> RelatedId;
        public string Name;
        public string CardType;
        public string Rarity;
        public string Color;
        public string Text;
        public List<JsonAbility> Abilities;
        public string Artist;
        public string Lore;
        public int? ManaCost;
        
        /*
         * Item card attributes
         */
        public string ItemType;
        public int? GoldCost;

        /*
         * Creep card attributes
         */

        public int? Attack;
        public int? Armor;
        public int? Health;
        public int? Bounty;

        /*
         * Hero card attributes
         */



        /*
         * Improvement card attributes
         */

        /*
         * Spell card attributes
         */
        public bool CrossLane;

        public Card ToInternalCard()
        {
            Card card;
            switch (CardType)
            {
                case "Creep":
                    card = new Creep();
                    var creep = (Creep) card;
                    creep.Armor = Armor ?? 0;
                    creep.Attack = Attack ?? 0;
                    creep.Health = Health ?? 0;
                    break;
                case "Improvement":
                    card = new Improvement();
                    var improvement = (Improvement) card;
                    improvement.Type = Abilities?.Min((ability) => ability.Type == null || ability.Type.Equals("Continuous") ? AbilityType.Passive : Enum.Parse<AbilityType>(ability.Type, true)) ?? AbilityType.Passive;
                    break;
                case "Hero":
                    card = new Hero();
                    var hero = (Hero)card;
                    hero.Armor = Armor ?? 0;
                    hero.Attack = Attack ?? 0;
                    hero.Health = Health ?? 0;
                    break;
                case "Spell":
                    card = new Spell();
                    break;
                case "Item":
                    card = new Item();
                    Item item = (Item)card;
                    item.Slot = Enum.Parse<ItemSlot>(ItemType);
                    break;
                default:
                    return null;
            }

            card.Name = Name;
            card.Text = Text;
            card.Abilities = Abilities?.ConvertAll((ability) => 
                new Ability()
                {
                    Name = ability.Name,
                    Text = ability.Text,
                    Type = ability.Type == null || ability.Type.Equals("Continuous") ? AbilityType.Passive : Enum.Parse<AbilityType>(ability.Type, true),
                    Cooldown = ability.Cooldown
                });
            card.Artist = Artist;
            card.Cost = ManaCost ?? GoldCost ?? 0;
            card.CrossLane = card is Improvement || CrossLane;
            card.Faction = !Color.Equals("Yellow", StringComparison.CurrentCultureIgnoreCase) ? Enum.Parse<Faction>(Color, true) : Faction.Item;
            card.Id = Id;
            card.Lore = Lore;
            card.Rarity = Enum.Parse<Rarity>(Rarity, true);
            card.RelatedCards = RelatedId;
            return card;
        }
    }

    public class JsonAbility
    {
        public string Name;
        public string Type;
        public string Text;
        public int Cooldown;
    }
}