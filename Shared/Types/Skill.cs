using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

//using Newtonsoft.Json;

namespace RpgApp.Shared.Types
{
    public class WarriorSkillList
    {
        [JsonPropertyName("warriorSkills")]
        public List<Skill> WarriorSkills { get; set; }
    }
    public class RangerSkillList
    {
        [JsonPropertyName("rangerSkills")]
        public List<Skill> RangerSkills { get; set; }
    }
    public class MageSkillList
    {
        [JsonPropertyName("Spells")]
        public List<Skill> MageSkills { get; set; }
    }
    public class Skill : IEquatable<Skill>
    {
        public string id => $"Skill|{Name}";
        public string PartitionKey => Name;
        [JsonIgnore]
        public int Index { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("goldCost")]
        public int GoldCost { get; set; }

        [JsonPropertyName("abilityCost")]
        public int AbilityCost { get; set; }

        [JsonPropertyName("classTypes")]
        [NotMapped]
        public List<string> AllowedClasses //All this is required to send and receive lists from sql db
        {
            get => AllowedClassesData?.Split(',').ToList();
            set => AllowedClassesData = string.Join(',', value ?? new List<string>());
        }
        [JsonIgnore]
        public string AllowedClassesData { get; set; }

        [JsonPropertyName("effects")]
        public List<Effect> Effects { get; set; }
        [JsonIgnore]

        public string ImageId
        {
            get
            {
                if (this.Name.Contains("sword", StringComparison.OrdinalIgnoreCase))
                    return "sword";
                if (Name.Contains("armor", StringComparison.OrdinalIgnoreCase))
                    return "foreign/armor";
                if (Name.Contains("shield", StringComparison.OrdinalIgnoreCase))
                    return "shield";
                if (Name.Contains("dagger", StringComparison.OrdinalIgnoreCase))
                    return "foreign/dagger";
                if (Name.Contains("potion", StringComparison.OrdinalIgnoreCase))
                    return "foreign/potionRed";
                if (Name.Contains("book", StringComparison.OrdinalIgnoreCase))
                    return "foreign/tome";
                if (Name.Contains("boots", StringComparison.OrdinalIgnoreCase))
                    return "shoes-slot";
                if (Name.Contains("wand", StringComparison.OrdinalIgnoreCase))
                    return "foreign/wand";
                if (Name.Contains("bow", StringComparison.OrdinalIgnoreCase))
                    return "foreign/bow";
                return "foreign/gemBlue";
            }
        }
        public ICollection<Player> Players { get; set; }

        #region IEquatable Implementation

        public bool Equals(Skill other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Description == other.Description && GoldCost == other.GoldCost && AbilityCost == other.AbilityCost;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Skill)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, GoldCost, AbilityCost);
        }
        #endregion
    }
}
