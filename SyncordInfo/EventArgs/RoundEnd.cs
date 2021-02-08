using ProtoBuf;
using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    public sealed class RoundEnd : SynEventArgs
    {
        [ProtoMember(4)]
        public SimpleRoundSummary RoundSummary { get; set; }
    }
}
