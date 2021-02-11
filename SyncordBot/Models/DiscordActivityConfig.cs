using DSharpPlus.Entities;

namespace SyncordBot.Models
{
    public struct DiscordActivityConfig
    {
        public string Name { get; set; }
        public ActivityType Activity { get; set; }
    }
}
