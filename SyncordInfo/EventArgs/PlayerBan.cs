using SyncordInfo.Communication;
using SyncordInfo.SimplifiedTypes;

namespace SyncordInfo.EventArgs
{
    public sealed class PlayerBan : DataBase
    {
        public SimplePlayer BannedPlayer { get; set; }
        public SimplePlayer BanningPlayer { get; set; }
        public int Duration { get; set; }
        public string Reason { get; set; }
    }
}
