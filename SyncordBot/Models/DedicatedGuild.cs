using Newtonsoft.Json;
using System.Collections.Generic;

namespace SyncordBot.Models
{
    public class DedicatedGuild
    {
        public ulong GuildID { get; set; } = 0;
        [JsonProperty("Full SL Address")]
        public string SLFullAddress { get; set; } = "";
        [JsonProperty("Dedicated Channels")]
        public Dictionary<EventTypes, ulong> DedicatedChannels { get; set; } = new Dictionary<EventTypes, ulong>();
    }
}
