﻿using Synapse.Config;
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

        [Description("Whether the server should attempt to reconnect to the syncord bot")]
        public bool AutoReconnect { get; internal set; }
    }
}