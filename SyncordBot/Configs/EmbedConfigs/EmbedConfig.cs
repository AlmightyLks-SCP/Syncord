using Newtonsoft.Json;

namespace SyncordBot.Configs.EmbedConfigs
{
    public class EmbedConfig
    {
        [JsonProperty("Display Server IP / Alias")]
        public bool DisplayServerIpOrAlias { get; init; }
        [JsonProperty("Player Joined / Left")]
        public PlayerJoinedLeftConfig PlayerJoinedLeftConfig { get; init; }
        [JsonProperty("Player Death")]
        public PlayerDeathConfig PlayerDeathConfig { get; init; }
        [JsonProperty("Round Summary")]
        public RoundSummaryConfig RoundSummaryConfig { get; init; }

        public EmbedConfig()
        {
            PlayerJoinedLeftConfig = new PlayerJoinedLeftConfig()
            {
                ShowIP = false,
                ShowUserId = true,
                ShowPing = true
            };
            PlayerDeathConfig = new PlayerDeathConfig()
            {
                ShowUserId = true
            };
            RoundSummaryConfig = new RoundSummaryConfig()
            {
                ShowRoundLength = true,
                ShowTotalKills = true,
                ShowTotalScpKills = true,
                ShowTotalFragGrenadeKills = true,
                ShowTotalEscapedDClass = true,
                ShowTotalEscapedScientists = true
            };
        }
    }
}
