using System.Threading.Tasks;
using TurnBasedRpg.Services;
using TurnBasedRpg.Types.Enums;

namespace TurnBasedRpg.Types
{
    public class CreateCharacter
    {
        public Task<Player> CreateNewCharacter(ClassType type)
        {
            var newPlayer = new Player();
            DiceRoller diceRoller = new DiceRoller();
            newPlayer.Level = 1;

            switch (type)
            {
                case ClassType.Mage:
                   
                    newPlayer.Strength = 10 - diceRoller.RollD4();
                    newPlayer.Intelligence = 10 + diceRoller.RollD8();
                    newPlayer.Toughness = 10;
                    newPlayer.Agility = 10;
                    newPlayer.Speed = 10;
                    newPlayer.MaxAbilityPoints = newPlayer.Intelligence * 2;
                    break;
                case ClassType.Warrior:
                    newPlayer.Strength = 10 + diceRoller.RollD8();
                    newPlayer.Intelligence = 10 - diceRoller.RollD4();
                    newPlayer.Toughness = 10;
                    newPlayer.Agility = 10;
                    newPlayer.Speed = 10;
                    newPlayer.MaxAbilityPoints = newPlayer.Strength * 2;
                    break;
                case ClassType.Ranger:
                    newPlayer.Strength = 10 ;
                    newPlayer.Intelligence = 10;
                    newPlayer.Toughness = 10 - diceRoller.RollD4();
                    newPlayer.Agility = 10 + diceRoller.RollD4();
                    newPlayer.Speed = 10 + diceRoller.RollD4();
                    newPlayer.MaxAbilityPoints = newPlayer.Agility + newPlayer.Speed;
                    break;
            }
            newPlayer.MaxHealth = newPlayer.Toughness * 10;
            newPlayer.Health = newPlayer.MaxHealth;
            newPlayer.AbilityPoints = newPlayer.MaxAbilityPoints;
            newPlayer.ClassType = type;
            newPlayer.Gold = 50;
            return Task.FromResult(newPlayer);
        }
    }
}