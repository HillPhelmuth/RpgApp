using Newtonsoft.Json;

namespace RpgApp.Shared.Types
{
    /// <summary>
    /// This class represents the Many to Many data relationship between Equipment and Player tables and exists
    /// solely facilitate that data structure.
    /// </summary>
    public class PlayerEquipment
    {
        [JsonIgnore]
        public int PlayerID { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
    }
}
