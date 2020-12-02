using Synapse;
using SyncordInfo;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

namespace SyncordPlugin.Syncord
{
    internal class SyncordBehaviour
    {
        private static SyncordBehaviour singleton;
        internal bool ClientConnected => client is null ? false : client.Connected;
        internal Task Heartbeating { get; private set; }
        private TcpClient client;
        private BinaryFormatter formatter;
        internal SyncordBehaviour()
        {
            formatter = new BinaryFormatter();
            client = new TcpClient();
            singleton = this;
        }

        private void StartHeartbeating()
            => Task.Run(() => Heartbeating = ListenForRequests());
        internal async Task ListenForRequests()
        {
            for (; ; )
            {
                try
                {
                    if (!client.Connected)
                    {
                        //SynapseController.Server.Logger.Info("Can't listen for Heartbeat with non-connected client.");
                        continue;
                    }

                    SynapseController.Server.Logger.Error($"Waiting for Requests...");

                    //Wait for and deserialize the incoming data
                    var info = formatter.Deserialize(client.GetStream()) as SharedInfo;

                    if (info is null)
                        continue;

                    switch (info.RequestType)
                    {
                        case RequestType.Heartbeat:
                            {
                                //if it's a heartbeat, return the heartbeat to the Discord Bot
                                SynapseController.Server.Logger.Info($"Received heartbeat");
                                var status = SendData(info.Content, RequestType.Heartbeat);
                                SynapseController.Server.Logger.Info($"Sent heartbeat: {status}");
                                break;
                            }
                        case RequestType.Query:
                            switch (info.Content)
                            {
                                case "Player Count":
                                    SynapseController.Server.Logger.Info($"Bot Queried Player Count");
                                    var status = SendData($"{Server.Get.Players.Count} / {CustomNetworkManager.slots}", RequestType.Response);
                                    SynapseController.Server.Logger.Info($"Send Player Count: {status}");
                                    break;
                            }
                            break;
                    }
                }
                catch (IOException)
                {
                    int port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                    SynapseController.Server.Logger.Error($"Socket connection for {port} was closed unexpectedly.");
                    client.GetStream().Close();
                    client.Close();
                }
                catch (SocketException e)
                {
                    int port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                    SynapseController.Server.Logger.Error($"Socket connection for {port} was closed unexpectedly.\n{e}");
                    client.GetStream().Close();
                    client.Close();
                }
                catch (SerializationException e)
                {
                    int port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                    SynapseController.Server.Logger.Error($"Could not serialize received data from {port}:\n{e}");
                    client.GetStream().Close();
                    client.Close();
                }
                catch (Exception e)
                {
                    SynapseController.Server.Logger.Error($"eee - Exception in listen:\n{e}");
                }
            }
        }

        internal static SendStatus ConnectClient()
        {
            try
            {
                //If the client, for whatever reason, disconnected - Reconnect.
                if (!singleton.client.Connected)
                {
                    //Stop heartingbeating for old client
                    singleton.Heartbeating?.Dispose();

                    //Reset client
                    singleton.client = new TcpClient();

                    SynapseController.Server.Logger.Warn($"Not connected, reconnecting.");

                    singleton.client.Connect(IPAddress.Loopback, SyncordPlugin.Config.DiscordBotPort);

                    singleton.StartHeartbeating();

                    singleton.SendData("Connect", RequestType.Connect);
                }
            }
            catch (IOException)
            {
                SynapseController.Server.Logger.Error($"Socket connection was closed unexpectedly.");
                return SendStatus.Unsuccessful;
            }
            catch (SocketException)
            {
                SynapseController.Server.Logger.Error($"Target machine was not listening on port {SyncordPlugin.Config.DiscordBotPort}...");

                //Disconnect client
                if (singleton.client.Client.Connected)
                    singleton.client.Client.Disconnect(false);
                return SendStatus.Error;
            }
            catch (Exception e)
            {
                SynapseController.Server.Logger.Error($"Exception in SendData:\n{e}");
                return SendStatus.Error;
            }

            return SendStatus.Successful;
        }
        internal SendStatus SendData(string data, RequestType reqType = RequestType.Event)
        {
            try
            {
                //If the client, for whatever reason, disconnected - Attempt reconnecting.
                if (!ClientConnected)
                    ConnectClient();

                //Prepare data to send
                var info = new SharedInfo() { Port = Server.Get.Port, RequestType = reqType, Content = data };

                //Serialize Data into stream.
                formatter.Serialize(client.GetStream(), info);
            }
            catch (Exception e)
            {
                SynapseController.Server.Logger.Error($"Exception in SendData:\n{e}");
                return SendStatus.Error;
            }

            return SendStatus.Successful;
        }
    }
}