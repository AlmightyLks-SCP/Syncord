using System.Collections.Generic;
using SyncordInfo.ServerStats;

namespace SyncordPlugin.Model
{
    public sealed class ServerStats
    {
        public LimitedSizeStack<DeathStat> DeathStats { get; private set; }
        public LimitedSizeStack<FpsStat> ServerFpsStats { get; private set; }
        public ServerStats()
        {
            DeathStats = new LimitedSizeStack<DeathStat>(500);
            ServerFpsStats = new LimitedSizeStack<FpsStat>(1000);
        }
    }
}
