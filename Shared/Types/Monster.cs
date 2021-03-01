using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RpgApp.Shared.Types
{
    public class Monster : LivingEntity
    {
        public int Id { get; set; }
        public List<Equipment> Treasure { get; set; }
        public string DamageDice { get; set; }
        public string Description { get; set; }
        public int ExpProvided => DifficultyLevel * DifficultyLevel * 2;
        public int GoldProvided => DifficultyLevel * DifficultyLevel * 10;
        public int DifficultyLevel { get; set; }
        public int Armor => DifficultyLevel;
        [NotMapped]
        public bool IsHit { get; set; }
        [NotMapped]
        public bool isDead { get; set; }
        [NotMapped]
        public bool isAttack { get; set; }

        //ToDo Add factory method to generate Monsters
        //ToDo Add private method to calculate exp provided
    }
}
