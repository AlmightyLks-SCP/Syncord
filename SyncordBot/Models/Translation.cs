using System.Collections.Generic;

namespace SyncordBot.Models
{
    public class Translation
    {
        public string Language { get; init; }
        public Dictionary<string, string> Elements { get; init; }
        public Translation()
        {
            Language = "Unknown";
            Elements = new Dictionary<string, string>();
        }
    }
}
