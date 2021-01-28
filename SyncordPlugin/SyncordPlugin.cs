using Synapse.Api;
using Synapse.Api.Plugin;
using SyncordPlugin.Config;
using SyncordPlugin.EventHandler;
using SyncordPlugin.Syncord;
using System.Reflection;
using System.Threading.Tasks;

namespace SyncordPlugin
{
    [PluginInformation(
        Author = "AlmightyLks",
        Description = "A way of connecting Discord with you SCP SL Server",
        Name = "SyncordPlugin",
        SynapseMajor = 2,
        SynapseMinor = 2,
        SynapsePatch = 0,
        Version = "0.9.0"
        )]
    public class SyncordPlugin : AbstractPlugin
    {
        [Synapse.Api.Plugin.Config(section = "Syncord")]
        public static SyncordConfig Config { get; set; }

        private PluginEventHandler handler;

        public override void Load()
        {
            handler = new PluginEventHandler();
        }
    }
}
