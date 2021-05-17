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

        public int Index { get; set; }
        public string UserId { get; set; }
        public int Level { get; set; }
        public int MaxAbilityPoints { get; set; }
        public int AbilityPoints { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public ClassType ClassType { get; set; }
        [JsonIgnore]
        public List<Skill> Skills { get; set; }
        public List<Equipment> Inventory { get; set; }
        [JsonIgnore]
        [NotMapped]
        public Equipment Weapon { get; set; }
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
                    WeaponHandId = item.Index;
                    break;
                case "TwoHand":
                    WeaponHandId = item.Index;
                    OffHandId = item.Index;
                    break;
            }

            Weapon = item;
        }
        public void EquipOffHand(Equipment item)
        {
            string stringClass = ClassAsString();
            if (item.EquipLocation == null || !item.EquipLocation.Contains("OffHand")) return;
            if (item.AllowedClasses.All(x => x != stringClass))
                return;
            OffHandId = item.Index;
        }
        public void EquipArmor(Equipment item)
        {
            if (item == null) return;
            var stringClass = ClassAsString();
            if (item.EquipLocation?.Contains("Body") == false) return;
            if (item.AllowedClasses.All(x => x != stringClass))
                return;
            BodyId = item.Index;
        }

        public void AddToInventory(Equipment item)
        {
            Inventory ??= new List<Equipment>();
            Inventory.Add(item);
        }

        public void Refresh()
        {
            Health = MaxHealth;
            AbilityPoints = MaxAbilityPoints;
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
