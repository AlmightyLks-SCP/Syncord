using DSharpPlus.Entities;
using SyncordBot.Configs.EmbedConfigs;
using System.Collections.Generic;

namespace SyncordBot.Configs.BotConfigs
{
    public class Config
    {
        public string Prefix { get; set; }
        public string BotToken { get; set; }
        public ushort Port { get; set; }
        public DiscordActivityConfig DiscordActivity { get; set; }
        public List<DedicatedGuild> Guilds { get; set; }
        public EmbedConfig EmbedConfigs { get; set; }
        public Config()
        {
            Prefix = "!";
            BotToken = "Your Bot Token here";
            Port = 8000;
            Guilds = new List<DedicatedGuild>();
            EmbedConfigs = new EmbedConfig();
            DiscordActivity = new DiscordActivityConfig()
            {
                Activity = ActivityType.Watching,
                Name = "{SLCount} Server/s"
            };
        }
    }
}