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
        [JsonIgnore]
        public int ID { get; set; }
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
            return Equals((Skill) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, GoldCost, AbilityCost);
        }
    }
}
