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
using Synapse.Config;
using System.ComponentModel;

namespace SyncordPlugin.Config
{
    public class SyncordConfig : AbstractConfigSection
    {
        [Description("Debug mode displays important info and errors in the console")]
        public bool DebugMode { get; set; } = false;

        [Description("Address which the Discord-Bot is hosted on")]
        public string DiscordBotAddress { get; set; } = "127.0.0.1";

        [Description("Port which the Discord-Bot is listening to")]
        public int DiscordBotPort { get; set; } = 8000;

        [Description("Whether the Server should try to reconnect when connection is lost")]
        public bool AutoReconnect { get; set; } = false;
    }
}