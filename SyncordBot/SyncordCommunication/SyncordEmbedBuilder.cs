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
        public static DiscordEmbed ToEmbed(this PlayerJoinLeave ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = ev.Identifier == "join" ? "Player Join" : "Player Leave";
            embedBuilder.Color = DiscordColor.Green;
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine("```");

            if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowUserId)
                strBuilder.AppendLine(ev.Player.UserId);
            if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowPing)
                strBuilder.AppendLine($"{ev.Player.Ping} ms");
            if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowIP)
                strBuilder.AppendLine($"{(ev.Player.DoNotTrack ? "Do Not Track" : ev.Player.IPAddress)}");

            strBuilder.AppendLine(ev.Time.ToLongTimeString());
            strBuilder.AppendLine("```");

            embedBuilder.AddField(ev.Player.Nickname,
                strBuilder.ToString(),
                true);

            embedBuilder.WithFooter($"Server: {ev.SLFullAddress}");
            embedBuilder.Timestamp = ev.Time;

            return embedBuilder.Build();
        }
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
        
        public static DiscordEmbed ToEmbed(this PlayerReport ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            //embedBuilder.Title = "Player Report";
            //embedBuilder.Color = DiscordColor.Blue;
            //var strBuilder = new StringBuilder();

            //strBuilder.
            //if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowUserId)
            //    strBuilder.AppendLine(ev.Player.UserId);
            //if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowPing)
            //    strBuilder.AppendLine($"{ev.Player.Ping} ms");
            //if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowIP)
            //    strBuilder.AppendLine($"{(ev.Player.DoNotTrack ? "Do Not Track" : ev.Player.IPAddress)}");

            //strBuilder.AppendLine(ev.Time.ToShortTimeString());

            //embedBuilder.AddField(ev.Player.Nickname,
            //    strBuilder.ToString(),
            //    true);

            //embedBuilder.WithFooter($"Server: {ev.SLFullAddress}");
            //embedBuilder.Timestamp = ev.Time;

            return embedBuilder.Build();
        }
        public static DiscordEmbed ToEmbed(this PlayerReport[] ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            //embedBuilder.Title = ev[0].Identifier == "join" ? "Player Join" : "Player Leave";
            //embedBuilder.Color = DiscordColor.Green;
            //foreach (var joinedArgs in ev)
            //{
            //    var strBuilder = new StringBuilder();
            //    strBuilder.AppendLine("```");

            //    if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowUserId)
            //        strBuilder.AppendLine(joinedArgs.Player.UserId);
            //    if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowPing)
            //        strBuilder.AppendLine($"{joinedArgs.Player.Ping} ms");
            //    if (Bot.Configs.EmbedConfigs.PlayerJoinedLeftConfig.ShowIP)
            //        strBuilder.AppendLine($"{(joinedArgs.Player.DoNotTrack ? "Do Not Track" : joinedArgs.Player.IPAddress)}");

            //    strBuilder.AppendLine(joinedArgs.Time.ToLongTimeString());
            //    strBuilder.AppendLine("```");

            //    embedBuilder.AddField(joinedArgs.Player.Nickname,
            //        strBuilder.ToString(),
            //        true);

            //    embedBuilder.WithFooter($"Server: {joinedArgs.SLFullAddress}");
            //}

            return embedBuilder.Build();
        }
    }
}
