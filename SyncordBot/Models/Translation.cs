using System.Collections.Generic;

namespace SyncordBot.Models
{
    public class Translation
    {
        public string Language { get; set; }
        public Dictionary<string, string> Elements { get; set; }
        public Translation()
        {
            Language = "Unknown";
            Elements = new Dictionary<string, string>();
        }
    }
}
