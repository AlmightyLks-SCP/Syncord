using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.Configs.EmbedConfigs
{
    public struct RoundEndConfig
    {
        public bool ShowRoundLength { get; set; }
        public bool ShowTotalKills { get; set; }
        public bool ShowTotalScpKills { get; set; }
        public bool ShowTotalFragGrenadeKills { get; set; }
        public bool ShowTotalEscapedDClass { get; set; }
        public bool ShowTotalEscapedScientists { get; set; }
    }
}
