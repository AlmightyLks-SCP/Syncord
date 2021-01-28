using EasyCommunication.Host.Connection;
using EasyCommunication.Helper;
using Serilog;
using SyncordInfo;
using Newtonsoft.Json;
using DSharpPlus.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;
using EasyCommunication.Events.Host.EventArgs;
using EasyCommunication.SharedTypes;
using SyncordInfo.EventArgs;

namespace SyncordBot.SyncordCommunication
{
    public sealed class CommunicationHandler
    {
        private Bot _bot;
        private EasyHost _easyHost;
        private ILogger _logger;

        public CommunicationHandler(EasyHost easyHost, Bot bot, ILogger logger)
        {
            _bot = bot;
            _logger = logger;
            _easyHost = easyHost;

            easyHost.EventHandler.ReceivedData += ReceivedDataFromSLServer;
            easyHost.EventHandler.ClientConnected += SLServerConnected;
            easyHost.EventHandler.ClientDisconnected += SLServerDisconnected;

            _easyHost.Open();
        }

        private void SLServerDisconnected(ClientDisconnectedEventArgs ev)
        {
            Console.WriteLine("A client disconnected");
        }
        private void SLServerConnected(ClientConnectedEventArgs ev)
        {
            Console.WriteLine("A client connected");
        }
        private void ReceivedDataFromSLServer(ReceivedDataEventArgs ev)
        {
            switch (ev.Type)
            {
                case DataType.ProtoBuf:
                    {
                        if (ev.Data.TryDeserialize(out PlayerJoined joined))
                        {
                            Console.WriteLine($"{joined.Player.Nickname} joined!");
                        }
                        break;
                    }
                default:
                    break;
            }
        }

    }
}
