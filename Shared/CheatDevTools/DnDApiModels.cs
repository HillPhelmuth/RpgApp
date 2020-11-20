using System.Collections.Generic;
using Newtonsoft.Json;

namespace RpgApp.Shared.CheatDevTools
{
    public class DnDApiModels
    {
    }
    public partial class GeneralList
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("results")]
        public List<GeneralApiData> GeneralApiData { get; set; }
    }

    public class EquipmentList
    {
        [JsonProperty("equipment")]
        public List<GeneralApiData> EquipmentsData { get; set; }
    }
    public partial class GeneralApiData
    {
        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class MonsterData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("hit_points")]
        public int HitPoints { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subtype")]
        public object Subtype { get; set; }

        [JsonProperty("alignment")]
        public string Alignment { get; set; }

        [JsonProperty("armor_class")]
        public long ArmorClass { get; set; }

        [JsonProperty("hit_dice")]
        public string HitDice { get; set; }

        [JsonProperty("strength")]
        public int Strength { get; set; }

        [JsonProperty("dexterity")]
        public int Dexterity { get; set; }

        [JsonProperty("constitution")]
        public int Constitution { get; set; }

        [JsonProperty("intelligence")]
        public int Intelligence { get; set; }

        [JsonProperty("wisdom")]
        public int Wisdom { get; set; }
        [JsonProperty("challenge_rating")]
        public double ChallengeRating { get; set; }
    }

    public class WeaponData
    {
        [JsonProperty("index")]
        public string Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("weapon_category")]
        public string WeaponCategory { get; set; }

        [JsonProperty("weapon_range")]
        public string WeaponRange { get; set; }

        [JsonProperty("category_range")]
        public string CategoryRange { get; set; }


        [JsonProperty("damage")]
        public Damage Damage { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }

    }
    public class Damage
    {
        [JsonProperty("damage_dice")]
        public string DamageDice { get; set; }


    }

    public class ArmorData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("armor_category")]
        public string ArmorCategory { get; set; }

        [JsonProperty("armor_class")]
        public ArmorClass ArmorClass { get; set; }

        [JsonProperty("str_minimum")]
        public long StrMinimum { get; set; }

        [JsonProperty("stealth_disadvantage")]
        public bool StealthDisadvantage { get; set; }

        [JsonProperty("weight")]
        public long Weight { get; set; }


    }
    public partial class ArmorClass
    {
        [JsonProperty("base")]
        public long Base { get; set; }

        [JsonProperty("dex_bonus")]
        public bool DexBonus { get; set; }

        [JsonProperty("max_bonus")]
        public object MaxBonus { get; set; }
    }

}
