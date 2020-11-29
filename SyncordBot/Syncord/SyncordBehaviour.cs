using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
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
    internal class SyncordBehaviour
    {
        internal Dictionary<int, int> heartbeats { get; private set; }
        private Dictionary<int, TcpClient> clientConnections;
        private System.Timers.Timer heartbeatTimer;
        private TcpListener tcpListener;
        private DiscordClient dClient;
        private BinaryFormatter binaryFormatter;
        internal SyncordBehaviour(DiscordClient client)
        {
            dClient = client;
            clientConnections = new Dictionary<int, TcpClient>();
            tcpListener = new TcpListener(IPAddress.Loopback, Bot.Configs.Port);
            heartbeats = new Dictionary<int, int>();
            binaryFormatter = new BinaryFormatter();
            heartbeatTimer = new System.Timers.Timer();
        }

        internal async Task Start()
        {
            Console.WriteLine("In here");
            _ = Task.Run(() => { _ = ListenForClient(); });
            heartbeatTimer.Interval = 5000;
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
                    tcpListener.Start();
                    TcpClient acceptedClient = tcpListener.AcceptTcpClient();
                    Task.Run(() => { _ = AcceptData(acceptedClient); });
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception in listen:\n{e}");
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
                        Console.WriteLine($"Client not connected..");
                        return;
                    }

                    SharedInfo info = binaryFormatter.Deserialize(acceptedClient.GetStream()) as SharedInfo;
                    HandleData(info, acceptedClient);
                }
                catch (IOException e)
                {
                    int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;
                    Console.WriteLine($"Socket connection for {port} was closed unexpectedly.");
                    clientConnections.Remove(port);
                    heartbeats.Remove(port);
                    break;
                }
                catch (SerializationException e)
                {
                    int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;
                    Console.WriteLine($"Could not serialize received data from {port}.");
                    clientConnections.Remove(port);
                    heartbeats.Remove(port);
                    break;
                }
                catch (Exception e)
                {
                    int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;
                    Console.WriteLine($"Exception on tcp for port {port}:\n{e}");
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
                    Console.WriteLine("Received invalid data");
                    return;
                }
                int senderPort = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;

                switch (info.Content)
                {
                    case "heartbeat":
                        {
                            if (heartbeats.ContainsKey(senderPort))
                                heartbeats[senderPort]++;
                            else
                                heartbeats.Add(senderPort, 1);
                        }
                        break;
                    default:
                        {
                            if (!clientConnections.Any((_) => _.Value == acceptedClient))
                            {
                                clientConnections.Add(senderPort, acceptedClient);
                                heartbeats.Add(senderPort, 1);
                            }

                            var embed = JsonConvert.DeserializeObject<DiscordEmbed>(info.Content);

                            foreach (var dedicatedGuild in Bot.Configs.Guilds.Where((_) => _.ServerPort == (senderPort)))
                            {
                                if (dedicatedGuild is null)
                                {
                                    Console.WriteLine($"No dedicated server found for Port {senderPort}");
                                    return;
                                }

                                var guild = await dClient.GetGuildAsync(dedicatedGuild.GuildID);

                                if (guild is null)
                                {
                                    Console.WriteLine($"No Guild found for Guild ID {dedicatedGuild.GuildID}");
                                    return;
                                }

                                foreach (var dedicatedChannel in dedicatedGuild.DedicatedChannels.Where((_) => _.Key == embed.Title))
                                {
                                    var channel = guild.GetChannel(dedicatedChannel.Value);

                                    if (channel is null)
                                    {
                                        Console.WriteLine($"No Channel found for Channel ID {dedicatedChannel} | Guild {dedicatedGuild.GuildID}");
                                        return;
                                    }

                                    await channel.SendMessageAsync(embed: embed);
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in Handle data:\n{e}");
            }
        }

        private async void HeartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await CheckHeartbeats();
        }
        private async Task SendHeartbeats()
        {
            try
            {
                foreach (var connection in clientConnections.ToArray())
                {
                    if (!connection.Value.Connected)
                        continue;
                    binaryFormatter.Serialize(connection.Value.GetStream(), new SharedInfo() { Content = "heartbeat" });
                    Console.WriteLine($"Sent {((IPEndPoint)(connection.Value.Client.RemoteEndPoint)).Port} heartbeat");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending Heartbeats:\n{e}");
            }
        }
        private async Task CheckHeartbeats()
        {
            try
            {
                foreach (var connection in clientConnections.ToArray())
                {
                    if (!heartbeats.TryGetValue(connection.Key, out int val))
                        return;

                    if (val == 0)
                    {
                        Console.WriteLine($"No hearbeats received from port {connection.Key}. Connection closed.");
                        heartbeats.Remove(connection.Key);
                        clientConnections.Remove(connection.Key);
                        connection.Value.Client.Close();
                    }
                }

                foreach (var _ in heartbeats.ToList())
                    heartbeats[_.Key] = 0;

                await SendHeartbeats();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception in Check Heartbeats:\n{e}");
            }
        }
    }
}
