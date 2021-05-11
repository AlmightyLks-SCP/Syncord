using Newtonsoft.Json;

namespace SyncordBot.Configs.EmbedConfigs
{
    public struct PlayerDeathConfig
    {
        [JsonProperty("Show User ID")]
        public bool ShowUserId { get; set; }
    }
}
