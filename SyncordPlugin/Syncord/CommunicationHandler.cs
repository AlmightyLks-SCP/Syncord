using System;
using System.Collections.Generic;
using System.Net;
using EasyCommunication.Connection;
using EasyCommunication.Events.Client.EventArgs;
using EasyCommunication.Helper;
using EasyCommunication.SharedTypes;
using SyncordInfo.Communication;
using MEC;
using Synapse.Api;
using Synapse;
using SyncordInfo.ServerStats;
using Newtonsoft.Json;
using SyncordPlugin.Model;
using SyncordPlugin.Helper;
using System.Linq;
using System.Net.Sockets;

namespace SyncordPlugin.Syncord
{
    public class CommunicationHandler
    {
        public EasyClient EasyClient { get; set; }
        private ServerStats _serverStats;
        public CommunicationHandler(ServerStats _serverStats)
        {
            this._serverStats = _serverStats;
            EasyClient = new EasyClient(2500)
            {
                BufferSize = 16384
            };
            Timing.RunCoroutine(AutoReconnect());
            EasyClient.EventHandler.ConnectedToHost += OnConnectedToHost;
            EasyClient.EventHandler.DisconnectedFromHost += OnDisconnectedFromHost;
            EasyClient.EventHandler.ReceivedData += OnReceivedData;
        }

        private void OnReceivedData(ReceivedDataEventArgs ev)
        {
            switch (ev.Type)
            {
                case DataType.ProtoBuf:
                    {
                        if (!ev.Data.TryDeserializeProtoBuf(out DataBase DataBase))
                            return;

                        if (ev.Data.TryDeserializeProtoBuf(out Ping ping) && ping != null)
                        {
                            Logger.Get.Info($"Ping latency: {(ping.Received - ping.Sent).Milliseconds} ms");
                        }
                        else if (ev.Data.TryDeserializeProtoBuf(out Query query))
                        {
                            switch (query.QueryType)
                            {
                                case QueryType.PlayerCount:
                                    {
                                        PlayerCountStat stat = new PlayerCountStat()
                                        {
                                            DateTime = DateTime.Now,
                                            MaxPlayers = Server.Get.Slots,
                                            PlayerCount = Server.Get.Players.Count
                                        };
                                        Response response = new Response()
                                        {
                                            SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                                            SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                                            Time = DateTime.Now,
                                            Query = query,
                                            Content = JsonConvert.SerializeObject(stat)
                                        };
                                        EasyClient.QueueData(response, DataType.ProtoBuf);
                                        break;
                                    }
                                case QueryType.PlayerDeaths:
                                    {
                                        Response response = new Response()
                                        {
                                            SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                                            SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                                            Time = DateTime.Now,
                                            Query = query,
                                            Content = JsonConvert.SerializeObject(_serverStats.DeathStats)
                                        };
                                        _serverStats.DeathStats.Clear();
                                        EasyClient.QueueData(response, DataType.ProtoBuf);
                                        break;
                                    }
                                case QueryType.ServerFps:
                                    {
                                        List<FpsStat> averageFpsStats;
                                        if (_serverStats.ServerFpsStats.Count == 0)
                                            averageFpsStats = new List<FpsStat>() { new FpsStat() { DateTime = DateTime.Now, Fps = -1, IsIdle = true } };
                                        else
                                            averageFpsStats = _serverStats.ServerFpsStats.AverageFpsPerSecond().ToList();
                                        Logger.Get.Warn($"Received and sending fps info: {averageFpsStats.Count}");
                                        Response response = new Response()
                                        {
                                            SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                                            SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                                            Time = DateTime.Now,
                                            Query = query,
                                            Content = JsonConvert.SerializeObject(averageFpsStats)
                                        };
                                        _serverStats.ServerFpsStats.Clear();
                                        EasyClient.QueueData(response, DataType.ProtoBuf);
                                        break;
                                    }
                            }
                        }
                        break;
                    }
            }
        }

        private IEnumerator<float> AutoReconnect()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(5f);
                try
                {
                    if (EasyClient.ClientConnected || !SyncordPlugin.Config.AutoConnect)
                        continue;
                    if (IPAddress.TryParse(SyncordPlugin.Config.DiscordBotAddress, out IPAddress botAddress))
                        EasyClient.ConnectToHost(botAddress, SyncordPlugin.Config.DiscordBotPort);
                    if (SyncordPlugin.Config.DebugMode && EasyClient.ClientConnected)
                        Logger.Get.Info($"Reconnected");
                }
                catch (Exception e)
                {
                    Logger.Get.Info($"Error in AutoReconnect:\n{e}");
                }
            }
        }

        private void OnDisconnectedFromHost(DisconnectedFromHostEventArgs ev)
        {
            if (SyncordPlugin.Config.DebugMode)
                Logger.Get.Warn($"Lost connection to host.");
        }
        private void OnConnectedToHost(ConnectedToHostEventArgs ev)
        {
            if (SyncordPlugin.Config.DebugMode)
                Logger.Get.Warn("Connected to host");
        }
    }
}
