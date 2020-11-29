using Synapse.Api.Plugin;
using SyncordPlugin.Config;
using SyncordPlugin.EventHandler;

namespace SyncordPlugin
{
    [PluginInformation(
        Author = "AlmightyLks",
        Description = "Hehe",
        Name = "Syncord ",
        SynapseMajor = 2,
        SynapseMinor = 2,
        SynapsePatch = 0,
        Version = "1.0.0"
        )]
    public class SyncordPlugin : AbstractPlugin
    {
        private PluginEventHandler handler;

        [Synapse.Api.Plugin.Config(section = "Syncord")]
        public static SyncordConfig Config { get; set; }

        public override void Load()
        {
            handler = new PluginEventHandler();
            handler.ListenForHeartbeats();

            Synapse.Api.Events.EventHandler.Get.Player.PlayerDeathEvent += handler.OnPlayerDeathEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerJoinEvent += handler.OnPlayerJoinEvent;
        }
    }
}
