using Newtonsoft.Json;
using RpgApp.Shared.Types.Enums;

namespace RpgApp.Shared.Types
{
    public class Effect
    {
        [JsonIgnore]
        public int ID { get; set; }
        
        [JsonProperty("type")]
        public EffectType Type { get; set; }

        [JsonProperty("area", NullValueHandling = NullValueHandling.Ignore)]
        public int? Area { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("attribute", NullValueHandling = NullValueHandling.Ignore)]
        public string Attribute { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public int? Duration { get; set; }
        

    }
}
