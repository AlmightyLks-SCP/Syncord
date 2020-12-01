using System;

namespace SyncordInfo
{
    [Serializable]
    public class SharedInfo
    {
        public int Port { get; set; }
        public RequestType RequestType { get; set; }
        public string Content { get; set; }
    }
}
