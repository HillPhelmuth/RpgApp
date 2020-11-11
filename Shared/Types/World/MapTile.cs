using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurnBasedRpg.Types.Enums;

namespace TurnBasedRpg.Types.World
{
    public class MapTile
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string MapMarker { get; set; } // probably become an enum later
        public string ImageUrl { get; set; }
    }
}
