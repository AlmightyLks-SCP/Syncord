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
        public bool SameMachine { get; set; }
        [ProtoMember(2)]
        public string SLFullAddress { get; set; }
        [ProtoMember(3)]
        public SimplePlayer Reporter { get; set; }
        [ProtoMember(4)]
        public SimplePlayer Target { get; set; }
        [ProtoMember(5)]
        public string Reason { get; set; }
        [ProtoMember(6)]
        public bool GlobalReport { get; set; }
        [ProtoMember(7)]
        public bool Allow { get; set; }
        [ProtoMember(8)]
        public DateTime Time { get; set; }
    }
}
