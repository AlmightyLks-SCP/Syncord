using ProtoBuf;

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public class SimpleRoundSummary
    {
        [ProtoMember(1)]
        public int RoundTime { get; set; }
        [ProtoMember(2)]
        public int TurnedIntoZombies { get; set; }
        [ProtoMember(3)]
        public int TotalKills { get; set; }
        [ProtoMember(4)]
        public int TotalKillsByScps { get; set; }
        [ProtoMember(5)]
        public int TotalKillsByFragGrenade { get; set; }
        [ProtoMember(6)]
        public int TotalEscapedDClass { get; set; }
        [ProtoMember(7)]
        public int TotalEscapedScientists { get; set; }
    }
}
