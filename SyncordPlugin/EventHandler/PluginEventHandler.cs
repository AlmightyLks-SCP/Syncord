using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        private SyncordBehaviour syncordBehav;
        public PluginEventHandler()
        {
            syncordBehav = new SyncordBehaviour();

            Synapse.Api.Events.EventHandler.Get.Player.PlayerDeathEvent += OnPlayerDeathEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerJoinEvent += OnPlayerJoinEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerLeaveEvent += OnPlayerLeaveEvent;
            Synapse.Api.Events.EventHandler.Get.Round.SpawnPlayersEvent += OnSpawnPlayersEvent;
            Synapse.Api.Events.EventHandler.Get.Server.ConsoleCommandEvent += Server_ConsoleCommandEvent;
            Synapse.Api.Events.EventHandler.Get.Server.RemoteAdminCommandEvent += Server_RemoteAdminCommandEvent; ;
        }

        private void Server_RemoteAdminCommandEvent(RemoteAdminCommandEventArgs ev)
            => MakeAndSendEmbed(ev);

        private void Server_ConsoleCommandEvent(ConsoleCommandEventArgs ev)
            => MakeAndSendEmbed(ev);

        private void OnPlayerLeaveEvent(PlayerLeaveEventArgs ev)
            => MakeAndSendEmbed(ev);

        private void OnSpawnPlayersEvent(SpawnPlayersEventArgs ev)
            => MakeAndSendEmbed(ev);
        internal void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
            => MakeAndSendEmbed(ev);
        internal void OnPlayerJoinEvent(PlayerJoinEventArgs ev)
            => MakeAndSendEmbed(ev);
        private void MakeAndSendEmbed(Synapse.Api.Events.EventHandler.ISynapseEventArgs ev)
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
        internal async void ListenForHeartbeats()
            => await Task.Run(() => _ = syncordBehav.ListenForHeartbeats());
    }
}