using ProtoBuf;

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public struct SimpleDamageType
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        [ProtoMember(2)]
        public bool IsWeapon { get; set; }
        [ProtoMember(3)]
        public bool IsScp { get; set; }
        [ProtoMember(4)]
        public int WeaponId { get; set; }
    }
}
