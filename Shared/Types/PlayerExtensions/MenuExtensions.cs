using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RpgComponentLibrary.Services;

namespace RpgApp.Shared.Types.PlayerExtensions
{
    public static class MenuExtensions
    {
        public static KeyValuePair<string, Equipment> AddImagePath(this Equipment equipment, int id = 0)
        {
            var random = new Random();
            id = id == 0 ? random.Next(1, 26) : id;
            return new KeyValuePair<string, Equipment>(ImageData.IndexedImages[id], equipment);
        }
        public static KeyValuePair<string, Skill> AddImagePath(this Skill skill, int id = 0)
        {
            var random = new Random();
            id = id == 0 ? random.Next(1, 26) : id;
            return new KeyValuePair<string, Skill>(ImageData.IndexedImages[id], skill);
        }
        public static string AddImage(this Skill skill, int id = 0)
        {
            var random = new Random();
            id = id == 0 ? random.Next(1, 26) : id;
            return ImageData.IndexedImages[id];
        }
        public static string AsDisplayString(this Equipment equip)
        {
            return $"Name: {equip.Name}\r\nDescription: {equip.Description}\r\nPrice: {equip.GoldCost}gp";
        }

        public static string AsDisplayString(this Skill skill)
        {
            return $"Name: {skill.Name}\r\nDescription: {skill.Description}\r\n Price:{skill.GoldCost}gp";
        }
    }
}
