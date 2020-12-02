using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using Newtonsoft.Json;
using System.Threading.Tasks;
using static Synapse.Api.Events.EventHandler;
using System;
using System.Collections.Generic;
using MEC;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        internal SyncordBehaviour SyncordBehaviour { get; set; }
        private SpamQueue SpamQueue;
        internal PluginEventHandler()
        {
            SyncordBehaviour = new SyncordBehaviour();
            SpamQueue = new SpamQueue();

            Get.Player.PlayerDeathEvent += OnPlayerDeathEvent;
            Get.Player.PlayerJoinEvent += OnPlayerJoinEvent;
            Get.Player.PlayerLeaveEvent += OnPlayerLeaveEvent;
            Get.Round.SpawnPlayersEvent += OnSpawnPlayersEvent;
            Get.Server.ConsoleCommandEvent += OnConsoleCommandEvent;
            Get.Server.RemoteAdminCommandEvent += OnRemoteAdminCommandEvent;
            Get.Player.PlayerBanEvent += OnPlayerBanEvent;
        }

        private void OnPlayerBanEvent(PlayerBanEventArgs ev)
            => MakeAndSendEmbed(ev);
        private void OnRemoteAdminCommandEvent(RemoteAdminCommandEventArgs ev)
            => MakeAndSendEmbed(ev);
        private void OnConsoleCommandEvent(ConsoleCommandEventArgs ev)
            => MakeAndSendEmbed(ev);
        private void OnPlayerLeaveEvent(PlayerLeaveEventArgs ev)
            => MakeAndSendEmbed(ev);
        private void OnSpawnPlayersEvent(SpawnPlayersEventArgs ev)
            => Timing.CallDelayed(1f, () => MakeAndSendEmbed(ev));
        internal void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
            => MakeAndSendEmbed(ev);
        internal void OnPlayerJoinEvent(PlayerJoinEventArgs ev)
            => MakeAndSendEmbed(ev);

        private async void MakeAndSendEmbed(ISynapseEventArgs ev)
        {
            try
            {
                if (!SyncordBehaviour.ClientConnected)
                    return;

                //if (ev is PlayerLeaveEventArgs playerLeave)
                //{
                //    SpamQueue.AddToQueue(playerLeave);
                //}
                //else if (ev is PlayerJoinEventArgs playerJoin)
                //{
                //    SpamQueue.AddToQueue(playerJoin);
                //}

                //Create Embed
                var dEmbed = ev.ToDiscordEmbedBuilder();

                if (dEmbed is null)
                    return;

                //Serialize to Json
                var dEmbedJson = JsonConvert.SerializeObject(dEmbed.Build());

                //Send to Discord Bot if client is not off
                var status = SyncordBehaviour.SendData(dEmbedJson);

                SynapseController.Server.Logger.Info(status.ToString());
            }
            catch (InvalidOperationException)
            {
                SynapseController.Server.Logger.Error($"Stream to write into was aborted");
            }
            catch (Exception e)
            {
                SynapseController.Server.Logger.Error($"Exception in MakeAndSendEmbed:\n{e}");
            }
        }
    }
}