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
    }
}
