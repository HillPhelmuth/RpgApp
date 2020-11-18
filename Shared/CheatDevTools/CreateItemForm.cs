using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Shared.CheatDevTools
{
    public class CreateItemForm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Rarity Rarity { get; set; }
        public Rarity[] RarityItems = Enum.GetValues(typeof(Rarity)).Cast<Rarity>().ToArray();

        [Required]
        public string Description { get; set; }

        [Required]
        public string EquipLocation { get; set; }

       
        public bool? AreaEffect { get; set; }

        [Required]
        public int GoldCost { get; set; }
        [Required]
        public EffectType Type { get; set; }
        
        public int? Area { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string AllowedClassesData { get; set; }

        public string Attribute { get; set; }

        
        public int? Duration { get; set; }
    }
}
