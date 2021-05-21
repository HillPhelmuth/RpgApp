using System.Collections.Generic;

namespace RpgApp.Shared.Types
{
    public class AllAppData
    {
        public List<Monster> Monsters { get; set; } = new();
        public List<Skill> Skills { get; set; } = new();
        public List<Equipment> Equipment { get; set; } = new();
        public override string ToString()
        {
            return $"Monster-{Monsters.Count}, Skills-{Skills.Count}, Equipment-{Equipment.Count}";
        }
    }

}
