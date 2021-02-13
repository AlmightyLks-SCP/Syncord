using ProtoBuf;
using System;

namespace SyncordInfo.Communication
{
    [ProtoContract]
    public class Ping : DataBase
    {
        [ProtoMember(4)]
        public DateTime Sent { get; set; }
        [ProtoMember(5)]
        public DateTime Received { get; set; }
    }
}
