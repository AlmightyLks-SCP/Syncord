using ProtoBuf;
using SyncordInfo.SimplifiedTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    public sealed class PlayerBan : SynEventArgs
    {
        [ProtoMember(4)]
        public SimplePlayer BannedPlayer { get; set; }
        [ProtoMember(5)]
        public SimplePlayer BanningPlayer { get; set; }
        [ProtoMember(6)]
        public int Duration { get; set; }
        [ProtoMember(7)]
        public string Reason { get; set; }
    }
}
