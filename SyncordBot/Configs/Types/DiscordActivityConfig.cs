using DSharpPlus.Entities;

namespace SyncordBot.Configs.Types
{
    public struct DiscordActivityConfig
    {
        public string Name { get; set; }
        public ActivityType Activity { get; set; }
    }
}
