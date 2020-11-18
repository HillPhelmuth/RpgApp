using System.ComponentModel.DataAnnotations;

namespace RpgApp.Shared.CheatDevTools
{
    public class CreateMonsterForm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int MaxHealth { get; set; }
        [Required]
        [Range(3, 50, ErrorMessage = "Enter a value between 3 and 50")]
        public int Intelligence { get; set; }
        [Required]
        [Range(3, 50, ErrorMessage = "Enter a value between 3 and 50")]
        public int Strength { get; set; }
        [Required]
        [Range(3, 50, ErrorMessage = "Enter a value between 3 and 50")]
        public int Agility { get; set; }
        [Required]
        [Range(3, 50, ErrorMessage = "Enter a value between 3 and 50")]
        public int Toughness { get; set; }
        [Required]
        [Range(3, 50, ErrorMessage = "Enter a value between 3 and 50")]
        public int Speed { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Enter a value between 3 and 20")]
        public int DifficultyLevel { get; set; }
        [Required(ErrorMessage = "Add damage dice.")]
        public string DamageDice { get; set; }
    }
}
