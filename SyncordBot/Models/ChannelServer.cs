using System.Collections.Generic;

namespace SyncordBot.Models
{
    public class ChannelServer
    {
        public ulong GuildID { get; set; }
        public ulong ChannelID { get; set; }
        public ulong MessageID { get; set; }
        public HashSet<int> ServerPorts { get; set; }
    }
}
