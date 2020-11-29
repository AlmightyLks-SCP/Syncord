using Synapse;
using SyncordInfo;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
                if (!client.Connected)
                {
                    //SynapseController.Server.Logger.Info("Can't listen for Heartbeat with non-connected client.");
                    continue;
                }

                SynapseController.Server.Logger.Error($"Before");
                var info = formatter.Deserialize(client.GetStream()) as SharedInfo;
                SynapseController.Server.Logger.Error($"After");

                if (info is null)
                    continue;

                SynapseController.Server.Logger.Info($"This arrived: {info.Content}");

                switch (info.Content)
                {
                    case "heartbeat":
                        SynapseController.Server.Logger.Info($"Received hearbeat");
                        var status = SendData("heartbeat");
                        SynapseController.Server.Logger.Info($"Sending hearbeat {status}");
                        break;
                }
                await Task.Delay(3500);
            }
        }
        internal SendStatus SendData(string data)
        {
            SynapseController.Server.Logger.Info("In SendData");

            try
            {
                if (!client.Connected)
                    client.Connect(IPAddress.Loopback, SyncordPlugin.Config.DiscordBotPort);

                formatter.Serialize(client.GetStream(), new SharedInfo { Content = data });
            }
            catch (Exception e)
            {
                SynapseController.Server.Logger.Error($"Could not establish connection\n{e}");
                return SendStatus.Error;
            }
            return SendStatus.Successful;
        }
    }
}