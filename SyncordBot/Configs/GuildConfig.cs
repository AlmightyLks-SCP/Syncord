using Newtonsoft.Json;
using SyncordBot.Configs.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.Configs
{
    public sealed class GuildConfig
    {
        public List<DedicatedGuild> Guilds { get; set; }

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
                File.WriteAllText(configPath, JsonConvert.SerializeObject(result));
            else
                result = JsonConvert.DeserializeObject<GuildConfig>(File.ReadAllText(configPath));

            return result;
        }
    }
}
