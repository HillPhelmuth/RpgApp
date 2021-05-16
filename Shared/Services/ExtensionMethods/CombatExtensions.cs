using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;
using RpgApp.Shared.Types.PlayerExtensions;

namespace RpgApp.Shared.Services.ExtensionMethods
{
    public static class CombatExtensions
    {
        public static int ToDiceValue(this string dice)
        {
            int damage = 0;
            if (!dice.Contains("D", StringComparison.OrdinalIgnoreCase))
            {
                int.TryParse(dice, out damage);
                return damage;
            }
            var diceRoller = new DiceRoller();
            var damageDiceValues = dice.ToUpper().Split('D');

            for (int i = 0; i < Convert.ToInt32(damageDiceValues[0]); i++)
            {
                damage += damageDiceValues[1] switch
                {
                    "2" => (diceRoller.RollD4() + 1) / 2,
                    "4" => diceRoller.RollD4(),
                    "6" => diceRoller.RollD6(),
                    "8" => diceRoller.RollD8(),
                    "10" => diceRoller.RollD10(),
                    "12" => diceRoller.RollD12(),
                    "20" => diceRoller.RollD20(),
                    _ => diceRoller.RollD6()
                };
            }
            return damage;
        }

        public static void ModifyAttribute(this object combatPlayer, string attribute, string value)
        {
            if (!(combatPlayer is CombatPlayer) && !(combatPlayer is Monster)) return;
            Type type = combatPlayer.GetType();
            PropertyInfo prop = type.GetProperty(attribute);
            var modifier = value.ToDiceValue();
            var currentVal = prop?.GetValue(combatPlayer) ?? 0;
            var propertyValue = combatPlayer is CombatPlayer ? (int)currentVal + modifier : (int)currentVal - modifier;
            prop?.SetValue(combatPlayer, propertyValue, null);
        }

        public static decimal GetModifier(this CombatPlayer player)
        {
            return player.ClassType switch
            {
                ClassType.Warrior => Math.Ceiling((decimal)player.Strength / 2),
                ClassType.Mage => Math.Ceiling((decimal)player.Intelligence / 2),
                ClassType.Ranger => Math.Ceiling((decimal)player.Agility / 2),
                _ => 0
            };
        }
    }
}
