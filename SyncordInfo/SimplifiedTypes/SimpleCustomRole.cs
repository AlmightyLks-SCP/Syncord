using ProtoBuf;

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public sealed class SimpleCustomRole
    {
        [ProtoMember(1)]
        public (string Name, int Id) Role { get; set; }
        [ProtoMember(2)]
        public (string Name, int Id) Team { get; set; }
        public SimpleCustomRole()
        {
            Role = (string.Empty, 0);
            Team = (string.Empty, 0);
        }
    }
}
