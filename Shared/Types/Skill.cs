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
        public string id => $"Skill|{Name}";
        public string PartitionKey => Name;
        [JsonIgnore]
        public int ID { get; set; }
        public string ImageID
        {
            get
            {
                //Mage skills
                if (this.Name.Contains("Basic heal", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/mage/basicheal";
                }
                if (this.Name.Contains("Enfeeble", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/mage/enfeeble";
                }
                if (this.Name.Contains("Idiocy", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/mage/idiocy";
                }
                if (this.Name.Contains("Magic missile", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/mage/magicmissile";
                }
                if (this.Name.Contains("Slow", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/mage/slow";
                }
                if (this.Name.Contains("Vertigo", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/mage/vertigo";
                }
                if (this.Name.Contains("Vulnerability", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/mage/vulnerability";
                }
                //Ranger skills
                if (this.Name.Contains("Camouflage", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/ranger/camouflage";
                }
                if (this.Name.Contains("Double shot", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/ranger/doubleshot";
                }
                if (this.Name.Contains("Set trap", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/ranger/settrap";
                }
                //Warrior skills
                if (this.Name.Contains("Cleave", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/warrior/cleave";
                }
                if (this.Name.Contains("Courage", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/warrior/courage";
                }
                if (this.Name.Contains("Devastate", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/warrior/devastate";
                }
                if (this.Name.Contains("Enrage", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/warrior/enrage";
                }
                if (this.Name.Contains("Overpower", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/warrior/overpower";
                }
                if (this.Name.Contains("Rally", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/warrior/rally";
                }
                if (this.Name.Contains("Whirlwind", StringComparison.OrdinalIgnoreCase))
                {
                    return "newSkills/warrior/whirlwind";
                }
                return "newSkills/warrior/enrage";
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
