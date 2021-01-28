using ProtoBuf;

namespace SyncordInfo.Communication
{
    [ProtoContract]
    public class Request
    {
        [ProtoMember(1)]
        public int ServerPort { get; set; }
        [ProtoMember(2)]
        public RequestType RequestType { get; set; }
    }

    [ProtoContract]
    public enum RequestType
    {
        Event,
        Connect,
        Query,
        Response
    }
}
