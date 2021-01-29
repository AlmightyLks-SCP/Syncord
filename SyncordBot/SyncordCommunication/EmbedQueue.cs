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
        public int SLPort { get; set; }
        public DiscordChannel DiscordChannel { get; set; }
        public Queue<PlayerJoined> PlayerJoinedQueue { get; set; }

        public EmbedQueue()
        {
            SLPort = 0;
            DiscordChannel = null;
            PlayerJoinedQueue = new Queue<PlayerJoined>();

            ProcessDataQueue();
        }
        private async Task ProcessDataQueue()
        {
            while (true)
            {
                await Task.Delay(150000);
                if (PlayerJoinedQueue.Count != 0)
                {
                    var playerJoinedArgs = PlayerJoinedQueue.ChunkBy(10).ToList();
                    var embed = playerJoinedArgs.ToEmbed();
                    await DiscordChannel.SendMessageAsync(embed: embed);
                }
            }
        }
    }
}
