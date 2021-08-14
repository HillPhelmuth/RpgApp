using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using RpgApp.Shared.Types.Enums;
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
        [JsonIgnore]
        public int ID { get; set; }
        public string MageImageID
        {
            get
            {
                if (this.Description.Contains("basicheal", StringComparison.OrdinalIgnoreCase))
                {
                    return "basicheal";
                }
                if (this.Description.Contains("enfeeble", StringComparison.OrdinalIgnoreCase))
                {
                    return "enfeeble";
                }
                if (this.Description.Contains("idiocy", StringComparison.OrdinalIgnoreCase))
                {
                    return "idiocy";
                }
                if (this.Description.Contains("magicmissile", StringComparison.OrdinalIgnoreCase))
                {
                    return "magicmissile";
                }
                if (this.Description.Contains("slow", StringComparison.OrdinalIgnoreCase))
                {
                    return "slow";
                }
                if (this.Description.Contains("vertigo", StringComparison.OrdinalIgnoreCase))
                {
                    return "vertigo";
                }
                if (this.Description.Contains("vulnerability", StringComparison.OrdinalIgnoreCase))
                {
                    return "vulnerability";
                }
                return "magicmissile";
            }
        }
        public string RangerImageID
        {
            get
            {
                if (this.Description.Contains("camouflage", StringComparison.OrdinalIgnoreCase))
                {
                    return "camouflage";
                }
                if (this.Description.Contains("doubleshot", StringComparison.OrdinalIgnoreCase))
                {
                    return "doubleshot";
                }
                if (this.Description.Contains("settrap", StringComparison.OrdinalIgnoreCase))
                {
                    return "settrap";
                }
                return "doubleshot";
            }
        }
        public string WarriorImageID
        {
            get
            {
                if (this.Description.Contains("cleave", StringComparison.OrdinalIgnoreCase))
                {
                    return "cleave";
                }
                if (this.Description.Contains("courage", StringComparison.OrdinalIgnoreCase))
                {
                    return "courage";
                }
                if (this.Description.Contains("devastate", StringComparison.OrdinalIgnoreCase))
                {
                    return "devastate";
                }
                if (this.Description.Contains("enrage", StringComparison.OrdinalIgnoreCase))
                {
                    return "enrage";
                }
                if (this.Description.Contains("overpower", StringComparison.OrdinalIgnoreCase))
                {
                    return "overpower";
                }
                if (this.Description.Contains("rally", StringComparison.OrdinalIgnoreCase))
                {
                    return "rally";
                }
                if (this.Description.Contains("whirlwind", StringComparison.OrdinalIgnoreCase))
                {
                    return "whirlwind";
                }
                return "enrage";
            }
        }

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
        public ICollection<Player> Players { get; set; }

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
    }
}
