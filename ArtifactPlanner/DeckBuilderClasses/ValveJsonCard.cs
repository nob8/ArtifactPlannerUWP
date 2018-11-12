using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ArtifactPlanner.DeckBuilderClasses
{
    /**
Base object:
{
    "card_set":{
        "version":1,
        "set_info":{
        "set_id":0,
        "pack_item_def":0,
        "name":{
            "english":"Base Set"
        }
    },
    "card_list":[
    {
        "card_id":1000,
        "base_card_id":1000,
        "card_type":"Stronghold",
        "card_name":{
            "english":"Ancient Tower"
        },
        "card_text":{

        },
        "mini_image":{
            "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/1000.91b2ed80da07ef5cf343540b09687fbf875168c8.png"
        },
        "large_image":{
            "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/1000_large_english.3dea67025da70c778d014dc3aae80c0c0a7008a6.png"
        },
        "ingame_image":{

        },
        "hit_points":80,
        "references":[

        ]

    },
    {
        "card_id":1002,
        "base_card_id":1002,
        "card_type":"Pathing",
        "card_name":{
            "english":"Left Path"
        },
        "card_text":{

        },
        "mini_image":{
            "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/1002.95db07546620aa3431e88471ac839c34ac1547f9.png"
        },
        "large_image":{
            "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/1002_large_english.570fd2a803121615a59744869e45f605c43d33e4.png"
        },
        "ingame_image":{

        },
        "references":[

        ]

    },
    {
        "card_id":1006,
        "base_card_id":1006,
        "card_type":"Creep",
        "card_name":{
            "english":"Melee Creep"
        },
        "card_text":{

        },
        "mini_image":{
            "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/1006.3c1f2d846354e3ddf9e0d5eef105334f66120813.png"
        },
        "large_image":{
            "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/1006_large_english.ca8ad07b111f6823f55725b23878b82732a004e2.png"
        },
        "ingame_image":{

        },
        "illustrator":"Forrest Imel",
        "mana_cost":3,
        "attack":2,
        "hit_points":4,
        "references":[

        ]

    },
    {
    "card_id":3000,
    "base_card_id":3000,
    "card_type":"Item",
    "card_name":{
        "english":"Traveler's Cloak"
    },
    "card_text":{
        "english":"Equipped hero has +4 Health."
    },
    "mini_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/3000.da00fd40824357ba4c29d5d061bdca37a10cabb5.png"
    },
    "large_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/3000_large_english.779b1e0d4b42be4615fc0517535483b7344f0bc5.png"
    },
    "ingame_image":{

    },
    "illustrator":"Julie Baroh",
    "sub_type":"Accessory",
    "gold_cost":3,
    "references":[

    ]

    },
    {
    "card_id":4000,
    "base_card_id":4000,
    "card_type":"Hero",
    "card_name":{
        "english":"Fahrvhan the Dreamer"
    },
    "card_text":{

    },
    "mini_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/4000.049cff338ab7274d0dcde4d4e3ec5bc5bcdd2a8e.png"
    },
    "large_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/4000_large_english.f4557af34d7079f579a0e66e8d1aae9cebad6f73.png"
    },
    "ingame_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/4000_ingame.cb1d509e48befec604e5ca48a6fe0752ff2ff1e5.png"
    },
    "illustrator":"Wisnu Tan",
    "is_green":true,
    "attack":4,
    "hit_points":10,
    "references":[
    {
        "card_id":4002,
        "ref_type":"includes",
        "count":3
    },
    {
        "card_id":4001,
        "ref_type":"passive_ability"
    }
    ]

    },
}
     */

    /**
     * {
    "card_id":4000,
    "base_card_id":4000,
    "card_type":"Hero",
    "card_name":{
        "english":"Fahrvhan the Dreamer"
    },
    "card_text":{

    },
    "mini_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/4000.049cff338ab7274d0dcde4d4e3ec5bc5bcdd2a8e.png"
    },
    "large_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/4000_large_english.f4557af34d7079f579a0e66e8d1aae9cebad6f73.png"
    },
    "ingame_image":{
        "default":"https://steamcdn-a.akamaihd.net/apps/583950/icons/set00/4000_ingame.cb1d509e48befec604e5ca48a6fe0752ff2ff1e5.png"
    },
    "illustrator":"Wisnu Tan",
    "is_green":true,
    "attack":4,
    "hit_points":10,
    "references":[
    {
        "card_id":4002,
        "ref_type":"includes",
        "count":3
    },
    {
        "card_id":4001,
        "ref_type":"passive_ability"
    }
    ]

    },
     */
    public class ValveJsonSet
    {
        public class ValveJsonSetMetadata
        {
            public int set_id;
            public int pack_item_def;
            public LocalizedString name;
        }
        public int version;
        [JsonProperty("set_info", Required = Required.Always)]
        public ValveJsonSetMetadata set_info;
        [JsonProperty("card_list", Required = Required.Always)]
        public List<ValveJsonCard> card_list;
    }
    public class LocalizedString
    {
        public string english;
        [JsonProperty("default")]
        public string Default;
    }
    public class ValveJsonCard
    {

        public class CardReference
        {
            public int card_id;
            public string ref_type;
            public int count;
        }
        public int card_id;
        public int base_card_id;
        public string card_type;
        public LocalizedString card_name;
        public LocalizedString card_text;
        public LocalizedString mini_image;
        public LocalizedString large_image;
        public LocalizedString ingame_image;
        public string illustrator;
        public List<CardReference> references;
    }
}
