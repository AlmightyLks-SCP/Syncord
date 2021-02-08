using ProtoBuf;

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public struct SimpleHitInfo
    {
        [ProtoMember(1)]
        public int Tool { get; set; }
        [ProtoMember(2)]
        public int Time { get; set; }
        [ProtoMember(3)]
        public string Attacker { get; set; }
        [ProtoMember(4)]
        public float Amount { get; set; }
        [ProtoMember(5)]
        public SimpleDamageType DamageType { get; set; }
    }
}
