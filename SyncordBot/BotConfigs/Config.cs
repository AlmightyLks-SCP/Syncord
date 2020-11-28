namespace SyncordBot.BotConfigs
{
    public class Config
    {
        public string Prefix { get; set; }
        public string BotToken { get; set; }
        public int Port { get; set; }

        internal Config()
        {
            Prefix = "!";
            BotToken = "Your Bot Token here";
            Port = 8000;
        }
    }
}