using SyncordInfo;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace SyncordPlugin.Syncord
{
    internal class SyncordBehaviour
    {
        private TcpClient client;
        private BinaryFormatter formatter;
        internal SyncordBehaviour()
        {
            client = new TcpClient();
            formatter = new BinaryFormatter();
        }
        internal SendStatus SendData(string data)
        {
            SynapseController.Server.Logger.Info("In SendData");
            try
            {
                client.Connect(IPAddress.Loopback, SyncordPlugin.Config.DiscordBotPort);
                formatter.Serialize(client.GetStream(), new SharedInfo { Content = data });
                client.Close();
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