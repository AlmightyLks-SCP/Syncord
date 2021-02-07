/*
    The following license applies to the entirety of this Repository and Solution.
    
    TLDR.: Don't use a damn thing from my work without crediting me, else I'll smite your arse.
    
    Copyright 2021 AlmightyLks

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the License for the specific language governing
    permissions and limitations under the License.
*/
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
