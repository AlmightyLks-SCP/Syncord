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
    public class SyncordBehaviour
    {
        public Dictionary<int, int> Heartbeats { get; private set; }
        private Bot bot;
        private Dictionary<int, TcpClient> clientConnections;
        private System.Timers.Timer heartbeatTimer;
        private TcpListener tcpListener;
        private BinaryFormatter binaryFormatter;
        private ILogger logger;
        public SyncordBehaviour(Bot bot, ILogger logger)
        {
            this.bot = bot;
            clientConnections = new Dictionary<int, TcpClient>();
            tcpListener = new TcpListener(IPAddress.Loopback, bot.Configs.Port);
            Heartbeats = new Dictionary<int, int>();
            binaryFormatter = new BinaryFormatter();
            heartbeatTimer = new System.Timers.Timer();
            this.logger = logger;
        }

        public async Task Start()
        {
            Task.Run(() => _ = ListenForClient());

            heartbeatTimer.Interval = 15_000;
            heartbeatTimer.AutoReset = true;
            heartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;
            heartbeatTimer.Start();
        }

        private async Task ListenForClient()
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
                    Task.Run(() => { _ = AcceptData(acceptedClient); });
                }
                catch (Exception e)
                {
                    logger.Error($"Exception in ListenForClient:\n{e}");
                    logger.Exception($"Exception in ListenForClient:\n{e}");
                }
            }
        }
        private async Task AcceptData(TcpClient acceptedClient)
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
                    var con = clientConnections.First((_) => _.Value == acceptedClient);

                    //Remove client from storage
                    clientConnections.Remove(con.Key);
                    Heartbeats.Remove(con.Key);

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
                    var con = clientConnections.First((_) => _.Value == acceptedClient);

                    //Remove from storage
                    clientConnections.Remove(con.Key);
                    Heartbeats.Remove(con.Key);

                    //End connections.
                    acceptedClient.GetStream().Close();
                    acceptedClient.Close();
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
                            if (Heartbeats.ContainsKey(info.Port))
                                Heartbeats[info.Port]++;
                            else
                                Heartbeats.Add(info.Port, 1);
                        }
                        break;
                    case RequestType.Event:
                        {
                            LogEventInfo(info);
                        }
                        break;
                    case RequestType.Connect:
                        {
                            if (!clientConnections.Any((_) => _.Value == acceptedClient))
                            {
                                clientConnections.Add(info.Port, acceptedClient);
                                Heartbeats.Add(info.Port, 1);
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

        private async void HeartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            => await CheckHeartbeats();
        private async Task CheckHeartbeats()
        {
            try
            {
                foreach (var connection in clientConnections.ToArray())
                {
                    if (!Heartbeats.TryGetValue(connection.Key, out int val))
                        return;

                    if (val == 0) //If no hearbeats have been returned
                    {
                        logger.Warn($"No hearbeats received from port {connection.Key}. Connection closed.");

                        //Remove from storage
                        Heartbeats.Remove(connection.Key);
                        clientConnections.Remove(connection.Key);

                        //Close connection
                        connection.Value.Client.Close();
                    }
                }

                foreach (var _ in Heartbeats.ToList())
                    Heartbeats[_.Key] = 0;

                UpdateBotActivity();
                await SendHeartbeats();
            }
            catch (Exception e)
            {
                logger.Error($"Exception in Check Heartbeats:\n{e}");
                logger.Exception($"Exception in Check Heartbeats:\n{e}");
            }
        }
        private async Task SendHeartbeats()
        {
            try
            {
                foreach (var connection in clientConnections.ToArray())
                {
                    if (!connection.Value.Connected)
                        continue;

                    //Send out Heartbeats to every client
                    binaryFormatter.Serialize(connection.Value.GetStream(), new SharedInfo() { Port = bot.Configs.Port, RequestType = RequestType.Heartbeat, Content = "Heartbeat" });
                    logger.Info($"Sent {((IPEndPoint)(connection.Value.Client.RemoteEndPoint)).Port} heartbeat");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error sending Heartbeats:\n{e}");
                logger.Exception($"Error sending Heartbeats:\n{e}");
            }
        }

        private async Task QueryServerStats()
        {
            for (; ; )
            {
                foreach (var scpPort in clientConnections.Keys)
                {
                    //Query every server for stats
                }
            }
        }
        private async Task SendServerStats()
        {

        }


        private async Task UpdateBotActivity()
            => await bot.Client.UpdateStatusAsync(new DiscordActivity($"{clientConnections.Count} SCP SL Servers"), UserStatus.Online);

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
