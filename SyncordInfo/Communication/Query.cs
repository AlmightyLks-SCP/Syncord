using ProtoBuf;

namespace SyncordInfo.Communication
{
    [ProtoContract]
    public sealed class Query
    {
        [ProtoMember(1)]
        public QueryType QueryType { get; set; }
    }
    [ProtoContract]
    public enum QueryType
    {
        PlayerCount,
        PlayerDeaths,
        ServerFps
    }
}
