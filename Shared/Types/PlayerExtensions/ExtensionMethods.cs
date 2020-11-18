using System.Collections.Generic;

namespace RpgApp.Shared.Types.PlayerExtensions
{
    public static class ExtensionMethods
    {
        public static Player ApplyCombatResults(this Player currentPlayer, CombatPlayer combatPlayer)
        {
            currentPlayer.Health = combatPlayer.Health;
            currentPlayer.AbilityPoints = combatPlayer.AbilityPoints;
            currentPlayer.Experience += combatPlayer.Experience;
            currentPlayer.Gold += combatPlayer.Gold;
            return currentPlayer;
        }

        public static Player UpdateDuringCombat(this Player currentPlayer, CombatPlayer combatPlayer)
        {
            currentPlayer.Health = combatPlayer.Health;
            currentPlayer.AbilityPoints = combatPlayer.AbilityPoints;
            return currentPlayer;
        }

        public static CombatPlayer ConvertToCombatMode(this Player player)
        {
            var combatPlayer = new CombatPlayer
            {
                AbilityPoints = player.AbilityPoints,
                Agility = player.Agility,
                Intelligence = player.Intelligence,
                Speed = player.Speed,
                Strength = player.Strength,
                Toughness = player.Toughness,
                ClassType = player.ClassType,
                Experience = player.Experience,
                Gold = player.Gold,
                Health = player.MaxHealth,
                MaxHealth = player.MaxHealth,
                MaxAbilityPoints = player.MaxAbilityPoints,
                Inventory = player.Inventory,
                Name = player.Name,
                Skills = player.Skills,
                Initiative = 0
            };
            if (player.WeaponHandId != null) 
                combatPlayer.EquipWeapon((int) player.WeaponHandId);
            if (player.BodyId != null)
                combatPlayer.EquipArmor((int)player.BodyId);
            return combatPlayer;
        }

        public static void AddToInventory(this Player player, Equipment item)
        {
            player.Inventory ??= new List<Equipment>();
            player.Inventory.Add(item);
        }
    }
}
