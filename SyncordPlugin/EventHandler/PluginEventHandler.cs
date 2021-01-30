using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using Newtonsoft.Json;
using System;
using MEC;
using SyncordInfo.EventArgs;
using EasyCommunication.SharedTypes;
using Synapse.Api;
using System.Threading.Tasks;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        public CommunicationHandler CommunicationHandler { get; set; }

        internal PluginEventHandler()
        {
            CommunicationHandler = new CommunicationHandler();

            Synapse.Api.Events.EventHandler.Get.Player.PlayerJoinEvent += OnPlayerJoinEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerLeaveEvent += OnPlayerLeaveEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerReportEvent += OnPlayerReportEvent;
            //Synapse.Api.Events.EventHandler.Get.Player.PlayerDeathEvent += OnPlayerDeathEvent;
            //Synapse.Api.Events.EventHandler.Get.Player.PlayerBanEvent += OnPlayerBanEvent;
            //Synapse.Api.Events.EventHandler.Get.Server.ConsoleCommandEvent += OnConsoleCommandEvent;
            //Synapse.Api.Events.EventHandler.Get.Server.RemoteAdminCommandEvent += OnRemoteAdminCommandEvent;
            //Synapse.Api.Events.EventHandler.Get.Round.SpawnPlayersEvent += OnSpawnPlayersEvent;
        }

        private void OnPlayerReportEvent(PlayerReportEventArgs ev)
            => MakeAndSendData(ev);

        private void OnPlayerBanEvent(PlayerBanEventArgs ev)
            => MakeAndSendData(ev);
        private void OnRemoteAdminCommandEvent(RemoteAdminCommandEventArgs ev)
            => MakeAndSendData(ev);
        private void OnConsoleCommandEvent(ConsoleCommandEventArgs ev)
            => MakeAndSendData(ev);
        private void OnPlayerLeaveEvent(PlayerLeaveEventArgs ev)
            => MakeAndSendData(ev);
        private void OnSpawnPlayersEvent(SpawnPlayersEventArgs ev)
            => Timing.CallDelayed(1f, () => MakeAndSendData(ev));
        private void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
            => MakeAndSendData(ev);
        private void OnPlayerJoinEvent(PlayerJoinEventArgs ev)
            => Timing.CallDelayed(0.1f, () => MakeAndSendData(ev));

        private void MakeAndSendData(Synapse.Api.Events.EventHandler.ISynapseEventArgs ev)
        {
            try
            {
                if (!CommunicationHandler.EasyClient.ClientConnected)
                    return;
                //Console.WriteLine($"Hereeeeeeeeeee: \"{CustomNetworkManager.Ip}\"");
                
                //Parse Player Join Event Args
                if (ev is PlayerJoinEventArgs join)
                {
                    if (join.TryParse(out PlayerJoinLeave joinedArgs))
                    {
                        var status = CommunicationHandler.EasyClient.QueueData(joinedArgs, DataType.ProtoBuf);
                        if (SyncordPlugin.Config.DebugMode && status != QueueStatus.Queued)
                            Logger.Get.Warn($"PlayerJoinEventArgs QueueStatus: {status}");
                    }
                    else if (SyncordPlugin.Config.DebugMode)
                    {
                        Logger.Get.Error($"Couldn't parse join information for {join?.Nickname} ({join?.Player?.UserId})");
                    }
                }
                //Parse Player Leave Event Args
                else if (ev is PlayerLeaveEventArgs leave)
                {
                    if (leave.TryParse(out PlayerJoinLeave leftArgs))
                    {
                        var status = CommunicationHandler.EasyClient.QueueData(leftArgs, DataType.ProtoBuf);
                        if (SyncordPlugin.Config.DebugMode && status != QueueStatus.Queued)
                            Logger.Get.Warn($"PlayerLeaveEventArgs QueueStatus: {status}");
                    }
                    else if (SyncordPlugin.Config.DebugMode)
                    {
                        Logger.Get.Error($"Couldn't parse leave information for {leave?.Player?.NickName} ({leave?.Player?.UserId})");
                    }
                }

            }
            catch (Exception e)
            {
                if (SyncordPlugin.Config.DebugMode)
                    Logger.Get.Error($"MakeAndSendData: {e}");
            }
        }
    }
}