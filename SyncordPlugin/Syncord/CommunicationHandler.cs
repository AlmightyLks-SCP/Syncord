using System;
using SyncordInfo.Communication;
using Synapse.Api;
using Synapse;
using SyncordInfo.ServerStats;
using SimpleTcp;
using System.Text;
using SyncordInfo.Helper;
using Newtonsoft.Json;
using SyncordPlugin.EventHandler;
using System.ComponentModel;
using System.Threading.Tasks;

namespace SyncordPlugin.Syncord
{
    internal class CommunicationHandler
    {
        internal SimpleTcpClient TcpClient { get; set; }

        private PluginEventHandler _pluginEventHandler;
        private BackgroundWorker _reconnectWorker;

        internal CommunicationHandler(string ip, int port, PluginEventHandler pluginEventHandler)
        {
            TcpClient = new SimpleTcpClient(ip, port);
            TcpClient.Keepalive.TcpKeepAliveRetryCount = int.MaxValue;
            TcpClient.Keepalive.EnableTcpKeepAlives = true;
            _pluginEventHandler = pluginEventHandler;

            TcpClient.Events.Connected += OnConnectedToHost;
            TcpClient.Events.Disconnected += OnDisconnectedFromHost;
            TcpClient.Events.DataReceived += OnReceivedData;
            TcpClient.Connect();

            if (SyncordPlugin.Config.AutoReconnect)
            {
                _reconnectWorker = new BackgroundWorker();
                _reconnectWorker.DoWork += OnDoWorkReconnectWorker;
                _reconnectWorker.RunWorkerCompleted += OnReconnectWorkerCompleted;
                _reconnectWorker.RunWorkerAsync();
            }
        }

        private async void OnReconnectWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            await Task.Delay(2500);
            _reconnectWorker.RunWorkerAsync();
        }
        private void OnDoWorkReconnectWorker(object sender, DoWorkEventArgs e)
        {
            if (TcpClient.IsConnected)
            {
                return;
            }

            try
            {
                TcpClient.Connect();
            }
            catch
            {
                if (SyncordPlugin.Config.DebugMode)
                    Synapse.Api.Logger.Get.Error("Couldn't reconnect");
            }
        }
        private void OnReceivedData(object sender, SimpleTcp.DataReceivedEventArgs ev)
        {
            string receivedJsonString = Encoding.UTF8.GetString(ev.Data);

            if (!receivedJsonString.TryDeserializeJson(out DataBase dataBase))
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
                                QueryType = query.QueryType,
                                JsonContent = JsonConvert.SerializeObject(stat)
                            };
                            TcpClient.SendAsJson(response);
                            break;
                        }
                    case QueryType.ServerFps:
                        {
                            FpsStat fpsStat = new FpsStat()
                            {
                                DateTime = DateTime.Now,
                                Fps = _pluginEventHandler.ServerFps,
                                IsIdle = IdleMode.IdleModeActive
                            };
                            Response response = new Response()
                            {
                                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                                Time = DateTime.Now,
                                QueryType = query.QueryType,
                                JsonContent = JsonConvert.SerializeObject(fpsStat)
                            };
                            TcpClient.SendAsJson(response);
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
