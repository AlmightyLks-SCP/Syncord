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
        public static DiscordEmbed ToEmbed(PlayerJoined ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Join";
            embedBuilder.Color = DiscordColor.Green;
            var strBuilder = new StringBuilder();

            if (Bot.Configs.EmbedConfigs.PlayerJoinedConfig.ShowUserId)
                strBuilder.AppendLine(ev.Player.UserId);
            if (Bot.Configs.EmbedConfigs.PlayerJoinedConfig.ShowPing)
                strBuilder.AppendLine($"{ev.Player.Ping} ms");
            if (Bot.Configs.EmbedConfigs.PlayerJoinedConfig.ShowIP)
                strBuilder.AppendLine($"{(ev.Player.DoNotTrack ? "Do Not Track" : ev.Player.IPAddress)}");

            strBuilder.AppendLine(ev.Time.ToShortTimeString());

            embedBuilder.AddField(ev.Player.Nickname,
                strBuilder.ToString(),
                true);

            embedBuilder.WithFooter($"Server: {ev.ServerPort}");
            embedBuilder.Timestamp = ev.Time;

            return embedBuilder.Build();
        }
        public static DiscordEmbed ToEmbed(List<PlayerJoined> ev)
        {
            var embedBuilder = new DiscordEmbedBuilder();

            embedBuilder.Title = "Player Join";
            embedBuilder.Color = DiscordColor.Green;
            foreach (var joinedArgs in ev)
            {
                var strBuilder = new StringBuilder();

                if (Bot.Configs.EmbedConfigs.PlayerJoinedConfig.ShowUserId)
                    strBuilder.AppendLine(joinedArgs.Player.UserId);
                if (Bot.Configs.EmbedConfigs.PlayerJoinedConfig.ShowPing)
                    strBuilder.AppendLine($"{joinedArgs.Player.Ping} ms");
                if (Bot.Configs.EmbedConfigs.PlayerJoinedConfig.ShowIP)
                    strBuilder.AppendLine($"{(joinedArgs.Player.DoNotTrack ? "Do Not Track" : joinedArgs.Player.IPAddress)}");

                strBuilder.AppendLine(joinedArgs.Time.ToShortTimeString());

                embedBuilder.AddField(joinedArgs.Player.Nickname,
                    strBuilder.ToString(),
                    true);
            }

            //embedBuilder.WithFooter($"Server: {ev.ServerPort}");
            //embedBuilder.Timestamp = ev.Time;

            return embedBuilder.Build();
        }
    }
}
