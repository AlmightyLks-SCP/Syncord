using ProtoBuf;
using SyncordInfo.EventArgs;
using System;

namespace SyncordInfo.Communication
{
    [ProtoContract]
    [ProtoInclude(1001, typeof(PlayerJoinLeave))]
    [ProtoInclude(1002, typeof(RoundEnd))]
    [ProtoInclude(1003, typeof(PlayerDeath))]
    [ProtoInclude(1004, typeof(PlayerBan))]
    [ProtoInclude(1005, typeof(Response))]
    [ProtoInclude(1006, typeof(Ping))]
    public class DataBase
    {
        [ProtoMember(1)]
        public bool SameMachine { get; set; }
        [ProtoMember(2)]
        public string SLFullAddress { get; set; }
        [ProtoMember(3)]
        public DateTime Time { get; set; }
    }
}
