using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.ServerStats
{
    public struct PlayerCountStat
    {
        public int MaxPlayers { get; set; }
        public int PlayerCount { get; set; }
        public DateTime DateTime { get; set; }
    }
}
