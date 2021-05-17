using System.Linq;
using Newtonsoft.Json;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Shared.Types.PlayerExtensions
{
    public class CombatPlayer : Player
    {
        public int ArmorValue => GetArmorValue() + ArmorModifier;
        public int ArmorModifier { get; set; }

        public string DamageDice
        {
            get
            {
                var dice = GetDamageDice();
                return string.IsNullOrWhiteSpace(dice) ? "1D2" : dice;
            }
        } /*=> GetDamageDice();*/

        private int GetArmorValue()
        {
            var inventory = Inventory;
            var armor = inventory.Find(x => x.Index == BodyId);

            var armorValue = armor?.Effects.Where(x => x.Type == EffectType.Defend).Select(x => x.Value).FirstOrDefault();
            int value = 0;
            return int.TryParse(armorValue, out value) ? value : default;
        }

        private string GetDamageDice()
        {
            var inventory = Inventory;
            var weapon = Weapon; //inventory.FirstOrDefault(x => x.Id == WeaponHandId);
            return weapon?.Effects.Select(x => x.Value).FirstOrDefault();

        }
        public void EquipWeapon(int itemId)
        {
            WeaponHandId = itemId;
        }

        public void EquipArmor(int itemId)
        {
            BodyId = itemId;
        }

        public override string ToString()
        {
            var jsonSetting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.SerializeObject(this, Formatting.Indented, jsonSetting);
        }

        public string ClassString() => ClassAsString();
    }
}
