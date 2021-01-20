namespace RpgApp.Shared.Types
{
    public abstract class LivingEntity
    {
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Initiative { get; set; }
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Toughness { get; set; }
        public int Speed { get; set; }
    }
}
