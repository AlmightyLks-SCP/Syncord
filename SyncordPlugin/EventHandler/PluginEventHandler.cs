using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using System;
using MEC;
using SyncordInfo.EventArgs;
using EasyCommunication.SharedTypes;
using Synapse.Api;
using SyncordInfo.SimplifiedTypes;
using SyncordPlugin.Model;
using SyncordInfo.ServerStats;
using System.Collections.Generic;
using SyncordPlugin.Helper;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        public CommunicationHandler CommunicationHandler { get; set; }
        private ServerStats _serverStats;
        private int _perRoundPlayerDeathCount;
        private short _fps;

        internal PluginEventHandler()
        {
            _serverStats = new ServerStats();
            CommunicationHandler = new CommunicationHandler(_serverStats);
            _perRoundPlayerDeathCount = 0;
            _fps = 0;

            Timing.RunCoroutine(StoreFps());

            Synapse.Api.Events.EventHandler.Get.Player.PlayerJoinEvent += OnPlayerJoinEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerLeaveEvent += OnPlayerLeaveEvent;
            Synapse.Api.Events.EventHandler.Get.Round.RoundEndEvent += OnRoundEndEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerDeathEvent += OnPlayerDeathEvent;
            Synapse.Api.Events.EventHandler.Get.Round.WaitingForPlayersEvent += OnWaitingForPlayersEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerBanEvent += OnPlayerBanEvent;
            Synapse.Api.Events.EventHandler.Get.Server.UpdateEvent += Server_UpdateEvent;
        }

        private void Server_UpdateEvent()
        {
            _fps = (short)(1.0f / UnityEngine.Time.smoothDeltaTime);
        }
        private IEnumerator<float> StoreFps()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(.25f);
                if (CommunicationHandler.EasyClient.ClientConnected)
                    _serverStats.ServerFpsStats.Push(new FpsStat() { IsIdle = IdleMode.IdleModeActive, Fps = _fps, DateTime = DateTime.Now });
            }
        }

        private void OnWaitingForPlayersEvent()
            => _perRoundPlayerDeathCount = 0;
        private void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
        {
            var dmgType = ev.HitInfo.GetDamageType();
            if (dmgType.name.ToLower() == "wall" || dmgType.name.ToLower() == "none")
                return;
            if (ev.Victim == null || ev.Killer == null)
                return;
            try
            {
                DeathStat stat = new DeathStat()
                {
                    DateTime = DateTime.Now,
                    Killer = ev.Killer.Parse(),
                    Victim = ev.Victim.Parse()
                };
                _serverStats.DeathStats.Push(stat);
            }
            catch (Exception e)
            {
                Logger.Get.Error($"Failed to Parse KillStat\n{e}");
            }
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
            try
            {
                if (!CommunicationHandler.EasyClient.ClientConnected)
                    return;

                //Parse Player Join Event Args
                if (ev is PlayerJoinEventArgs join)
                {
                    if (join.Player.Hub.isLocalPlayer)
                        return;
                    if (join.TryParse(out PlayerJoinLeave joinedArgs))
                    {
                        var status = CommunicationHandler.EasyClient.QueueData(joinedArgs, DataType.ProtoBuf);
                        if (SyncordPlugin.Config.DebugMode && status != QueueStatus.Queued)
                            Logger.Get.Warn($"PlayerJoinEventArgs QueueStatus: {status}");
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
                        var status = CommunicationHandler.EasyClient.QueueData(leftArgs, DataType.ProtoBuf);
                        if (SyncordPlugin.Config.DebugMode && status != QueueStatus.Queued)
                            Logger.Get.Warn($"PlayerLeaveEventArgs QueueStatus: {status}");
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
                        var status = CommunicationHandler.EasyClient.QueueData(roundEnd, DataType.ProtoBuf);
                        if (SyncordPlugin.Config.DebugMode && status != QueueStatus.Queued)
                            Logger.Get.Warn($"RoundEnd QueueStatus: {status}");
                    }
                }
                //Parse Player Death Event Args
                else if (ev is PlayerDeathEventArgs death)
                {
                    if (death.Killer == null || death.Victim == null || death.Killer.Hub.isLocalPlayer || death.Victim.Hub.isLocalPlayer)
                        return;

                    if (death.TryParse(out PlayerDeath deathArgs))
                    {
                        var status = CommunicationHandler.EasyClient.QueueData(deathArgs, DataType.ProtoBuf);
                        if (SyncordPlugin.Config.DebugMode && status != QueueStatus.Queued)
                            Logger.Get.Warn($"PlayerDeathEventArgs QueueStatus: {status}");
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
                        var status = CommunicationHandler.EasyClient.QueueData(banArgs, DataType.ProtoBuf);
                        if (SyncordPlugin.Config.DebugMode && status != QueueStatus.Queued)
                            Logger.Get.Warn($"PlayerBanEventArgs QueueStatus: {status}");
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