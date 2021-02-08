using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public sealed class SimpleSynapseGroup
    {
        [ProtoMember(1)]
        public bool Default { get; set; }
        [ProtoMember(2)]
        public bool Northwood { get; set; }
        [ProtoMember(3)]
        public bool RemoteAdmin { get; set; }
        [ProtoMember(4)]
        public string Badge { get; set; }
        [ProtoMember(5)]
        public string Color { get; set; }
        [ProtoMember(6)]
        public bool Cover { get; set; }
        [ProtoMember(7)]
        public bool Hidden { get; set; }
        [ProtoMember(8)]
        public byte KickPower { get; set; }
        [ProtoMember(9)]
        public byte RequiredKickPower { get; set; }
        [ProtoMember(10)]
        public List<string> Permissions { get; set; }
        [ProtoMember(11)]
        public List<string> Members { get; set; }
    }
}
