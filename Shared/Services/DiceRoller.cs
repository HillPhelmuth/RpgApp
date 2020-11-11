using System;

namespace TurnBasedRpg.Services
{
    public class DiceRoller
    {
        private readonly Random _random;

        public DiceRoller() => _random = new Random();
        public int RollD20() => _random.Next(1, 21);

        public int RollD10() => _random.Next(1, 11);

        public int RollD12() => _random.Next(1, 13);

        public int RollD6() => _random.Next(1, 7);

        public int RollD6Multiple(int diceNumber)
        {
            int rollTotal = 0;
            for (int i = 0; i < diceNumber; i++)
            {
                rollTotal += _random.Next(1, 7);
            }
            return rollTotal;
        }
        public int RollPercentile() => _random.Next(1, 101);

        public int RollD4() => _random.Next(1, 5);

        public int RollD8() => _random.Next(1, 9);
    }
}
