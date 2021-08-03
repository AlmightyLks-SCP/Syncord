using SyncordInfo.EventArgs;
using System;

namespace SyncordInfo.Communication
{
    public class DataBase
    {
        public bool SameMachine { get; set; }
        public string SLFullAddress { get; set; }
        public DateTime Time { get; set; }
        public MessageType MessageType { get; set; }
    }
}
