using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
