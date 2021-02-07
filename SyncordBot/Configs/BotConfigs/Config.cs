/*
    The following license applies to the entirety of this Repository and Solution.
    
    
    
    Copyright 2021 AlmightyLks

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the License for the specific language governing
    permissions and limitations under the License.
*/
using DSharpPlus.Entities;
using SyncordBot.Configs.EmbedConfigs;
using SyncordBot.Configs.Translation;
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
        public TranslationConfig TranslationConfig{ get; set; }
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
            TranslationConfig = new TranslationConfig();
        }
    }
}