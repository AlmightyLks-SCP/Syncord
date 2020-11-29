using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SyncordInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace SyncordBot.Syncord
{
    internal class SyncordBehaviour
    {
        private Dictionary<int, TcpClient> clientConnections;
        internal Dictionary<int, int> heartbeats { get; private set; }
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

        internal async Task/*<System.Timers.Timer>*/ Start()
        {
            Console.WriteLine("In here");
            Task.Run(() => { _ = ListenForClient(); });
            heartbeatTimer.Interval = 15000;
            heartbeatTimer.AutoReset = true;
            heartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;
            heartbeatTimer.Start();
            //return heartbeatTimer;
        }

        private async Task ListenForClient()
        {
            for (; ; )
            {
                try
                {
                    tcpListener.Start();
                    TcpClient acceptedClient = tcpListener.AcceptTcpClient();
                    _ = Task.Run(() => { _ = AcceptClient(acceptedClient); });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private async Task AcceptClient(TcpClient acceptedClient)
        {
            try
            {

                if (!acceptedClient.Connected)
                {
                    Console.WriteLine($"Client not connected..");
                    return;
                }

                SharedInfo info = binaryFormatter.Deserialize(acceptedClient.GetStream()) as SharedInfo;
                await HandleData(info, acceptedClient);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientConnections.Any((_) => _.Value == acceptedClient))
                    acceptedClient.Close();
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
                                clientConnections.Add(senderPort, acceptedClient);

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
                Console.WriteLine(e);
            }
        }

        private void HeartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("Timer Elapsed.");
            _ = CheckHeartbeats();
        }

        private async Task SendHeartbeats()
        {
            for (; ; )
            {
                foreach (var connection in clientConnections.ToArray())
                {
                    binaryFormatter.Serialize(connection.Value.GetStream(), new SharedInfo() { Content = "heartbeat" });
                }

                await Task.Delay(15000);
            }
        }

        private async Task CheckHeartbeats()
        {
            try
            {
                foreach (var connection in clientConnections.ToArray())
                {
                    if (heartbeats[connection.Key] == 0)
                    {
                        Console.WriteLine($"No hearbeats received from port {connection.Key}. Connection closed.");
                        heartbeats.Remove(connection.Key);
                        clientConnections.Remove(connection.Key);
                        connection.Value.Client.Close();
                    }
                    else
                    {
                        Console.WriteLine("Every heartbeat is fine");
                    }
                }

                foreach (var _ in heartbeats.ToList())
                    heartbeats[_.Key] = 0;

                Console.WriteLine("Resetted hearbeats");

                _ = SendHeartbeats();

                Console.WriteLine("Heartbeats sent");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:\n{e}");
            }
        }

        /*
        private async Task SendHeartbeats()
        {
            for (; ; )
            {
                foreach (var connection in tcpClients.ToArray())
                {
                    binaryFormatter.Serialize(connection.Value.Client.GetStream(), new SharedInfo() { Content = "heartbeat" });
                    connection.Value.Timer.Interval = 
                }

                await Task.Delay(15000);
            }
        }
        private async Task ResetHearbeats()
        {
            for (; ; )
            {
                foreach (var _ in heartbeats.ToList())
                    heartbeats[_.Key] = 0;

                await Task.Delay(12000);

                Console.WriteLine("Resetted hearbeats.");
            }
        }
        private async Task CheckHeartbeats()
        {
            await Task.Delay(5000);
            for (; ; )
            {
                foreach (var connection in tcpClients.ToArray())
                {
                    if (heartbeats[connection.Key] == 0)
                    {
                        Console.WriteLine($"No hearbeats received from port {connection.Key}. Connection closed.");
                        heartbeats.Remove(connection.Key);
                        tcpClients.Remove(connection.Key);
                        connection.Value.Client.Close();
                    }
                }
                await Task.Delay(30000);
            }
        }
    */
    }
}
