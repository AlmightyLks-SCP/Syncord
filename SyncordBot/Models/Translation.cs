using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
