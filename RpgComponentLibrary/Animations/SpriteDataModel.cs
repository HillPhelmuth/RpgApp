using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RpgComponentLibrary.Animations
{
    public class SpriteDataModel
    {
        [JsonPropertyName("frames")]
        public List<Frame> Frames { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("imgUrl")]
        public string ImgUrl { get; set; }
        [JsonPropertyName("imageName")]
        public string ImageName { get; set; }

    }

    public partial class Frame
    {
        [JsonPropertyName("index")]
        public int Index { get; set; }

        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }

        [JsonPropertyName("w")]
        public int W { get; set; }

        [JsonPropertyName("h")]
        public int H { get; set; }
    }

    public enum TopViewAvatar
    {
        Green,
        Pink,
        Boyle,
        Cop,
        BurgerKing,
        ChearLeader
    }
}
