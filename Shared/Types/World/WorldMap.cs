using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TurnBasedRpg.Types.Enums;

namespace TurnBasedRpg.Types.World
{
    public class WorldMap
    {
        public int ID { get; set; }
        public string MapName { get; set; }
        public string BackgroundUrl { get; set; }
        public MapTile[,] GridMap { get; set; }

        public WorldMap(string mapName, string background)
        {
            MapName = mapName;
            BackgroundUrl = background;
            GridMap = new MapTile[12,12];
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    GridMap[i,j] = new MapTile {XCoordinate = i, YCoordinate = j};
                }
            }
        }

    }

}
