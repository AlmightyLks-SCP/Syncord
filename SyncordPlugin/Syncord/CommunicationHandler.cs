using System;
using System.Collections.Generic;
using System.Net;
using EasyCommunication;
using EasyCommunication.Events.Client.EventArgs;
using EasyCommunication.Helper;
using EasyCommunication.SharedTypes;
using SyncordInfo.Communication;
using MEC;
using Synapse.Api;
using Synapse;
using SyncordInfo.ServerStats;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Sockets;
using EasyCommunication.Connection;

namespace SyncordPlugin.Syncord
{
    public class CommunicationHandler
    {
        public EasyClient EasyClient { get; set; }

        public CommunicationHandler()
        {
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
                            Logger.Get.Warn($"Received: {query.QueryType}");
                            switch (query.QueryType)
                            {
                                case QueryType.PlayerCount:
                                    {
                                        PlayerCountStat stat = new PlayerCountStat()
                                        {
                                            DateTime = DateTime.Now,
                                            MaxPlayers = (ushort)Server.Get.Slots,
                                            PlayerCount = (ushort)Server.Get.Players.Count
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
