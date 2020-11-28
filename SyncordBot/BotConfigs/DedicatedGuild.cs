using System;
using System.Collections.Generic;
using System.Text;

namespace SyncordBot.BotConfigs
{
    public class DedicatedGuild
    {
        public ulong GuildID { get; set; } = 0;
        public int ServerPort { get; set; } = 0;
        public Dictionary<string, ulong> DedicatedChannels { get; set; } = new Dictionary<string, ulong>();
        public DedicatedGuild()
        {
            GuildID = 0;
            ServerPort = 0;
            DedicatedChannels.Add("Ya Yeet", 0);
        }
    }
}
