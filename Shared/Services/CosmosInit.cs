using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using RpgApp.Shared.Types;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Shared.Services
{
    public class CosmosInit
    {
        public static Player CreatePlayer(string name, Equipment weapon, Equipment armor, Skill skill, ClassType type = ClassType.Warrior)
        {
            var warrior = CreateCharacter.CreateNewCharacter(type).Result;
            warrior.UserId = "admin";
            warrior.Name = name;
            warrior.Inventory = new List<Equipment> { weapon, armor };
            warrior.Skills = new List<Skill> { skill };
            warrior.Gold = 50;
            return warrior;
        }

        public static List<Monster> CreateMonsters()
        {
            return new()
            {
                new Monster
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

                },
                new Monster
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

                },
                new Monster
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

                }
            };
        }
        public static List<Skill> AllSkills()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var skillsResourceNames = assembly.GetManifestResourceNames().Where(x => x.EndsWith("Skills.json"));
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
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
                    var mageSkills = JsonSerializer.Deserialize<MageSkillList>(skillResult, options);
                    allSkills.AddRange(mageSkills.MageSkills);
                }

                if (resourceName.Contains("Warrior"))
                {
                    var warriorSkills = JsonSerializer.Deserialize<WarriorSkillList>(skillResult, options);
                    allSkills.AddRange(warriorSkills.WarriorSkills);
                }

                if (resourceName.Contains("Ranger"))
                {
                    var rangerSkills = JsonSerializer.Deserialize<RangerSkillList>(skillResult, options);
                    allSkills.AddRange(rangerSkills.RangerSkills);
                }
            }

            return allSkills;
        }

        public static EquipmentList EquipmentList()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            var equipResourceName = assembly.GetManifestResourceNames()
                .SingleOrDefault(s => s.EndsWith("EquipmentList.json")); // finds the json file

            string result = "";
            //Code block below reads the information in the json file into memory so I can use it as a .net object.
            using var stream = assembly.GetManifestResourceStream(equipResourceName);
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            // takes our json file text and converts or "deserializes" the json into a list of our Equipment class
            var equipmentList = JsonSerializer.Deserialize<EquipmentList>(result, options);
            return equipmentList;
        }
    }
}
