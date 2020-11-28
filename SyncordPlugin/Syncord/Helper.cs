using DSharpPlus.Entities;
using Synapse;
using Synapse.Api.Events.SynapseEventArguments;
using System.Linq;

namespace SyncordPlugin.Syncord
{
    public static class Helper
    {
        public static DSharpPlus.Entities.DiscordEmbed ToDiscordEmbed(this PlayerDeathEventArgs ev)
        {
            SynapseController.Server.Logger.Info("In ToDiscordEmbed");
            var embedBuilder = new DiscordEmbedBuilder();
            var damageType = ev.HitInfo.GetDamageType();

            string victimRole = string.Empty;
            string killerRole = string.Empty;

            if (ev.Victim.RoleID < -1 || ev.Victim.RoleID > 17)
                victimRole = Server.Get.RoleManager.CustomRoles.Values.First(x => x.Value == ev.Victim.RoleID).Key;
            else
                victimRole = ev.Victim.RoleType.ToString();

            if (ev.Victim.RoleID < -1 || ev.Victim.RoleID > 17)
                killerRole = Server.Get.RoleManager.CustomRoles.Values.First(x => x.Value == ev.Killer.RoleID).Key;
            else
                killerRole = ev.Killer.RoleType.ToString();

            embedBuilder.Title = "Player Death";
            embedBuilder.Color = DiscordColor.Blue;

            embedBuilder.AddField($"Killer", $"{killerRole}\n{ev.Killer.DisplayName}\n{ev.Killer.UserId}", true);
            embedBuilder.AddField($"Victim:", $"{victimRole}\n{ev.Victim.DisplayName}\n{ev.Victim.UserId}\nWas {(ev.Victim.IsCuffed ? "" : "not ")}Cuffed", true);
            embedBuilder.AddField($"Weapon:", $"{damageType.name}", true);

            return embedBuilder.Build();
        }
    }
}
