using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using System;
using MEC;
using SyncordInfo.EventArgs;
using Synapse.Api;
using SyncordInfo.SimplifiedTypes;
using SyncordInfo.Helper;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        internal CommunicationHandler CommunicationHandler { get; set; }

        public float ServerFps { get; private set; }
        private ushort _perRoundPlayerDeathCount;

        internal PluginEventHandler(string ipPort)
        {
            CommunicationHandler = new CommunicationHandler(ipPort, this);

            Synapse.Api.Events.EventHandler.Get.Server.UpdateEvent += OnServerUpdateEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerJoinEvent += OnPlayerJoinEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerLeaveEvent += OnPlayerLeaveEvent;
            Synapse.Api.Events.EventHandler.Get.Round.RoundEndEvent += OnRoundEndEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerDeathEvent += OnPlayerDeathEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerBanEvent += OnPlayerBanEvent;
        }

        private void OnServerUpdateEvent()
        {
            ServerFps = 1.0f / UnityEngine.Time.smoothDeltaTime;
        }
        private void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
        {
            var dmgType = ev.HitInfo.GetDamageType();
            if (dmgType.name.ToLower() == "wall" || dmgType.name.ToLower() == "none")
                return;
            if (ev.Victim == null || ev.Killer == null)
                return;

            _perRoundPlayerDeathCount++;

            MakeAndSendData(ev);
        }
        private void OnRoundEndEvent()
            => MakeAndSendData(ParseHelper.GetSimpleRoundSummary(_perRoundPlayerDeathCount));
        private void OnPlayerLeaveEvent(PlayerLeaveEventArgs ev)
            => MakeAndSendData(ev);
        private void OnPlayerJoinEvent(PlayerJoinEventArgs ev)
            => Timing.CallDelayed(0.75f, () => MakeAndSendData(ev));
        private void OnPlayerBanEvent(PlayerBanEventArgs ev)
            => MakeAndSendData(ev);

        private void MakeAndSendData(object ev)
        {
            Synapse.Api.Logger.Get.Info("1");
            try
            {
                Synapse.Api.Logger.Get.Info("2");
                if (!CommunicationHandler.TcpClient.IsConnected)
                    return;
                Synapse.Api.Logger.Get.Info("3");

                //Parse Player Join Event Args
                if (ev is PlayerJoinEventArgs join)
                {
                    Synapse.Api.Logger.Get.Info("4");
                    if (join.Player.Hub.isLocalPlayer)
                        return;
                    Synapse.Api.Logger.Get.Info("5");
                    if (join.TryParse(out PlayerJoinLeave joinedArgs))
                    {
                        Synapse.Api.Logger.Get.Info("6");
                        CommunicationHandler.TcpClient.SendAsJson(joinedArgs);
                    }
                    else if (SyncordPlugin.Config.DebugMode)
                    {
                        Logger.Get.Error($"Couldn't parse join information for PlayerJoinEventArgs");
                    }
                }
                //Parse Player Leave Event Args
                else if (ev is PlayerLeaveEventArgs leave)
                {
                    if (leave.Player.Hub.isLocalPlayer)
                        return;
                    if (leave.TryParse(out PlayerJoinLeave leftArgs))
                    {
                        //SL / Synapse Bug, name sometimes appears as empty
                        if (string.IsNullOrWhiteSpace(leftArgs.Player.Nickname))
                        {
                            //Try again after a second
                            Timing.CallDelayed(1f, () =>
                            {
                                //Return if parsing failed this time / Nickname is still empty
                                if (!leave.TryParse(out PlayerJoinLeave reattemptleftArgs) || string.IsNullOrWhiteSpace(leftArgs.Player.Nickname))
                                    return;
                            });
                        }
                        CommunicationHandler.TcpClient.SendAsJson(leftArgs);
                    }
                    else if (SyncordPlugin.Config.DebugMode)
                    {
                        Logger.Get.Error($"Couldn't parse join information for PlayerLeaveEventArgs");
                    }
                }
                //Parse SimpleRoundSummary
                else if (ev is SimpleRoundSummary simpleRoundSummary)
                {
                    if (simpleRoundSummary.TryParse(out RoundEnd roundEnd))
                    {
                        CommunicationHandler.TcpClient.SendAsJson(roundEnd);
                    }
                }
                //Parse Player Death Event Args
                else if (ev is PlayerDeathEventArgs death)
                {
                    if (death.Killer == null || death.Victim == null || death.Killer.Hub.isLocalPlayer || death.Victim.Hub.isLocalPlayer)
                        return;

                    if (death.TryParse(out PlayerDeath deathArgs))
                    {
                        CommunicationHandler.TcpClient.SendAsJson(deathArgs);
                    }
                    else if (SyncordPlugin.Config.DebugMode)
                    {
                        Logger.Get.Error($"Couldn't parse join information for PlayerDeathEventArgs");
                    }
                }
                //Parse Player Ban Event Args
                else if (ev is PlayerBanEventArgs ban)
                {
                    if (ban.TryParse(out PlayerBan banArgs))
                    {
                        CommunicationHandler.TcpClient.SendAsJson(banArgs);
                    }
                    else if (SyncordPlugin.Config.DebugMode)
                    {
                        Logger.Get.Error($"Couldn't parse join information for PlayerBanEventArgs");
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