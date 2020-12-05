using System;
using System.Collections.Generic;
using System.Text;

namespace SyncordBot.Models
{
    public class ServerStats
    {
        public List<ChannelServer> Servers { get; private set; }    
        public ServerStats()
        {
            Servers = new List<ChannelServer>();
        }
    }
}
