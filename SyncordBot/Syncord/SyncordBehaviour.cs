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
        private Dictionary<int, TcpClient> tcpClients;
        private Dictionary<int, int> heartBeats;
        private TcpListener tcpListener;
        private DiscordClient dClient;
        internal SyncordBehaviour(DiscordClient client)
        {
            dClient = client;
            tcpClients = new Dictionary<int, TcpClient>();
            tcpListener = new TcpListener(IPAddress.Loopback, Bot.Configs.Port);
            heartBeats = new Dictionary<int, int>();
        }

        internal async Task Start()
        {
            await Task.Run(() => { _ = ListenForClient(); });
            await Task.Run(() => { _ = CheckHeartBeats(); });
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
                BinaryFormatter formatter = new BinaryFormatter();

                if (!acceptedClient.Connected)
                {
                    Console.WriteLine($"Client not connected..");
                    acceptedClient.Close();
                    return;
                }

                SharedInfo info = formatter.Deserialize(acceptedClient.GetStream()) as SharedInfo;
                await HandleData(info, acceptedClient);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (tcpClients.Any((_) => _.Value == acceptedClient))
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
                int port = ((IPEndPoint)acceptedClient.Client.RemoteEndPoint).Port;

                switch (info.Content)
                {
                    case "heartbeat":
                        {
                            if (heartBeats.ContainsKey(port))
                                heartBeats[port]++;
                            else
                                heartBeats.Add(port, 0);
                        }
                        break;
                    default:
                        {
                            tcpClients.Add(port, acceptedClient);

                            var embed = JsonConvert.DeserializeObject<DiscordEmbed>(info.Content);

                            var dedicatedGuild = Bot.Configs.Guilds.FirstOrDefault((_) => _.ServerPort == (port));

                            if (dedicatedGuild is null)
                            {
                                Console.WriteLine($"No dedicated server found for Port {port}");
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
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task CheckHeartBeats()
        {
            for (; ; )
            {
                foreach (var connection in tcpClients.ToArray())
                {
                    if (heartBeats[connection.Key] == 0)
                    {
                        heartBeats.Remove(connection.Key);
                        tcpClients.Remove(connection.Key);
                    }
                    else
                    {
                        heartBeats[connection.Key]++;
                    }

                }
                await Task.Delay(10000);
            }
        }
    }
}
