using DSharpPlus.Entities;
using Synapse;
using Synapse.Api.Events.SynapseEventArguments;
using System;
using System.Linq;

namespace SyncordPlugin.Syncord
{
    public static class Helper
    {
        public static DSharpPlus.Entities.DiscordEmbed ToDiscordEmbed(this Synapse.Api.Events.EventHandler.ISynapseEventArgs e)
        {
            DSharpPlus.Entities.DiscordEmbed embed = DefaultEmbed();

            if (e is PlayerDeathEventArgs)
                embed = (e as PlayerDeathEventArgs).ToEmbed();
            else if (e is PlayerJoinEventArgs)
                embed = (e as PlayerJoinEventArgs).ToEmbed();
            else if (e is SpawnPlayersEventArgs)
                embed = (e as SpawnPlayersEventArgs).ToEmbed();
            else if (e is PlayerLeaveEventArgs)
                embed = (e as PlayerLeaveEventArgs).ToEmbed();
            else if (e is ConsoleCommandEventArgs)
                embed = (e as ConsoleCommandEventArgs).ToEmbed();
            else if (e is RemoteAdminCommandEventArgs)
                embed = (e as RemoteAdminCommandEventArgs).ToEmbed();

            return embed;
        }

        private static DSharpPlus.Entities.DiscordEmbed DefaultEmbed()
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Error";
            embedBuilder.Color = DiscordColor.DarkRed;

            return embedBuilder.Build();
        }

        private static DSharpPlus.Entities.DiscordEmbed ToEmbed(this PlayerDeathEventArgs ev)
        {
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
            embedBuilder.Color = DiscordColor.DarkRed;

            embedBuilder.AddField($"Killer", $"{killerRole}\n{ev.Killer.DisplayName}\n{ev.Killer.UserId}", true);
            embedBuilder.AddField($"Victim:", $"{victimRole}\n{ev.Victim.DisplayName}\n{ev.Victim.UserId}\nWas {(ev.Victim.IsCuffed ? "" : "not ")}Cuffed", true);
            embedBuilder.AddField($"Weapon:", $"{damageType.name}", true);

            return embedBuilder.Build();
        }
        private static DSharpPlus.Entities.DiscordEmbed ToEmbed(this PlayerJoinEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Join";
            embedBuilder.Color = DiscordColor.Green;

            embedBuilder.AddField(ev.Nickname, $"{ev.Player.UserId}\n{ev.Player.Ping} ms\n{(ev.Player.DoNotTrack ? "Do Not Track" : ev.Player.IpAddress)}", true);

            return embedBuilder.Build();
        }
        private static DSharpPlus.Entities.DiscordEmbed ToEmbed(this SpawnPlayersEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Round Start Spawn";
            embedBuilder.Color = DiscordColor.Blue;
            var roleIds = ev.SpawnPlayers.Values.Distinct();

            foreach (var roleid in roleIds)
            {
                string roleName = string.Empty;

                if (roleid < -1 || roleid > 17)
                    roleName = Server.Get.RoleManager.CustomRoles.Values.First(x => x.Value == roleid).Key;
                else
                    roleName = ((RoleType)roleid).ToString();

                embedBuilder.AddField(roleName, $"Amount: {ev.SpawnPlayers.Values.Where((_) => _ == roleid).Count()}", true);
            }
            return embedBuilder.Build();
        }
        private static DSharpPlus.Entities.DiscordEmbed ToEmbed(this PlayerLeaveEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Leave";
            embedBuilder.Color = DiscordColor.Red;

            embedBuilder.AddField(ev.Player.NickName, $"{ev.Player.UserId}\n{ev.Player.Ping} ms\n{(ev.Player.DoNotTrack ? "Do Not Track" : ev.Player.IpAddress)}", true);

            return embedBuilder.Build();
        }
        private static DSharpPlus.Entities.DiscordEmbed ToEmbed(this ConsoleCommandEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Console Command";
            embedBuilder.Color = DiscordColor.Blue;

            embedBuilder.AddField(ev.Player.NickName, $"Command issued: {ev.Command}", true);

            return embedBuilder.Build();
        }
        private static DSharpPlus.Entities.DiscordEmbed ToEmbed(this RemoteAdminCommandEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Remote Admin Command";
            embedBuilder.Color = DiscordColor.Blue;

            embedBuilder.AddField(ev.Sender.Nickname, $"Command issued: {ev.Command}", true);

            return embedBuilder.Build();
        }
    }
}
