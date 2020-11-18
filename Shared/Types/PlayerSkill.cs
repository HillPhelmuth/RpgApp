using Newtonsoft.Json;

namespace RpgApp.Shared.Types
{
    /// <summary>
    /// This class represents the Many to Many data relationship between Skill and Player tables and exists
    /// solely facilitate that data structure.
    /// </summary>
    public class PlayerSkill
    {
        [JsonIgnore]
        public int PlayerID { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }
        public int SkillID { get; set; }
        public Skill Skill { get; set; }
    }
}
