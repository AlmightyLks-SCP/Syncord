using System.Collections.Generic;

namespace SyncordBot.BotConfigs
{
    public class DedicatedGuild
    {
        public ulong GuildID { get; set; } = 0;
        public int ServerPort { get; set; } = 0;
        public Dictionary<string, ulong> DedicatedChannels { get; set; } = new Dictionary<string, ulong>();
    }
}
