using DSharpPlus.Entities;
using Newtonsoft.Json;
using SyncordBot.Configs.EmbedConfigs;
using SyncordBot.Configs.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace SyncordBot.Configs
{
    public sealed class BotConfig
    {
        public string Prefix { get; set; }
        public string BotToken { get; set; }
        public ushort Port { get; set; }
        public bool RemoteConnection { get; set; }
        public DiscordActivityConfig DiscordActivity { get; set; }
        public EmbedConfig EmbedConfigs { get; set; }
        public BotConfig()
        {
            Prefix = "!";
            BotToken = "Your Bot Token here";
            Port = 8000;
            RemoteConnection = false;
            DiscordActivity = new DiscordActivityConfig()
            {
                Activity = ActivityType.Watching,
                Name = "{SLCount} Server/s"
            };
            EmbedConfigs = new EmbedConfig();
        }

        public static BotConfig Load()
        {
            BotConfig result = new BotConfig();
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Configs");
            string configPath = Path.Combine(configDirectory, "Bot-Config.json");

            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);

            if (!File.Exists(configPath))
                File.WriteAllText(configPath, JsonConvert.SerializeObject(result));
            else
                result = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(configPath));

            return result;
        }
    }
}