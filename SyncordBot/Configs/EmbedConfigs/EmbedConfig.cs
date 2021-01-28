namespace SyncordBot.Configs.EmbedConfigs
{
    public class EmbedConfig
    {
        public PlayerJoinedConfig PlayerJoinedConfig { get; set; }

        public EmbedConfig()
        {
            PlayerJoinedConfig = new PlayerJoinedConfig()
            {
                ShowIP = false,
                ShowUserId = true,
                ShowPing = true
            };
        }
    }
}
