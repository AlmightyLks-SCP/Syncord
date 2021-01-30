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
using System.Collections.Generic;
using SyncordBot.BotConfigs;
using SyncordBot.Models;

namespace SyncordBot.SyncordCommunication
{
    public sealed class CommunicationHandler
    {
        private Bot _bot;
        private EasyHost _easyHost;
        private List<EmbedQueue> _embedQueues;
        private ILogger _logger;

        public CommunicationHandler(EasyHost easyHost, Bot bot, ILogger logger)
        {
            _bot = bot;
            _logger = logger;
            _easyHost = easyHost;
            _embedQueues = new List<EmbedQueue>();

            easyHost.EventHandler.ReceivedData += ReceivedDataFromSLServer;
            easyHost.EventHandler.ClientConnected += SLServerConnected;
            easyHost.EventHandler.ClientDisconnected += SLServerDisconnected;

            _easyHost.Open();
        }

        public async Task CreateChannelEmbedQueues()
        {
            foreach (DedicatedGuild dedicatedGuild in Bot.Configs.Guilds)
            {
                if (dedicatedGuild == null)
                    continue;

                var guild = await _bot.Client.GetGuildAsync(dedicatedGuild.GuildID);

                if (guild == null)
                {
                    _logger.Warning($"No Guild found for Guild ID {dedicatedGuild.GuildID}");
                    continue;
                }

                foreach (var dedicatedChannel in dedicatedGuild.DedicatedChannels)
                {
                    try
                    {
                        var channel = guild.GetChannel(dedicatedChannel.Value);
                        if (channel == null)
                        {
                            _logger.Warning($"No Channel found for Channel ID {dedicatedChannel} | Guild {dedicatedGuild.GuildID}");
                            continue;
                        }

                        EmbedQueue embedQueueElement = _embedQueues.FirstOrDefault(_ => _.DiscordChannel.Id == dedicatedChannel.Value);
                        if (embedQueueElement == null)
                        {
                            var embedQueue = new EmbedQueue(_logger)
                            {
                                DiscordChannel = channel
                            };

                            if(dedicatedChannel.Key == EventTypes.PlayerJoin)
                            {
                                embedQueue.PlayerJoinedQueue = new Dictionary<string, Queue<PlayerJoinLeave>>()
                                {
                                    { dedicatedGuild.SLFullAddress, new Queue<PlayerJoinLeave>() }
                                };
                            }
                            else if(dedicatedChannel.Key == EventTypes.PlayerLeave)
                            {
                                embedQueue.PlayerLeftQueue = new Dictionary<string, Queue<PlayerJoinLeave>>()
                                {
                                    { dedicatedGuild.SLFullAddress, new Queue<PlayerJoinLeave>() }
                                };
                            }
                            _embedQueues.Add(embedQueue);
                        }
                        else
                        {
                            if (dedicatedChannel.Key == EventTypes.PlayerJoin)
                            {
                                embedQueueElement.PlayerJoinedQueue.Add(dedicatedGuild.SLFullAddress, new Queue<PlayerJoinLeave>());
                            }
                            else if (dedicatedChannel.Key == EventTypes.PlayerLeave)
                            {
                                embedQueueElement.PlayerLeftQueue.Add(dedicatedGuild.SLFullAddress, new Queue<PlayerJoinLeave>());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Exception in CreateChannelEmbedQueues");
                    }
                }
            }
        }

        private void ReceivedDataFromSLServer(ReceivedDataEventArgs ev)
        {
            switch (ev.Type)
            {
                case DataType.ProtoBuf:
                    {
                        if (ev.Data.TryDeserialize(out PlayerJoinLeave joinLeave))
                        {
                            if (joinLeave.Identifier == "join")
                            {
                                Console.WriteLine($"{joinLeave.Player.Nickname} joined {joinLeave.SLFullAddress}!");
                                string ipAddress = joinLeave.SameMachine ? $"127.0.0.1:{joinLeave.SLFullAddress.Split(':')[1]}" : joinLeave.SLFullAddress;

                                var embedQueueElement = _embedQueues.FirstOrDefault(_ => _.PlayerJoinedQueue.ContainsKey(ipAddress));
                                if (embedQueueElement == null)
                                {
                                    _logger.Warning($"ReceivedDataFromSLServer: Received join data from unconfigured SL Server");
                                    return;
                                }
                                Queue<PlayerJoinLeave> joinQueue = embedQueueElement.PlayerJoinedQueue[ipAddress];
                                if (joinQueue == null)
                                {
                                    _logger.Warning($"ReceivedDataFromSLServer: A configured SL Server has a null-queue");
                                    return;
                                }
                                joinQueue.Enqueue(joinLeave);
                            }
                            else if (joinLeave.Identifier == "leave")
                            {
                                Console.WriteLine($"{joinLeave.Player.Nickname} left {joinLeave.SLFullAddress}!");
                                string ipAddress = joinLeave.SameMachine ? $"127.0.0.1:{joinLeave.SLFullAddress.Split(':')[1]}" : joinLeave.SLFullAddress;

                                var embedQueueElement = _embedQueues.FirstOrDefault(_ => _.PlayerLeftQueue.ContainsKey(ipAddress));
                                if (embedQueueElement == null)
                                {
                                    _logger.Warning($"ReceivedDataFromSLServer: Received leave data from unconfigured SL Server");
                                    return;
                                }
                                Queue<PlayerJoinLeave> joinQueue = embedQueueElement.PlayerLeftQueue[ipAddress];
                                if (joinQueue == null)
                                {
                                    _logger.Warning($"ReceivedDataFromSLServer: A configured SL Server has a null-queue");
                                    return;
                                }
                                joinQueue.Enqueue(joinLeave);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
        }
        private void SLServerDisconnected(ClientDisconnectedEventArgs ev)
        {
            Console.WriteLine("A client disconnected");
        }
        private void SLServerConnected(ClientConnectedEventArgs ev)
        {
            Console.WriteLine("A client connected");
        }
    }
}
