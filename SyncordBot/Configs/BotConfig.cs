using DSharpPlus.Entities;
using Newtonsoft.Json;
using SyncordBot.Configs.EmbedConfigs;
using SyncordBot.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace SyncordBot.Configs
{
    public sealed class BotConfig
    {
        public string Prefix { get; init; }
        [JsonProperty("Bot Token")]
        public string BotToken { get; init; }
        public ushort Port { get; init; }
        [JsonProperty("Discord Activity")]
        public DiscordActivityConfig DiscordActivity { get; init; }
        [JsonProperty("Embed Configs")]
        public EmbedConfig EmbedConfigs { get; init; }
        public bool DebugMode { get; set; }

        public BotConfig()
        {
            Prefix = "!";
            BotToken = "Your Bot Token here";
            Port = 8000;
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
                File.WriteAllText(configPath, JsonConvert.SerializeObject(result, Formatting.Indented));
            else
                result = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(configPath));

            return result;
        }
    }
}