using System.Net;
using EasyCommunication.Client.Connection;
using EasyCommunication.Events.Client.EventArgs;
using Synapse.Api;

namespace SyncordPlugin.Syncord
{
    public class CommunicationHandler
    {
        public static CommunicationHandler Singleton { get; private set; }
        public EasyClient EasyClient;

        public CommunicationHandler()
        {
            Singleton = this;
            EasyClient = new EasyClient();

            EasyClient.EventHandler.ConnectedToHost += OnConnectedToHost;
            EasyClient.EventHandler.DisconnectedFromHost += OnDisconnectedFromHost;
        }

        private void OnDisconnectedFromHost(DisconnectedFromHostEventArgs ev)
        {
            Logger.Get.Info("Lost connection to host");
        }
        private void OnConnectedToHost(ConnectedToHostEventArgs ev)
        {
            Logger.Get.Info("Connected to host");
        }

        public static bool ConnectToHost()
        {
            try
            {
                if (IPAddress.TryParse(SyncordPlugin.Config.DiscordBotAddress, out IPAddress botAddress))
                    Singleton.EasyClient.ConnectToHost(botAddress, SyncordPlugin.Config.DiscordBotPort);
                else
                    return false;   //Invalid IP
            }
            catch
            {
                return false;       //Invalid Host
            }
            return true;
        }
        public static void DisconnectFromHost()
            => Singleton.EasyClient.DisconnectFromHost();
    }
}
