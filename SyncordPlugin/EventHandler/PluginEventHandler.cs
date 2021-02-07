﻿/*
    The following license applies to the entirety of this Repository and Solution.
    
    
    
    Copyright 2021 AlmightyLks

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the License for the specific language governing
    permissions and limitations under the License.
*/
using Synapse.Api.Events.SynapseEventArguments;
using SyncordPlugin.Syncord;
using Newtonsoft.Json;
using System;
using MEC;
using SyncordInfo.EventArgs;
using EasyCommunication.SharedTypes;
using Synapse.Api;
using System.Threading.Tasks;
using SyncordInfo.SimplifiedTypes;
using Synapse;

namespace SyncordPlugin.EventHandler
{
    internal class PluginEventHandler
    {
        public CommunicationHandler CommunicationHandler { get; set; }
        private int _playerDeathCount;

        internal PluginEventHandler()
        {
            CommunicationHandler = new CommunicationHandler();
            _playerDeathCount = 0;

            Synapse.Api.Events.EventHandler.Get.Player.PlayerJoinEvent += OnPlayerJoinEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerLeaveEvent += OnPlayerLeaveEvent;
            Synapse.Api.Events.EventHandler.Get.Round.RoundEndEvent += OnRoundEndEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerDeathEvent += OnPlayerDeathEvent;
            Synapse.Api.Events.EventHandler.Get.Round.WaitingForPlayersEvent += OnWaitingForPlayersEvent;
            Synapse.Api.Events.EventHandler.Get.Player.PlayerBanEvent += OnPlayerBanEvent;
        }

        private void OnWaitingForPlayersEvent()
            => _playerDeathCount = 0;
        private void OnPlayerDeathEvent(PlayerDeathEventArgs ev)
        {
            var dmgType = ev.HitInfo.GetDamageType();
            if (dmgType.name.ToLower() == "wall" || dmgType.name.ToLower() == "none")
                return;
            if (ev.Victim == null || ev.Killer == null)
                return;

            _playerDeathCount++;
            MakeAndSendData(ev);
        }
        private void OnRoundEndEvent()
            => MakeAndSendData(ParseHelper.GetSimpleRoundSummary(_playerDeathCount));
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
                                if(!leave.TryParse(out PlayerJoinLeave reattemptleftArgs) || string.IsNullOrWhiteSpace(leftArgs.Player.Nickname))
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