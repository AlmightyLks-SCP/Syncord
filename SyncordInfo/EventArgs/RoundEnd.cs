using SyncordInfo.Communication;
using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.EventArgs
{
    public sealed class RoundEnd : DataBase
    {
        public SimpleRoundSummary RoundSummary { get; set; }
    }
}
