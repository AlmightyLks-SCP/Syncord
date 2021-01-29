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
        public string ServerAddress{ get; set; }
        [ProtoMember(2)]
        public int ServerPort { get; set; }
        [ProtoMember(3)]
        public SimplePlayer Player { get; set; }
        [ProtoMember(4)]
        public DateTime Time { get; set; }
    }
}
