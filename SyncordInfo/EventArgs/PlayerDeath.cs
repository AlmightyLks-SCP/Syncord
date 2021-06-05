using SyncordInfo.Communication;
using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.EventArgs
{
    public sealed class PlayerDeath : DataBase
    {
        public SimplePlayer Victim { get; set; }
        public SimplePlayer Killer { get; set; }
        public SimpleHitInfo HitInfo { get; set; }
    }
}
