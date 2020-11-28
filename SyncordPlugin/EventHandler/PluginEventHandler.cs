using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using Newtonsoft.Json;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        private SyncordBehaviour syncordBehav;
        public PluginEventHandler()
        {
            syncordBehav = new SyncordBehaviour();
        }
        internal void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
        {
            try
            {
                var dEmbed = ev.ToDiscordEmbed();
                var dEmbedJson = JsonConvert.SerializeObject(dEmbed);
                var status = syncordBehav.SendData(dEmbedJson);

                if (SyncordPlugin.Config.LogStatus)
                    SynapseController.Server.Logger.Info(status.ToString());
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