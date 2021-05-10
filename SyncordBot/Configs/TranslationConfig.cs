using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SyncordBot.Configs
{
    public class TranslationConfig
    {
        public Models.Translation Translation { get; init; }
        public TranslationConfig()
        {
            Translation = new Models.Translation
            {
                Language = "English",
                Elements = new Dictionary<string, string>()
                {
                    ["Player Join"] = "Player Join",
                    ["Player Leave"] = "Player Leave",
                    ["Player Ban"] = "Player Ban",
                    ["ms"] = "ms",
                    ["Weapon"] = "Weapon",
                    ["Server"] = "Server",
                    ["Banned"] = "Banned",
                    ["Reason"] = "Reason",
                    ["Duration"] = "Duration",
                    ["Minutes"] = "Minute(s)",
                    ["Hours"] = "Hour(s)",
                    ["Days"] = "Day(s)",
                    ["Years"] = "Year(s)",
                    ["Player Death"] = "Player Death",
                    ["Killer"] = "Killer",
                    ["Victim"] = "Victim",
                    ["Was not Cuffed"] = "Was not Cuffed",
                    ["Was Cuffed"] = "Was Cuffed",
                    ["Round Summary"] = "Round Summary",
                    ["Round Length"] = "Round Length",
                    ["Total Kills"] = "Total Kills",
                    ["Kills By SCPs"] = "Kills By SCPs",
                    ["Kills By Frag Grenades"] = "Kills By Frag Grenades",
                    ["Escaped D-Class"] = "Escaped D-Class",
                    ["Escaped Scientists"] = "Escaped Scientists"
                }
            };
        }

        public static TranslationConfig Load()
        {
            TranslationConfig result = new TranslationConfig();
            string configDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Configs");
            string configPath = Path.Combine(configDirectory, "Translation-Config.json");

            if (!Directory.Exists(configDirectory))
                Directory.CreateDirectory(configDirectory);

            if (!File.Exists(configPath))
                File.WriteAllText(configPath, JsonConvert.SerializeObject(result, Formatting.Indented));
            else
                result = JsonConvert.DeserializeObject<TranslationConfig>(File.ReadAllText(configPath));

            return result;
        }
    }
}
