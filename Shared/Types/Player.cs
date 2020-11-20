using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Shared.Types
{
    public class Player : LivingEntity
    {
        #region Properties

        public int ID { get; set; }
        public string UserId { get; set; }
        public int Level { get; set; }
        public int MaxAbilityPoints { get; set; }
        public int AbilityPoints { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public ClassType ClassType { get; set; }
        [JsonIgnore]
        public List<Skill> Skills { get; set; }
        //[NotMapped] 
        //public List<Skill> SkillList => Skills.Select(x => x.Skill).ToList();
        [JsonIgnore]
        public List<Equipment> Inventory { get; set; }
        //[NotMapped]
        //public List<Equipment> InventoryList => Inventory.Select(x => x.Equipment).ToList();

        public int? WeaponHandId { get; protected set; }
        public int? OffHandId { get; protected set; }
        public int? BodyId { get; protected set; }
        public int? FeetId { get; protected set; }
        public int? HeadId { get; protected set; }

        #endregion

        #region public Methods
        public static implicit operator string(Player player)
        {
            var jsonSetting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.SerializeObject(player, Formatting.Indented, jsonSetting);
        }
        public void EquipWeapon(Equipment item)
        {
            var stringClass = ClassAsString();
            if (!item.EquipLocation.Contains("Hand")) return;
            if (item.AllowedClasses.All(x => x != stringClass))
                return;
            switch (item.EquipLocation)
            {
                case "OneHand":
                    WeaponHandId = item.Id;
                    break;
                case "TwoHand":
                    WeaponHandId = item.Id;
                    OffHandId = item.Id;
                    break;
            }
        }
        public void EquipOffHand(Equipment item)
        {
            string stringClass = ClassAsString();
            if (item.EquipLocation == null || !item.EquipLocation.Contains("OffHand")) return;
            if (item.AllowedClasses.All(x => x != stringClass))
                return;
            OffHandId = item.Id;
        }
        public void EquipArmor(Equipment item)
        {
            if (item == null) return;
            var stringClass = ClassAsString();
            if (item.EquipLocation?.Contains("Body") == false) return;
            if (item.AllowedClasses.All(x => x != stringClass))
                return;
            BodyId = item.Id;
        }

        public void AddToInventory(Equipment item)
        {
            Inventory.Add(item);
        }

        #endregion

        #region private methods

        protected string ClassAsString()
        {
            return ClassType switch
            {
                ClassType.Mage => "Mage",
                ClassType.Warrior => "Warrior",
                ClassType.Ranger => "Ranger",
                _ => ""
            };
        }

        #endregion
    }
}
