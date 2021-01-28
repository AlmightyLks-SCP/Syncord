using DSharpPlus.Entities;
using Serilog;
using SyncordBot.Helper;
using SyncordInfo.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.SyncordCommunication
{
    public sealed class MessageQueue
    {
        private Bot _bot;
        private ILogger _logger;
        private Queue<(int SLPort, List<PlayerJoined> Args)> _playerJoinedQueue;

        public MessageQueue(Bot bot, ILogger logger)
        {
            _bot = bot;
            _logger = logger;
            _playerJoinedQueue = new Queue<(int SLPort, List<PlayerJoined> Args)>();
        }
        private async Task ProcessDataQueue()
        {
            while (true)
            {
                await Task.Delay(2500);
                if (_playerJoinedQueue.Count != 0)
                {
                    var playerJoinedArgs = _playerJoinedQueue.ChunkBy(10);
                    // To-do:
                    // - Load Chunk
                    // - Process Chunk of args to one embed
                    // - Don't forget: Discord message ratelimit is per-channel.
                }
            }
        }

        private async Task LogEventInfo(DiscordEmbed embed)
        {
            //try
            //{
            //    foreach (var dedicatedGuild in Bot.Configs.Guilds.Where((_) => _.ServerPort == (info.Port)))
            //    {
            //        if (dedicatedGuild is null)
            //        {
            //            _logger.Warning($"No dedicated server found for Port {info.Port}");
            //            continue;
            //        }

            //        var guild = await _bot.Client.GetGuildAsync(dedicatedGuild.GuildID);

            //        if (guild is null)
            //        {
            //            _logger.Warning($"No Guild found for Guild ID {dedicatedGuild.GuildID}");
            //            continue;
            //        }

            //        foreach (var dedicatedChannel in dedicatedGuild.DedicatedChannels.Where((_) => _.Key == embed.Title))
            //        {
            //            var channel = guild.GetChannel(dedicatedChannel.Value);

            //            if (channel is null)
            //            {
            //                _logger.Warning($"No Channel found for Channel ID {dedicatedChannel} | Guild {dedicatedGuild.GuildID}");
            //                continue;
            //            }

            //            await channel.SendMessageAsync(embed: embed);
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    _logger.Error($"Exception in LogEventInfo");
            //}
        }
    }
}
