using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace SyncordBot.Configs
{
    public sealed class AliasConfig
    {
        public Dictionary<string, string> Aliases { get; set; }
        public AliasConfig()
        {
            Aliases = new Dictionary<string, string>();
        }
        public static AliasConfig Load()
        {
            AliasConfig result = new AliasConfig();
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Configs");
            string configPath = Path.Combine(configDirectory, "Alias-Config.json");

            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);

            if (!File.Exists(configPath))
                File.WriteAllText(configPath, JsonConvert.SerializeObject(result));
            else
                result = JsonConvert.DeserializeObject<AliasConfig>(File.ReadAllText(configPath));

            return result;
        }
    }
}
