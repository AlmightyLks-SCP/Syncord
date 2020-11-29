using Synapse;
using SyncordInfo;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SyncordPlugin.Syncord
{
    internal class SyncordBehaviour
    {
        private TcpClient client;
        private BinaryFormatter formatter;
        internal SyncordBehaviour()
        {
            formatter = new BinaryFormatter();
            client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Server.Get.Port));
        }

        internal async Task ListenForHeartbeats()
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

                    SynapseController.Server.Logger.Error($"Waiting for heartbeat...");
                    var info = formatter.Deserialize(client.GetStream()) as SharedInfo;

                    if (info is null)
                        continue;

                    switch (info.Content)
                    {
                        case "heartbeat":
                            SynapseController.Server.Logger.Info($"Received hearbeat");
                            var status = SendData(info.Content);
                            SynapseController.Server.Logger.Info($"Sent hearbeat: {status}");
                            break;
                    }
                }
                catch (IOException)
                {
                    int port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                    Console.WriteLine($"Socket connection for {port} was closed unexpectedly.");
                    client.Close();
                    break;
                }
                catch (SocketException e)
                {
                    int port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                    SynapseController.Server.Logger.Error($"Socket connection for {port} was closed unexpectedly.\n{e}");
                    client.Close();
                    break;
                }
                catch (SerializationException e)
                {
                    int port = ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                    SynapseController.Server.Logger.Error($"Could not serialize received data from {port}:\n{e}");
                    client.Close();
                    break;
                }
                catch (Exception e)
                {
                    SynapseController.Server.Logger.Error($"Exception in listen:\n{e}");
                }
            }
        }
        internal SendStatus SendData(string data, int attempt = 0)
        {
            if(attempt >= 3)
            {
                SynapseController.Server.Logger.Error($"Attempted reconnecting {attempt} times. Aborting.");
                return SendStatus.Unsuccessful;
            }

            try
            {
                if (!client.Connected)
                    client.Connect(IPAddress.Loopback, SyncordPlugin.Config.DiscordBotPort);

                formatter.Serialize(client.GetStream(), new SharedInfo { Content = data });
            }
            catch (IOException e)
            {
                Console.WriteLine($"Socket connection was closed unexpectedly.");
                return SendStatus.Unsuccessful;
            }
            catch (SocketException e)
            {
                SynapseController.Server.Logger.Error($"Attempted reconnecting on an already existing socket connection. Attempting reconnection...");
                client.Close();
                client = new TcpClient(new IPEndPoint(IPAddress.Loopback, Server.Get.Port));
                SendData(data, ++attempt);
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