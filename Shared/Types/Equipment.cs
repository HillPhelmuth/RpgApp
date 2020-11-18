using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Shared.Types
{
    
    public class EquipmentList
    {
        [JsonProperty("Equipment")]
        public List<Equipment> Equipments { get; set; }
    }
    public class Equipment
    {
        //[JsonProperty("id")]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rarity")]
        public Rarity Rarity { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("equipLocation")]
        public string EquipLocation { get; set; }

        [JsonProperty("areaEffect", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AreaEffect { get; set; }

        [JsonProperty("goldCost")]
        public int GoldCost { get; set; }

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
        //[JsonIgnore]
        //public List<PlayerEquipment> PlayerEquipments { get; set; }
        [NotMapped]
        public bool IsEquipped { get; set; }
        
    }
}
