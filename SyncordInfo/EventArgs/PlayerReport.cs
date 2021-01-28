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
    public sealed class PlayerReport
    {
        [ProtoMember(1)]
        public int ServerPort { get; set; }
        [ProtoMember(2)]
        public SimplePlayer Reporter { get; }
        [ProtoMember(3)]
        public SimplePlayer Target { get; }
        [ProtoMember(4)]
        public string Reason { get; }
        [ProtoMember(5)]
        public bool GlobalReport { get; set; }
        [ProtoMember(6)]
        public bool Allow { get; set; }
    }
}
