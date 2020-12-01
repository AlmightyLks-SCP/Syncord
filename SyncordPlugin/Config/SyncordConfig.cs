using Synapse.Config;
using System.ComponentModel;

namespace SyncordPlugin.Config
{
    public class SyncordConfig : AbstractConfigSection
    {
        [Description("Port which the Discord-Bot is listening to")]
        public int DiscordBotPort { get; set; } = 8000;

    }
}