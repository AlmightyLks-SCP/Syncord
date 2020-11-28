using System.Collections.Generic;

namespace SyncordBot.BotConfigs
{
    public class Config
    {
        public string Prefix { get; set; } = "!";
        public string BotToken { get; set; } = "Your Bot Token here";
        public int Port { get; set; } = 8000;
        public List<DedicatedGuild> Guilds { get; set; } = new List<DedicatedGuild>() { new DedicatedGuild() };
    }
}