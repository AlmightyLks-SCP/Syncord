using System;

namespace SyncordInfo
{
    [Serializable]
    public enum RequestType
    {
        Event,
        Heartbeat,
        Connect,
        Query,
        Response
    }
}
