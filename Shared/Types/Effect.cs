//using Newtonsoft.Json;

using System.Text.Json.Serialization;
using RpgApp.Shared.Types.Enums; //using Newtonsoft.Json;

namespace RpgApp.Shared.Types
{
    public class Effect
    {
        [JsonIgnore]
        public int ID { get; set; }

        [JsonPropertyName("type")]
        public EffectType Type { get; set; }

        [JsonPropertyName("areaEffect")]
        public int? Area { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("attribute")]
        public string Attribute { get; set; }

        [JsonPropertyName("duration")]
        public int? Duration { get; set; }


    }
}
