using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Server.Data
{
    public class RpgDbInitializer
    {
        /// <summary>
        /// Checks to see if the database exists. If it doesn't it creates it with some data. This is called
        /// from Program.cs on app start.
        /// </summary>
        /// <param name="context"></param>
        public static void Initialize(RpgDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Players.Any() || context.Equipment.Any())
                return;
            // Going to create the Equipment with the .json file by making it an embedded resource so I can
            // use Reflection to grab it's contents at runtime.
            var assembly = Assembly.GetExecutingAssembly(); // Gets all embedded and compiled files in the program
            var equipResourceName = assembly.GetManifestResourceNames()
                .SingleOrDefault(s => s.EndsWith("EquipmentList.json"));// finds the json file

            string result = "";
            //Code block below reads the information in the json file into memory so I can use it as a .net object.
            using var stream = assembly.GetManifestResourceStream(equipResourceName);
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            // takes our json file text and converts or "deserializes" the json into a list of our Equipment class
            var equipmentList = JsonConvert.DeserializeObject<EquipmentList>(result);

            // do the same for our Skill jsons
            var skillsResourceNames = assembly.GetManifestResourceNames().Where(x => x.EndsWith("Skills.json"));
            var allSkills = new List<Skill>();
            foreach (var resourceName in skillsResourceNames)
            {
                string skillResult = "";
                using var skillStream = assembly.GetManifestResourceStream(resourceName);
                using (var reader = new StreamReader(skillStream))
                {
                    skillResult = reader.ReadToEnd();
                }

                if (resourceName.Contains("Mage"))
                {
                    var mageSkills = JsonConvert.DeserializeObject<MageSkillList>(skillResult);
                    allSkills.AddRange(mageSkills.MageSkills);
                }
                if (resourceName.Contains("Warrior"))
                {
                    var warriorSkills = JsonConvert.DeserializeObject<WarriorSkillList>(skillResult);
                    allSkills.AddRange(warriorSkills.WarriorSkills);
                }
                if (resourceName.Contains("Ranger"))
                {
                    var rangerSkills = JsonConvert.DeserializeObject<RangerSkillList>(skillResult);
                    allSkills.AddRange(rangerSkills.RangerSkills);
                }
            }


            // add each equipment item to our database Equipment table
            foreach (var equipment in equipmentList.Equipments)
            {
                //context = database, Equipment = Epuipment table
                context.Equipment.Add(equipment);
            }
            // add each skill item to our database Skill table
            foreach (var skill in allSkills)
            {
                context.Skills.Add(skill);
            }
            var dagger = equipmentList.Equipments.FirstOrDefault(x => x.Name == "Dagger");
            var enrage = allSkills.FirstOrDefault(x => x.Name == "Enrage");
            var magicMissile = allSkills.FirstOrDefault(x => x.Name == "Magic missile");
            var doubleShot = allSkills.FirstOrDefault(x => x.Name == "Double shot");
            var create = new CreateCharacter();
            // Now we create a player to seed the Players table
            var warrior = create.CreateNewCharacter(ClassType.Warrior).Result;
            warrior.UserId = "admin";
            warrior.Name = "Seed Warrior";
            warrior.Inventory = new List<Equipment> { dagger };
            warrior.Skills = new List<Skill> { enrage };
            warrior.Gold = 50;
            var mage = create.CreateNewCharacter(ClassType.Mage).Result;
            mage.UserId = "admin";
            mage.Name = "Seed Mage";
            mage.Inventory = new List<Equipment> { dagger };
            mage.Skills = new List<Skill> { magicMissile };
            mage.Gold = 50;
            var ranger = create.CreateNewCharacter(ClassType.Ranger).Result;
            ranger.UserId = "admin";
            ranger.Name = "Seed Ranger";
            ranger.Inventory = new List<Equipment> { dagger };
            ranger.Skills = new List<Skill> { doubleShot };
            ranger.Gold = 50;
            // and add it to our Players table
            context.Players.AddRange(warrior, mage, ranger);
            var monster = new Monster
            {
                Name = "Tiny Pirate",
                Description = "Being both tiny and a pirate, their monstrosity was obvious",
                MaxHealth = 50,
                Health = 50,
                DifficultyLevel = 1,
                Agility = 10,
                Strength = 6,
                Intelligence = 5,
                Toughness = 5,
                Speed = 10,
                DamageDice = "1D4"

            };
            var monster2 = new Monster
            {
                Name = "Big ol' Fatty",
                Description = "Hard to bring down, but slow as hell",
                MaxHealth = 100,
                Health = 100,
                DifficultyLevel = 1,
                Agility = 10,
                Strength = 6,
                Intelligence = 5,
                Toughness = 5,
                Speed = 3,
                DamageDice = "1D4"

            };
            var monster3 = new Monster
            {
                Name = "Small Fast Monster",
                Description = "I don't feel like thinking of a name or description for this",
                MaxHealth = 20,
                Health = 20,
                DifficultyLevel = 1,
                Agility = 10,
                Strength = 6,
                Intelligence = 5,
                Toughness = 5,
                Speed = 20,
                DamageDice = "1D4"

            };
            context.Monsters.AddRange(monster, monster2, monster3);
            //Saving will write all these changes to the db
            context.SaveChanges();
        }
    }
}
