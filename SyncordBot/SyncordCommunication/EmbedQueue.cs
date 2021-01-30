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
        public Dictionary<string, Queue<PlayerReport>> PlayerReportQueue { get; set; }

        private ILogger _logger;

        public EmbedQueue(ILogger logger)
        {
            DiscordChannel = null;
            PlayerJoinedQueue = new Dictionary<string, Queue<PlayerJoinLeave>>();
            PlayerLeftQueue = new Dictionary<string, Queue<PlayerJoinLeave>>();
            PlayerReportQueue = new Dictionary<string, Queue<PlayerReport>>();
            _logger = logger;

            ProcessDataQueue();
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
                    else if (PlayerReportQueue.Any(_ => _.Value.Count != 0))
                    {
                        var ipAndQueue = PlayerReportQueue.FirstOrDefault(_ => _.Value.Count != 0);

                        var playerReportArgs = ipAndQueue.Value.ChunkBy(25);
                        var embed = playerReportArgs.ToEmbed();
                        await DiscordChannel.SendMessageAsync(embed: embed);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Exception in ProcessDataQueue");
                }
            }
        }
    }
}
