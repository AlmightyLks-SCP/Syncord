using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.ServerStats
{
    public struct DeathStat
    {
        public DateTime DateTime { get; set; }
        public SimplePlayer Killer { get; set; }
        public SimplePlayer Victim { get; set; }
    }
}
