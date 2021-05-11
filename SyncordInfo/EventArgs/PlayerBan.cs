using ProtoBuf;
using SyncordInfo.Communication;
using SyncordInfo.SimplifiedTypes;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    public sealed class PlayerBan : DataBase
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
