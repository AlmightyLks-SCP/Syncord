using DSharpPlus;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using SyncordInfo;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SyncordBot.Syncord
{
    internal class SyncordBehaviour
    {
        private List<TcpClient> tcpClients;
        private TcpListener tcpListener;
        private DiscordClient dClient;
        internal SyncordBehaviour(DiscordClient client)
        {
            dClient = client;
            tcpClients = new List<TcpClient>();
            tcpListener = new TcpListener(IPAddress.Loopback, Bot.Configs.Port);
        }
        internal async Task Start()
            => await Task.Run(() => { _ = ListenForClient(); });

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
                for (; ; )
                {
                    if (!acceptedClient.Connected)
                    {
                        Console.WriteLine($"Client not connected..");
                        acceptedClient.Close();
                        continue;
                    }

                    SharedInfo info = formatter.Deserialize(acceptedClient.GetStream()) as SharedInfo;
                    await HandleData(info, acceptedClient);
                }
            }
            catch (SerializationException e)
            {
                Console.WriteLine($"Deserialization failed, closing connection...\n{e}");

                if (tcpClients.Remove(acceptedClient))
                    acceptedClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task HandleData(SharedInfo info, TcpClient acceptedClient)
        {
            try
            {
                if (info is null)
                {
                    Console.WriteLine("Received data null");
                    return;
                }

                var embed = JsonConvert.DeserializeObject<DiscordEmbed>(info.Content);
                await dClient.Guilds[727996170051518504].GetChannel(782245173526134814).SendMessageAsync(embed: embed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
