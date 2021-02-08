using System;
using System.Collections.Generic;
using System.Net;
using EasyCommunication.Connection;
using EasyCommunication.Events.Client.EventArgs;
using MEC;
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
            EasyClient = new EasyClient(3500)
            {
                BufferSize = 2048
            };
            Timing.RunCoroutine(AutoReconnect());
            EasyClient.EventHandler.ConnectedToHost += OnConnectedToHost;
            EasyClient.EventHandler.DisconnectedFromHost += OnDisconnectedFromHost;
        }
        private IEnumerator<float> AutoReconnect()
        {
            for (; ; )
            {
                yield return Timing.WaitForSeconds(5f);
                try
                {
                    if (EasyClient.ClientConnected || !SyncordPlugin.Config.AutoReconnect)
                        continue;
                    if (IPAddress.TryParse(SyncordPlugin.Config.DiscordBotAddress, out IPAddress botAddress))
                        EasyClient.ConnectToHost(botAddress, SyncordPlugin.Config.DiscordBotPort);
                    if (SyncordPlugin.Config.DebugMode && EasyClient.ClientConnected)
                        Logger.Get.Info($"Reconnected");
                }
                catch (Exception e)
                {
                    Logger.Get.Info($"Error in AutoReconnect:");
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
