using ProtoBuf;
using SyncordInfo.Communication;
using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    public sealed class PlayerJoinLeave : DataBase
    {
        [ProtoMember(4)]
        public string Identifier { get; set; } // "join" / "leave"
        [ProtoMember(5)]
        public SimplePlayer Player { get; set; }
    }
}
