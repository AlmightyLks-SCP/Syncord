using ProtoBuf;

namespace SyncordInfo.Communication
{
    [ProtoContract]
    public class Response
    {
        [ProtoMember(1)]
        public int ServerPort { get; set; }
        [ProtoMember(2)]
        public Query Query { get; set; }
        [ProtoMember(3)]
        public string Content { get; set; }
    }
}
