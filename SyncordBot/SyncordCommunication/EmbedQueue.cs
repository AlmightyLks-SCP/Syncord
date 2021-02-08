using DSharpPlus.Entities;
using Serilog;
using SyncordBot.Helper;
using SyncordBot.SyncordCommunication;
using SyncordInfo.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.SyncordCommunication
{
    public sealed class EmbedQueue
    {
        public DiscordChannel DiscordChannel { get; set; }
        public Dictionary<string, Queue<PlayerJoinLeave>> PlayerJoinedQueue { get; set; }
        public Dictionary<string, Queue<PlayerJoinLeave>> PlayerLeftQueue { get; set; }
        public Dictionary<string, Queue<RoundEnd>> RoundEndQueue { get; set; }
        public Dictionary<string, Queue<PlayerDeath>> PlayerDeathQueue { get; set; }
        public Dictionary<string, Queue<PlayerBan>> PlayerBanQueue { get; set; }

        private ILogger _logger;

        public EmbedQueue(ILogger logger)
        {
            DiscordChannel = null;
            PlayerJoinedQueue = new Dictionary<string, Queue<PlayerJoinLeave>>();
            PlayerLeftQueue = new Dictionary<string, Queue<PlayerJoinLeave>>();
            RoundEndQueue = new Dictionary<string, Queue<RoundEnd>>();
            PlayerDeathQueue = new Dictionary<string, Queue<PlayerDeath>>();
            PlayerBanQueue = new Dictionary<string, Queue<PlayerBan>>();

            _logger = logger;

            new Task(async () => await ProcessDataQueue()).Start();
        }
        private async Task ProcessDataQueue()
        {
            while (true)
            {
                await Task.Delay(1000);
                try
                {
                    if (PlayerJoinedQueue.Any(_ => _.Value.Count != 0))
                    {
                        var ipAndQueue = PlayerJoinedQueue.FirstOrDefault(_ => _.Value.Count != 0);

                        var playerJoinedArgs = ipAndQueue.Value.ChunkBy(25);
                        var embed = playerJoinedArgs.ToEmbed();
                        await DiscordChannel.SendMessageAsync(embed: embed);
                    }
                    else if (PlayerLeftQueue.Any(_ => _.Value.Count != 0))
                    {
                        var ipAndQueue = PlayerLeftQueue.FirstOrDefault(_ => _.Value.Count != 0);

                        var playerLeftArgs = ipAndQueue.Value.ChunkBy(25);
                        var embed = playerLeftArgs.ToEmbed();
                        await DiscordChannel.SendMessageAsync(embed: embed);
                    }
                    else if (RoundEndQueue.Any(_ => _.Value.Count != 0))
                    {
                        var ipAndQueue = RoundEndQueue.FirstOrDefault(_ => _.Value.Count != 0);

                        var roundEndArgs = ipAndQueue.Value.Dequeue();
                        var embed = roundEndArgs.ToEmbed();
                        await DiscordChannel.SendMessageAsync(embed: embed);
                    }
                    else if (PlayerDeathQueue.Any(_ => _.Value.Count != 0))
                    {
                        var ipAndQueue = PlayerDeathQueue.FirstOrDefault(_ => _.Value.Count != 0);

                        var playerDeathArgs = ipAndQueue.Value.ChunkBy(8);
                        var embed = playerDeathArgs.ToEmbed();
                        await DiscordChannel.SendMessageAsync(embed: embed);
                    }
                    else if (PlayerBanQueue.Any(_ => _.Value.Count != 0))
                    {
                        var ipAndQueue = PlayerBanQueue.FirstOrDefault(_ => _.Value.Count != 0);

                        var playerBanDeath = ipAndQueue.Value.ChunkBy(8);
                        var embed = playerBanDeath.ToEmbed();
                        await DiscordChannel.SendMessageAsync(embed: embed);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Exception in ProcessDataQueue\n{e}");
                }
            }
        }
    }
}
