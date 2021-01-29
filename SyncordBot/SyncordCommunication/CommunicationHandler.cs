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

namespace SyncordBot.SyncordCommunication
{
    public sealed class CommunicationHandler
    {
        private Bot _bot;
        private EasyHost _easyHost;
        private List<EmbedQueue> _embedQueue;
        private ILogger _logger;

        public CommunicationHandler(EasyHost easyHost, Bot bot, ILogger logger)
        {
            _bot = bot;
            _logger = logger;
            _easyHost = easyHost;
            _embedQueue = new List<EmbedQueue>();

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

                        EmbedQueue embedQueueElement = _embedQueue.FirstOrDefault(_ => _.DiscordChannel.Id == dedicatedChannel.Value);
                        if (embedQueueElement == null)
                        {
                            _embedQueue.Add(new EmbedQueue()
                            {
                                DiscordChannel = channel,
                                SLPort = dedicatedGuild.ServerPort
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Error($"Exception in LogEventInfo");
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
                        if (ev.Data.TryDeserialize(out PlayerJoined joined))
                        {
                            Console.WriteLine($"{joined.Player.Nickname} joined!");
                            joined.ServerAddress = ev.Sender?.GetIPv4();
                            var embedQueueElement = _embedQueue.FirstOrDefault(_ => _.SLPort == joined.ServerPort);
                            embedQueueElement.PlayerJoinedQueue.Enqueue(joined);
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

        //private async Task LogEventInfo(string type)
        //{
        //    try
        //    {
        //        foreach (var dedicatedGuild in Bot.Configs.Guilds.Where((_) => _.ServerPort == (info.Port)))
        //        {
        //            if (dedicatedGuild is null)
        //            {
        //                _logger.Warning($"No dedicated server found for Port {info.Port}");
        //                continue;
        //            }

        //            var guild = await _bot.Client.GetGuildAsync(dedicatedGuild.GuildID);

        //            if (guild is null)
        //            {
        //                _logger.Warning($"No Guild found for Guild ID {dedicatedGuild.GuildID}");
        //                continue;
        //            }

        //            foreach (var dedicatedChannel in dedicatedGuild.DedicatedChannels.Where((_) => _.Key == embed.Title))
        //            {
        //                var channel = guild.GetChannel(dedicatedChannel.Value);

        //                if (channel is null)
        //                {
        //                    _logger.Warning($"No Channel found for Channel ID {dedicatedChannel} | Guild {dedicatedGuild.GuildID}");
        //                    continue;
        //                }

        //                await channel.SendMessageAsync(embed: embed);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.Error($"Exception in LogEventInfo");
        //    }
        //}
    }
}
