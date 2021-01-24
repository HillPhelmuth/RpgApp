using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RpgApp.Client.Pages.StyleTests
{
    public partial class CompLibraryTester
    {
        private double life;
        private double mana;
        private double stamina;
        private bool showProgress;
        private bool showMenu;
        private List<TestEquipment> equipments = new();
        private List<KeyValuePair<string, TestEquipment>> ImagesEquipPairs = new();
        private List<string> TestLog = new();
        protected override Task OnInitializedAsync()
        {
            equipments = new List<TestEquipment>
{
            new("Sword", "This is an amazing sword", 10, "sword"),
            new("Shield", "This is an amazing Shield", 15,"shield"),
            new("Helmet", "This is just helmet", 5, "foreign/helmet"),
            new("Large Sword", "This is an amazing Large sword", 20, "foreign/swordWood"),
            new("Heavy Shield", "This is an amazing Heavy Shield", 25, "foreign/shield"),
            new("Plate Armor", "This is just some plate armor", 90,"foreign/armor"),
            new("Axe", "This is an amazing fucking Axe", 110,"foreign/axeDouble"),
            new("Hammer", "This is an amazing hammer", 10, "foreign/hammer"),
            new("dagger", "This is an amazing dagger", 15,"foreign/dagger"),
            new("Wand", "This is a magic wand", 500, "foreign/wand"),
            new("Tome", "This is an amazing magic book", 20, "foreign/tome"),
            new("scroll", "This is an amazing word scroll", 25, "foreign/scroll"),
            new("Bow", "This is just some kinda bow", 90,"foreign/bow"),
            new("Map", "This is just Map", 90,"foreign/map"),
            new("Blue Potion", "This is an amazing blue Potion", 110,"foreign/potionBlue"),
            new("Green Potion", "This is an amazing green Potion", 110,"foreign/potionGreen"),
            new("Red Potion", "This is an amazing red Potion", 110,"foreign/potionRed"),
            new("Blue Gem", "This is an amazing blue gen", 110,"foreign/gemBlue"),
            new("Green Gem", "This is an amazing green gem", 110,"foreign/gemGreen"),
            new("Red Gem", "This is an amazing red gem", 110,"foreign/gemRed"),
            new("Bow", "This is just some Heart thing", 90,"foreign/heart"),
            new("Empty", "This is an Empty head", 110,"helmet-slot"),
            new("Empty", "This is an Empty weapon", 110,"weapon-slot"),
            new("Empty", "This is an Empty magic", 110,"magic-slot"),
            new("Empty", "This is just missing feet", 90,"shoes-slot")


        };
            ImagesEquipPairs = AddImages(equipments);
            return base.OnInitializedAsync();
        }

        private void AddToLog(string info)
        {
            TestLog.Add(info);
            StateHasChanged();
        }
        private List<KeyValuePair<string, TestEquipment>> AddImages(List<TestEquipment> equipment)
        {
            var result = new List<KeyValuePair<string, TestEquipment>>();
            foreach (var equip in equipment)
            {
                var kvp = new KeyValuePair<string, TestEquipment>(equip.ImagePath, equip);
                result.Add(kvp);
            }
            return result;
        }
        private async Task UpdateBar(string meterNmae)
        {
            showProgress = true;
            await InvokeAsync(StateHasChanged);
            var random = new Random();
            double newVal = random.Next(1, 101);
            switch (meterNmae)
            {
                case "Life":
                    life = newVal;
                    break;
                case "Mana":
                    mana = newVal;
                    break;
                case "Stamina":
                    stamina = newVal;
                    break;
            }

            await InvokeAsync(StateHasChanged);
        }

        public class TestEquipment
        {
            public TestEquipment(string name, string description, int goldCost, string imagePath = "empty-slot")
            {
                Name = name;
                Description = description;
                GoldCost = goldCost;
                ImagePath = imagePath;
            }
            public string Name { get; set; }
            public string Description { get; set; }
            public int GoldCost { get; set; }
            public bool IsPurchased { get; set; }
            public string ImagePath { get; set; }

            public override string ToString()
            {
                return $"Name: {Name}\r\nDescription: {Description}\r\nPrice: {GoldCost}";
            }
        }
    }
}
