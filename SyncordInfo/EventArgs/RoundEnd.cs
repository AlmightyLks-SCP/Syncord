using ProtoBuf;
using SyncordInfo.Communication;
using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    public sealed class RoundEnd : DataBase
    {
        [ProtoMember(4)]
        public SimpleRoundSummary RoundSummary { get; set; }
    }
}
