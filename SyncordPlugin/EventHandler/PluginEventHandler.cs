using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using Newtonsoft.Json;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        private SyncordBehaviour syncord;
        public PluginEventHandler()
        {
            syncord = new SyncordBehaviour();
        }
        internal void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
        {
            try
            {
                var dEmbed = ev.ToDiscordEmbed();
                var dEmbedJson = JsonConvert.SerializeObject(dEmbed);
                SynapseController.Server.Logger.Info(syncord.SendData(dEmbedJson).ToString());
            }
            catch (System.Exception e)
            {
                SynapseController.Server.Logger.Info("--------------");
                SynapseController.Server.Logger.Error(e.ToString());
                SynapseController.Server.Logger.Info("--------------");
            }
        }

        internal void OnPlayerJoinEvent(PlayerJoinEventArgs ev)
        {

        }
    }
}