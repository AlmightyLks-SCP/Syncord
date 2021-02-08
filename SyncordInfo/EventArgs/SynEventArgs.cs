using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    [ProtoInclude(1001, typeof(PlayerJoinLeave))]
    [ProtoInclude(1002, typeof(RoundEnd))]
    [ProtoInclude(1003, typeof(PlayerDeath))]
    [ProtoInclude(1004, typeof(PlayerBan))]
    public class SynEventArgs
    {
        [ProtoMember(1)]
        public bool SameMachine { get; set; }
        [ProtoMember(2)]
        public string SLFullAddress { get; set; }
        [ProtoMember(3)]
        public DateTime Time { get; set; }
    }
}
