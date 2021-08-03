using System.Collections.Generic;

namespace SyncordInfo.SimplifiedTypes
{
    public sealed class SimpleSynapseGroup
    {
        public bool Default { get; set; }
        public bool Northwood { get; set; }
        public bool RemoteAdmin { get; set; }
        public string Badge { get; set; }
        public string Color { get; set; }
        public bool Cover { get; set; }
        public bool Hidden { get; set; }
        public byte KickPower { get; set; }
        public byte RequiredKickPower { get; set; }
        public List<string> Permissions { get; set; }
        public List<string> Members { get; set; }
    }
}
