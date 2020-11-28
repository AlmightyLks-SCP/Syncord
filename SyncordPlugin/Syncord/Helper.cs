using DSharpPlus.Entities;
using Synapse.Api.Events.SynapseEventArguments;
using Utf8Json;

namespace SyncordPlugin.Syncord
{
    public static class Helper
    {
        public static DSharpPlus.Entities.DiscordEmbed ToDiscordEmbed(this PlayerDeathEventArgs ev)
        {
            SynapseController.Server.Logger.Info("In ToDiscordEmbed");
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Death";
            embedBuilder.Color = DiscordColor.Blue;
            embedBuilder.AddField($"Killer", $"{ev.Killer.DisplayName}\n{ev.Killer.UserId}", true);
            embedBuilder.AddField($"Victim:", $"{ev.Victim.DisplayName}\n{ev.Victim.UserId}", true);

            return embedBuilder.Build();
        }
    }
}
