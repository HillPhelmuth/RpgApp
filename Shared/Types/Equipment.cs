using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using RpgApp.Shared.Types.Enums; //using Newtonsoft.Json;

namespace RpgApp.Shared.Types
{

    public class EquipmentList
    {
        [JsonPropertyName("Equipment")]
        public List<Equipment> Equipments { get; set; }
    }
    public class Equipment : IEquatable<Equipment>
    {
        //[JsonPropertyName("id")]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("rarity")]
        public Rarity Rarity { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("equipLocation")]
        public string EquipLocation { get; set; }

        [JsonPropertyName("areaEffect")]
        public bool? AreaEffect { get; set; }

        [JsonPropertyName("goldCost")]
        public int GoldCost { get; set; }

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
        [NotMapped]
        public bool IsEquipped { get; set; }
        [JsonIgnore]
        public ICollection<Player> Players { get; set; }


        public bool Equals(Equipment other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Description == other.Description && EquipLocation == other.EquipLocation && GoldCost == other.GoldCost;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Equipment) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Description, EquipLocation, GoldCost);
        }
    }
}
