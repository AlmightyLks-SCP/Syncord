using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.Configs.EmbedConfigs
{
    public struct PlayerJoinedLeftConfig
    {
        [JsonProperty("Show User ID")]
        public bool ShowUserId { get; set; }
        [JsonProperty("Show Ping")]
        public bool ShowPing { get; set; }
        [JsonProperty("Show IP (or Do Not Track)")]
        public bool ShowIP { get; set; }
    }
}
