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
    public class Skill
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
    }
}
