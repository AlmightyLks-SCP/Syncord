using DSharpPlus.Entities;
using MEC;
using Synapse;
using Synapse.Api.Events.SynapseEventArguments;
using System;
using System.Collections.Generic;
using System.Linq;
using static Synapse.Api.Events.EventHandler;

namespace SyncordPlugin.Syncord
{
    internal class SpamQueue
    {
        private Queue<(ISynapseEventArgs Context, DateTime Time)> QueuedRequests;
        private DiscordEmbedBuilder embedBuilder;

        internal SpamQueue()
        {
            QueuedRequests = new Queue<(ISynapseEventArgs Context, DateTime Time)>();
            embedBuilder = new DiscordEmbedBuilder();

            //Timing.RunCoroutine(ProcessQueue());
        }

        internal void AddToQueue(PlayerJoinEventArgs e)
            => QueuedRequests.Enqueue((e, DateTime.UtcNow));
        internal void AddToQueue(PlayerLeaveEventArgs e)
            => QueuedRequests.Enqueue((e, DateTime.UtcNow));

        private IEnumerator<float> ProcessQueue()
        {
            for (; ; )
            {
                embedBuilder = new DiscordEmbedBuilder();
                int count = 5;

                if(QueuedRequests.Count < 5)
                    count = QueuedRequests.Count;

                for (int i = 0; i < count; i++)
                {
                    var cur = QueuedRequests.Dequeue();

                    if (cur.Context is PlayerJoinEventArgs playerJoin)
                    {
                        embedBuilder.Title = "Player Join";
                        embedBuilder.Color = DiscordColor.Green;

                        embedBuilder.AddField(playerJoin.Nickname + " joined", $"{playerJoin.Player.UserId}\n{playerJoin.Player.Ping} ms\n{(playerJoin.Player.DoNotTrack ? "Do Not Track" : playerJoin.Player.IpAddress)}", true);

                        embedBuilder.WithFooter(Server.Get.Port.ToString());
                        embedBuilder.Timestamp = DateTime.UtcNow;
                    }
                    else if(cur.Context is PlayerJoinEventArgs playerLeave)
                    {
                        embedBuilder.Title = "Player Leave";
                        embedBuilder.Color = DiscordColor.Red;

                        embedBuilder.AddField(playerLeave.Player.NickName + " left", $"{playerLeave.Player.UserId}\n{playerLeave.Player.Ping} ms\n{(playerLeave.Player.DoNotTrack ? "Do Not Track" : playerLeave.Player.IpAddress)}", true);

                        embedBuilder.WithFooter(Server.Get.Port.ToString());
                        embedBuilder.Timestamp = DateTime.UtcNow;
                    }
                }
                yield return Timing.WaitForSeconds(1.0f);
            }
        }
    }
}
