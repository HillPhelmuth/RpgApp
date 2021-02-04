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
        
    }

    public partial class Frame
    {
        [JsonPropertyName("index")]
        public long Index { get; set; }

        [JsonPropertyName("x")]
        public long X { get; set; }

        [JsonPropertyName("y")]
        public long Y { get; set; }

        [JsonPropertyName("w")]
        public long W { get; set; }

        [JsonPropertyName("h")]
        public long H { get; set; }
    }
    //public class Specs
    //{
    //    [JsonPropertyName("x")]
    //    public double X { get; set; }

    //    [JsonPropertyName("y")]
    //    public double Y { get; set; }

    //    [JsonPropertyName("w")]
    //    public double W { get; set; }

    //    [JsonPropertyName("h")]
    //    public double H { get; set; }
    //}
}
