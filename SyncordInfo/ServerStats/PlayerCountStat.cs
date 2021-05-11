using System;

namespace SyncordInfo.ServerStats
{
    public struct PlayerCountStat
    {
        public ushort MaxPlayers { get; set; }
        public ushort PlayerCount { get; set; }
        public DateTime DateTime { get; set; }
    }
}
