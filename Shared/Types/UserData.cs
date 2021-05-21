using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RpgApp.Shared.Types
{
    public class UserData
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("players")]
        public List<Player> Players { get; set; } = new();
    }
}
