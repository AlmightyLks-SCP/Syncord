using ProtoBuf;
using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    public sealed class PlayerDeath : SynEventArgs
    {
        [ProtoMember(4)]
        public SimplePlayer Victim { get; set; }
        [ProtoMember(5)]
        public SimplePlayer Killer { get; set; }
        [ProtoMember(6)]
        public SimpleHitInfo HitInfo { get; set; }
    }
}
