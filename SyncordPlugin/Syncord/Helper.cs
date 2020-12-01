using DSharpPlus.Entities;
using Synapse;
using Synapse.Api.Events.SynapseEventArguments;
using System;
using System.Linq;

namespace SyncordPlugin.Syncord
{
    internal static class Helper
    {
        internal static DiscordEmbedBuilder ToDiscordEmbedBuilder(this Synapse.Api.Events.EventHandler.ISynapseEventArgs e)
        {
            DiscordEmbedBuilder embedBuilder = DefaultEmbedBuilder();

            if (e is PlayerDeathEventArgs)
                embedBuilder = (e as PlayerDeathEventArgs).ToEmbedBuilder();
            else if (e is PlayerJoinEventArgs)
                embedBuilder = (e as PlayerJoinEventArgs).ToEmbedBuilder();
            else if (e is SpawnPlayersEventArgs)
                embedBuilder = (e as SpawnPlayersEventArgs).ToEmbedBuilder();
            else if (e is PlayerLeaveEventArgs)
                embedBuilder = (e as PlayerLeaveEventArgs).ToEmbedBuilder();
            else if (e is ConsoleCommandEventArgs)
                embedBuilder = (e as ConsoleCommandEventArgs).ToEmbedBuilder();
            else if (e is RemoteAdminCommandEventArgs)
                embedBuilder = (e as RemoteAdminCommandEventArgs).ToEmbedBuilder();
            else if (e is PlayerBanEventArgs)
                embedBuilder = (e as PlayerBanEventArgs).ToEmbedBuilder();

            return embedBuilder;
        }

        private static DiscordEmbedBuilder DefaultEmbedBuilder()
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Error";
            embedBuilder.Color = DiscordColor.DarkRed;

            embedBuilder.WithFooter(Server.Get.Port.ToString());
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }

        private static DiscordEmbedBuilder ToEmbedBuilder(this PlayerDeathEventArgs ev)
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

            embedBuilder.WithFooter($"Server: {Server.Get.Port}");
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }
        private static DiscordEmbedBuilder ToEmbedBuilder(this PlayerJoinEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Join";
            embedBuilder.Color = DiscordColor.Green;

            embedBuilder.AddField(ev.Nickname, $"{ev.Player.UserId}\n{ev.Player.Ping} ms\n{(ev.Player.DoNotTrack ? "Do Not Track" : ev.Player.IpAddress)}", true);

            embedBuilder.WithFooter($"Server: {Server.Get.Port}");
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }
        private static DiscordEmbedBuilder ToEmbedBuilder(this PlayerLeaveEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Leave";
            embedBuilder.Color = DiscordColor.Red;

            embedBuilder.AddField(ev.Player.NickName, $"{ev.Player.UserId}\n{ev.Player.Ping} ms\n{(ev.Player.DoNotTrack ? "Do Not Track" : ev.Player.IpAddress)}", true);

            embedBuilder.WithFooter($"Server: {Server.Get.Port}");
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }
        private static DiscordEmbedBuilder ToEmbedBuilder(this SpawnPlayersEventArgs ev)
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

            embedBuilder.WithFooter($"Server: {Server.Get.Port}");
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }
        private static DiscordEmbedBuilder ToEmbedBuilder(this ConsoleCommandEventArgs ev)
        {
            if (ev.Command.ToLower().StartsWith(".key")) 
                return null;
            
            var embedBuilder = new DiscordEmbedBuilder();
            
            embedBuilder.Title = "Console Command";
            embedBuilder.Color = DiscordColor.Blue;

            embedBuilder.AddField(ev.Player.NickName, $"Command issued: {ev.Command}", true);

            embedBuilder.WithFooter($"Server: {Server.Get.Port}");
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }
        private static DiscordEmbedBuilder ToEmbedBuilder(this RemoteAdminCommandEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Remote Admin Command";
            embedBuilder.Color = DiscordColor.Blue;

            embedBuilder.AddField(ev.Sender.Nickname, $"Command issued: {ev.Command}", true);

            embedBuilder.WithFooter($"Server: {Server.Get.Port}");
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }
        private static DiscordEmbedBuilder ToEmbedBuilder(this PlayerBanEventArgs ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Ban";
            embedBuilder.Color = DiscordColor.DarkRed;

            embedBuilder.AddField($"{ev.Issuer.NickName} ({ev.Issuer.UserId})", 
                $"Banned: {ev.BannedPlayer.NickName}\n{ev.BannedPlayer.UserId}\nReason: {ev.Reason}\n" +
                $"Duration: {ev.Duration / 60} Minutes | {ev.Duration / 60 / 60} Hours | {ev.Duration / 60 / 60 / 24} Days | {ev.Duration / 60 / 60 / 24 / 365} Years",
                true);

            embedBuilder.WithFooter($"Server: {Server.Get.Port}");
            embedBuilder.Timestamp = DateTime.UtcNow;

            return embedBuilder;
        }
    }
}
