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
    public sealed class PlayerJoined
    {
        [ProtoMember(1)]
        public int ServerPort { get; set; }
        [ProtoMember(2)]
        public SimplePlayer Player { get; set; }
        [ProtoMember(3)]
        public DateTime Time { get; set; }
    }
}
