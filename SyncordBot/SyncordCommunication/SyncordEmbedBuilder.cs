using DSharpPlus.Entities;
using SyncordInfo.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.SyncordCommunication
{
    public static class SyncordEmbedBuilder
    {
        public static DiscordEmbed ToEmbed(this PlayerJoinLeave[] ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = ev[0].Identifier == "join" ? "Player Join" : "Player Leave";
            embedBuilder.Color = DiscordColor.Green;
            foreach (var joinedArgs in ev)
            {
                var strBuilder = new StringBuilder();
                strBuilder.AppendLine("```");

                if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowUserId)
                    strBuilder.AppendLine(joinedArgs.Player.UserId);
                if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowPing)
                    strBuilder.AppendLine($"{joinedArgs.Player.Ping} ms");
                if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowIP)
                    strBuilder.AppendLine($"{(joinedArgs.Player.DoNotTrack ? "Do Not Track" : joinedArgs.Player.IPAddress)}");

                strBuilder.AppendLine(joinedArgs.Time.ToLongTimeString());
                strBuilder.AppendLine("```");

                embedBuilder.AddField(joinedArgs.Player.Nickname,
                    strBuilder.ToString(),
                    true);

                embedBuilder.WithFooter($"Server: {joinedArgs.SLFullAddress}");
            }

            return embedBuilder.Build();
        }

        public static DiscordEmbed ToEmbed(this PlayerDeath[] ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Death";
            embedBuilder.Color = DiscordColor.DarkRed;
            foreach (var playerDeath in ev)
            {
                var damageType = playerDeath.HitInfo.DamageType;

                string victimRole = playerDeath.Victim.Role.Role.Name;
                string killerRole = playerDeath.Killer.Role.Role.Name;

                if (Bot.Configs.EmbedConfigs.PlayerDeathConfig.ShowUserId)
                {
                    embedBuilder.AddField($"Killer",
                        $"{killerRole}\n{playerDeath.Killer.DisplayName}\n{playerDeath.Killer.UserId}",
                        true);
                    embedBuilder.AddField($"Victim:",
                        $"{victimRole}\n{playerDeath.Victim.DisplayName}\n{playerDeath.Victim.UserId}\nWas {(playerDeath.Victim.IsCuffed ? "" : "not ")}Cuffed"
                        , true);
                }
                else
                {
                    embedBuilder.AddField($"Killer",
                        $"{killerRole}\n{playerDeath.Killer.DisplayName}",
                        true);
                    embedBuilder.AddField($"Victim:",
                        $"{victimRole}\n{playerDeath.Victim.DisplayName}\nWas {(playerDeath.Victim.IsCuffed ? "" : "not ")}Cuffed"
                        , true);
                }
                embedBuilder.AddField($"Weapon:",
                    $"{damageType.Name}",
                    true);

                embedBuilder.WithFooter($"Server: {playerDeath.SLFullAddress}");
                embedBuilder.Timestamp = playerDeath.Time;
            }

            return embedBuilder.Build();
        }

        public static DiscordEmbed ToEmbed(this RoundEnd ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Round Summary";
            embedBuilder.Color = DiscordColor.Gold;

            if (Bot.Configs.EmbedConfigs.RoundEndConfig.ShowRoundLength)
                embedBuilder.AddField("Round Length",
                    TimeSpan.FromSeconds(ev.RoundSummary.RoundTime).ToString(),
                    false);
            if (Bot.Configs.EmbedConfigs.RoundEndConfig.ShowTotalKills)
                embedBuilder.AddField("Total Kills",
                ev.RoundSummary.TotalKills.ToString(),
                true);
            if (Bot.Configs.EmbedConfigs.RoundEndConfig.ShowTotalScpKills)
                embedBuilder.AddField("Kills By SCPs",
                ev.RoundSummary.TotalKillsByScps.ToString(),
                true);
            if (Bot.Configs.EmbedConfigs.RoundEndConfig.ShowTotalFragGrenadeKills)
                embedBuilder.AddField("Kills By Frag Grenades",
                ev.RoundSummary.TotalKillsByFragGrenade.ToString(),
                true);

            if (Bot.Configs.EmbedConfigs.RoundEndConfig.ShowTotalEscapedDClass)
                embedBuilder.AddField("Escaped D-Class",
                ev.RoundSummary.TotalEscapedDClass.ToString(),
                true);
            if (Bot.Configs.EmbedConfigs.RoundEndConfig.ShowTotalEscapedScientists)
                embedBuilder.AddField("Escaped Scientists",
                ev.RoundSummary.TotalEscapedScientists.ToString(),
                true);

            embedBuilder.WithFooter($"Server: {ev.SLFullAddress}");
            embedBuilder.Timestamp = ev.Time;

            return embedBuilder.Build();
        }

        public static DiscordEmbed ToEmbed(this PlayerBan[] ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Ban";
            embedBuilder.Color = DiscordColor.DarkRed;
            foreach (var playerBan in ev)
            {
                embedBuilder.AddField($"{playerBan.BanningPlayer.Nickname} ({playerBan.BanningPlayer.UserId})",
                    $"Banned: {playerBan.BannedPlayer.Nickname}\n{playerBan.BannedPlayer.UserId}\nReason: {playerBan.Reason}\n" +
                    $"Duration: {playerBan.Duration / 60} Minutes | " +
                    $"{playerBan.Duration / 60 / 60} Hours | " +
                    $"{playerBan.Duration / 60 / 60 / 24} Days | " +
                    $"{playerBan.Duration / 60 / 60 / 24 / 365} Years",
                    true);

                embedBuilder.WithFooter($"Server: {playerBan.SLFullAddress}");
                embedBuilder.Timestamp = playerBan.Time;
            }


            return embedBuilder.Build();
        }
    }
}
