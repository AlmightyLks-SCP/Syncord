using Newtonsoft.Json;
using SyncordBot.Models;
using System.Collections.Generic;
using System.IO;

namespace SyncordBot.Configs
{
    public sealed class GuildConfig
    {
        public List<DedicatedGuild> Guilds { get; init; }

        public GuildConfig()
        {
            Guilds = new List<DedicatedGuild>();
        }

        public static GuildConfig Load()
        {
            GuildConfig result = new GuildConfig();
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Configs");
            string configPath = Path.Combine(configDirectory, "Guild-Config.json");

            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);

            if (!File.Exists(configPath))
                File.WriteAllText(configPath, JsonConvert.SerializeObject(result, Formatting.Indented));
            else
                result = JsonConvert.DeserializeObject<GuildConfig>(File.ReadAllText(configPath));

            return result;
        }
    }
}
