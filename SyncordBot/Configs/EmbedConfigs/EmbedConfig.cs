namespace SyncordBot.Configs.EmbedConfigs
{
    public class EmbedConfig
    {
        public PlayerJoinedLeftConfig PlayerJoinedLeftConfig { get; set; }
        public PlayerDeathConfig PlayerDeathConfig { get; set; }
        public RoundEndConfig RoundEndConfig { get; set; }

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
            RoundEndConfig = new RoundEndConfig()
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
