using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SyncordBot.Logging;
using SyncordBot.Models;
using SyncordInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SyncordBot.Syncord
{
    public class SyncordConnection
    {
        public Dictionary<int, TcpClient> ClientConnections { get; private set; }
        private TcpListener tcpListener;
        private BinaryFormatter binaryFormatter;
        private Bot bot;
        private ILogger logger;

        public SyncordConnection(ILogger logger, Bot bot)
        {
            this.logger = logger;
            this.bot = bot;
            ClientConnections = new Dictionary<int, TcpClient>();
            tcpListener = new TcpListener(IPAddress.Loopback, bot.Configs.Port);
            binaryFormatter = new BinaryFormatter();
        }

        public void Start()
            => Task.Run(() => ListenForClient());

        private void ListenForClient()
        {
            for (; ; )
            {
                try
                {
                    logger.Info($"Bot is listening for connections on {IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString())} on port {((IPEndPoint)tcpListener.LocalEndpoint).Port}");

                    //Listen for clients
                    tcpListener.Start();
                    TcpClient acceptedClient = tcpListener.AcceptTcpClient();

                    //Handle each client individually.
                    Task.Run(() => { AcceptData(acceptedClient); });
                }
                catch (Exception e)
                {
                    logger.Error($"Exception in ListenForClient:\n{e}");
                    logger.Exception($"Exception in ListenForClient:\n{e}");
                }
            }
        }
        private void AcceptData(TcpClient acceptedClient)
        {
            for (; ; )
            {
                try
                {
                    if (!acceptedClient.Connected)
                    {
                        logger.Warn($"Client not connected..");
                        return;
                    }

                    //Accept SharedInfo data from the client.
                    SharedInfo info = binaryFormatter.Deserialize(acceptedClient.GetStream()) as SharedInfo;
                    HandleData(info, acceptedClient);
                }
                catch (IOException e)
                {
                    int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;
                    logger.Error($"Socket connection for {port} was closed unexpectedly.");
                    logger.Exception($"Socket connection for {port} was closed unexpectedly.\n{e}");

                    //Find connection
                    var con = ClientConnections.First((_) => _.Value == acceptedClient);

                    //Remove client from storage
                    ClientConnections.Remove(con.Key);
                    bot.Heartbeat.Heartbeats.Remove(con.Key);

                    //End connections.
                    acceptedClient.GetStream().Close();
                    acceptedClient.Close();
                    break;
                }
                catch (SerializationException e)
                {
                    int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;
                    logger.Error($"Could not serialize received data from {port}.");
                    logger.Exception($"Could not serialize received data from {port}.\n{e}");

                    //Find client
                    var con = ClientConnections.First((_) => _.Value == acceptedClient);

                    //Remove from storage
                    ClientConnections.Remove(con.Key);
                    bot.Heartbeat.Heartbeats.Remove(con.Key);

                    //End connections.
                    acceptedClient.GetStream().Close();
                    acceptedClient.Close();
                    break;
                }
                catch (InvalidOperationException e)
                {
                    int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;
                    logger.Error($"Exception on client for port {port}:\n{e}");
                    logger.Exception($"Exception on client for port {port}:\n{e}");
                    break;
                }
                catch (Exception e)
                {
                    int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;
                    logger.Error($"Exception on client for port {port}:\n{e}");
                    logger.Exception($"Exception on client for port {port}:\n{e}");
                    break;
                }
            }
        }
        private async Task HandleData(SharedInfo info, TcpClient acceptedClient)
        {
            try
            {
                if (info is null)
                {
                    logger.Error("Received invalid data");
                    logger.Exception("Received invalid data");
                    return;
                }

                switch (info.RequestType)
                {
                    case RequestType.Heartbeat:
                        {
                            if (bot.Heartbeat.Heartbeats.ContainsKey(info.Port))
                                bot.Heartbeat.Heartbeats[info.Port]++;
                            else
                                bot.Heartbeat.Heartbeats.Add(info.Port, 1);
                        }
                        break;
                    case RequestType.Event:
                        {
                            LogEventInfo(info);
                        }
                        break;
                    case RequestType.Connect:
                        {
                            if (!ClientConnections.Any((_) => _.Value == acceptedClient))
                            {
                                ClientConnections.Add(info.Port, acceptedClient);
                                bot.Heartbeat.Heartbeats.Add(info.Port, 1);
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Error($"Exception in Handle data:\n{e}");
                logger.Exception($"Exception in Handle data:\n{e}");
            }
        }
        private async void LogEventInfo(SharedInfo info)
        {
            try
            {
                var embed = JsonConvert.DeserializeObject<DiscordEmbed>(info.Content);

                foreach (var dedicatedGuild in bot.Configs.Guilds.Where((_) => _.ServerPort == (info.Port)))
                {
                    if (dedicatedGuild is null)
                    {
                        logger.Warn($"No dedicated server found for Port {info.Port}");
                        continue;
                    }

                    var guild = await bot.Client.GetGuildAsync(dedicatedGuild.GuildID);

                    if (guild is null)
                    {
                        logger.Warn($"No Guild found for Guild ID {dedicatedGuild.GuildID}");
                        continue;
                    }

                    foreach (var dedicatedChannel in dedicatedGuild.DedicatedChannels.Where((_) => _.Key == embed.Title))
                    {
                        var channel = guild.GetChannel(dedicatedChannel.Value);

                        if (channel is null)
                        {
                            logger.Warn($"No Channel found for Channel ID {dedicatedChannel} | Guild {dedicatedGuild.GuildID}");
                            continue;
                        }

                        await channel.SendMessageAsync(embed: embed);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Warn($"Exception in LogEventInfo");
                logger.Exception($"Exception in LogEventInfo\n{e}");
            }
        }
    }
}
