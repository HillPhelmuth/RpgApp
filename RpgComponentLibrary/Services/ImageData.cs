using System.Collections.Generic;

namespace RpgComponentLibrary.Services
{
    public static class ImageData
    {
        public static Dictionary<int,string> IndexedImages => new()
        {
            {1, "sword"}, {2, "shield"}, {3, "foreign/helmet"}, {4, "foreign/swordWood"},
            {5, "foreign/shield"}, {6, "foreign/armor"}, {7, "foreign/axeDouble"}, {8, "foreign/hammer"},
            {9, "foreign/dagger"}, {10, "foreign/wand"}, {11, "foreign/tome"}, {12, "foreign/scroll"},
            {13, "foreign/bow"}, {14, "foreign/map"}, {15, "foreign/potionBlue"}, {16, "foreign/potionGreen"},
            {17, "foreign/potionRed"}, {18, "foreign/gemBlue"}, {19, "foreign/gemGreen"}, {20, "foreign/gemRed"},
            {21, "foreign/heart"}, {22, "helmet-slot"}, {23, "weapon-slot"}, {24, "magic-slot"}, {25, "shoes-slot"}
        };
        public static Dictionary<int, string> MageSkills => new()
        {
            { 1, "basicheal" },
            { 2, "enfeeble" },
            { 3, "idiocy" },
            { 4, "magicmissile" },
            { 5, "slow" },
            { 6, "vertigo" },
            { 7, "vulnerability" }
        };
        public static Dictionary<int, string> RangerSkills => new()
        {
            { 1, "camouflage" },
            { 2, "doubleshot" },
            { 3, "settrap" }
        };
        public static Dictionary<int, string> WarriorSkills => new()
        {
            { 1, "cleave" },
            { 2, "courage" },
            { 3, "devastate" },
            { 4, "enrage" },
            { 5, "overpower" },
            { 6, "rally" },
            { 7, "whirlwind" }
        };
    }
}
