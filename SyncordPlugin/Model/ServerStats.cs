using System.Collections.Generic;
using SyncordInfo.ServerStats;

namespace SyncordPlugin.Model
{
    public sealed class ServerStats
    {
        public List<DeathStat> DeathStats { get; set; }
        public List<FpsStat> ServerFpsStats { get; set; }
        public ServerStats()
        {
            DeathStats = new List<DeathStat>();
            ServerFpsStats = new List<FpsStat>();
        }
    }
}
