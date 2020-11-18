namespace RpgApp.Shared.Types.World
{
    public class MapTile
    {
        public int XCoordinate { get; set; }
        public int YCoordinate { get; set; }
        public string MapMarker { get; set; } // probably become an enum later
        public string ImageUrl { get; set; }
    }
}
