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
    public sealed class PlayerJoinLeave
    {
        [ProtoMember(1)]
        public bool SameMachine { get; set; }
        [ProtoMember(2)]
        public string SLFullAddress { get; set; }
        [ProtoMember(3)]
        public string Identifier { get; set; } // "join" / "leave"
        [ProtoMember(4)]
        public SimplePlayer Player { get; set; }
        [ProtoMember(5)]
        public DateTime Time { get; set; }
    }
}
