using Synapse;
using SyncordInfo;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SyncordPlugin.Syncord
{
    internal class SyncordBehaviour
    {
        private BinaryFormatter formatter;
        internal SyncordBehaviour()
        {
            formatter = new BinaryFormatter();
        }
        internal SendStatus SendData(string data)
        {
            SynapseController.Server.Logger.Info("In SendData");
            SynapseController.Server.Logger.Info($"{SyncordPlugin.Config.DiscordBotPort}");
            SynapseController.Server.Logger.Info($"{Server.Get.Port}");
            try
            {
                IPEndPoint ipLocalEndPoint = new IPEndPoint(IPAddress.Loopback, Server.Get.Port);

                using (TcpClient client = new TcpClient(ipLocalEndPoint))
                {
                    client.Connect(IPAddress.Loopback, SyncordPlugin.Config.DiscordBotPort);
                    formatter.Serialize(client.GetStream(), new SharedInfo { Content = data });
                    client.Close();
                }
            }
            catch (Exception e)
            {
                SynapseController.Server.Logger.Error(e.ToString());
                return SendStatus.Error;
            }
            return SendStatus.Successful;
        }
    }
}