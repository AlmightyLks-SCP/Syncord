using System;

namespace SyncordInfo.Communication
{
    public class Ping : DataBase
    {
        public DateTime Sent { get; set; }
        public DateTime Received { get; set; }
    }
}
