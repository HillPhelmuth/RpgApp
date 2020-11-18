using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace RpgApp.Shared.Types
{
    public class WarriorSkillList
    {
        [JsonProperty("warriorSkills")]
        public List<Skill> WarriorSkills { get; set; }
    }
    public class RangerSkillList
    {
        [JsonProperty("rangerSkills")]
        public List<Skill> RangerSkills { get; set; }
    }
    public class MageSkillList
    {
        [JsonProperty("Spells")]
        public List<Skill> MageSkills { get; set; }
    }
    public class Skill
    {
        [JsonIgnore]
        public int ID { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("goldCost")]
        public int GoldCost { get; set; }

        [JsonProperty("abilityCost")]
        public int AbilityCost { get; set; }
        
        [JsonProperty("classTypes")]
        [NotMapped]
        public List<string> AllowedClasses //All this is required to send and receive lists from sql db
        {
            get => AllowedClassesData?.Split(',').ToList();
            set => AllowedClassesData = string.Join(',', value ?? new List<string>());
        }
        [JsonIgnore]
        public string AllowedClassesData { get; set; }
        
        [JsonProperty("effects")]
        public List<Effect> Effects { get; set; }
        //public List<PlayerSkill> PlayerSkills { get; set; }
    }
}
