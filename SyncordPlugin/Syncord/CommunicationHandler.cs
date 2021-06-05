using System;
using System.Collections.Generic;
using System.Net;
using SyncordInfo.Communication;
using MEC;
using Synapse.Api;
using Synapse;
using SyncordInfo.ServerStats;
using Newtonsoft.Json;
using SimpleTcp;
using System.Linq;
using System.Text;
using SyncordInfo.Helper;

namespace SyncordPlugin.Syncord
{
    public class CommunicationHandler
    {
        public SimpleTcpClient TcpCLient { get; set; }

        public CommunicationHandler(string ip, int port)
        {
            TcpCLient = new SimpleTcpClient(ip, port);
            TcpCLient.Keepalive.TcpKeepAliveRetryCount = int.MaxValue;

            TcpCLient.Events.Connected += OnConnectedToHost;
            TcpCLient.Events.Disconnected += OnDisconnectedFromHost;
            TcpCLient.Events.DataReceived += OnReceivedData;
            TcpCLient.Connect();
        }
        public CommunicationHandler(string ipPort)
        {
            TcpCLient = new SimpleTcpClient(ipPort);

            TcpCLient.Events.Connected += OnConnectedToHost;
            TcpCLient.Events.Disconnected += OnDisconnectedFromHost;
            TcpCLient.Events.DataReceived += OnReceivedData;
        }

        private void OnReceivedData(object sender, SimpleTcp.DataReceivedEventArgs ev)
        {
            string receivedJsonString = Encoding.UTF8.GetString(ev.Data);

            if (!receivedJsonString.TryDeserializeJson(out DataBase DataBase))
                return;

            if (receivedJsonString.TryDeserializeJson(out Ping ping) && ping != null)
            {
                Logger.Get.Info($"Ping latency: {(ping.Received - ping.Sent).Milliseconds} ms");
            }
            else if (receivedJsonString.TryDeserializeJson(out Query query))
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
                                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                                Time = DateTime.Now,
                                Query = query,
                                Content = JsonConvert.SerializeObject(stat)
                            };
                            TcpCLient.SendAsJson(response);
                            break;
                        }
                }
            }
        }
        private void OnDisconnectedFromHost(object sender, SimpleTcp.ClientDisconnectedEventArgs ev)
        {
            if (SyncordPlugin.Config.DebugMode)
                Logger.Get.Warn($"Lost connection to host.");
        }
        private void OnConnectedToHost(object sender, SimpleTcp.ClientConnectedEventArgs ev)
        {
            if (SyncordPlugin.Config.DebugMode)
                Logger.Get.Warn("Connected to host");
        }
    }
}
