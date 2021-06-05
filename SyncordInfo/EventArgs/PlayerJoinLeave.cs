using SyncordInfo.Communication;
using SyncordInfo.SimplifiedTypes;
using System;

namespace SyncordInfo.EventArgs
{
    public sealed class PlayerJoinLeave : DataBase
    {
        public string Identifier { get; set; } // "join" / "leave"
        public SimplePlayer Player { get; set; }
    }
}
