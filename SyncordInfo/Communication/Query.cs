using System;
using ProtoBuf;

namespace SyncordInfo.Communication
{
    [ProtoContract]
    public class Query
    {
        [ProtoMember(1)]
        public int ServerPort { get; set; }
        [ProtoMember(2)]
        public QueryType QueryType { get; set; }
    }
    [ProtoContract]
    public enum QueryType
    {
        PlayerCount
    }
}
