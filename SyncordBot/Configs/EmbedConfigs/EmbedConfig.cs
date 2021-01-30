namespace SyncordBot.Configs.EmbedConfigs
{
    public class EmbedConfig
    {
        public PlayerJoinedLeftConfig PlayerJoinedLeftConfig { get; set; }

        public EmbedConfig()
        {
            PlayerJoinedLeftConfig = new PlayerJoinedLeftConfig()
            {
                ShowIP = false,
                ShowUserId = true,
                ShowPing = true
            };
        }
    }
}
