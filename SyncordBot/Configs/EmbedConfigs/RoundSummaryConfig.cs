using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.Configs.EmbedConfigs
{
    public struct RoundSummaryConfig
    {
        [JsonProperty("Show Round Length")]
        public bool ShowRoundLength { get; init; }
        [JsonProperty("Show Total Kills")]
        public bool ShowTotalKills { get; init; }
        [JsonProperty("Show Total Scp Kills")]
        public bool ShowTotalScpKills { get; init; }
        [JsonProperty("Show Total Frag Grenade Kills")]
        public bool ShowTotalFragGrenadeKills { get; init; }
        [JsonProperty("Show Total Escaped DClass")]
        public bool ShowTotalEscapedDClass { get; init; }
        [JsonProperty("Show Total Escaped Scientists")]
        public bool ShowTotalEscapedScientists { get; init; }
    }
}
