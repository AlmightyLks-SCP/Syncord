using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace SyncordBot.Handlers
{
    public class MessageHandler
    {
        public Bot Bot { private get; set; }
        public static void Init(DiscordClient discord)
        {
            //discord.MessageAcknowledged += OnMessageAcknowledged;
            //discord.MessagesBulkDeleted += OnMessagesBulkDeleted;
            //discord.MessageUpdated += OnMessageUpdated;
        }

        private Task OnMessageAcknowledged(MessageAcknowledgeEventArgs e)
        {
            return Task.CompletedTask;
        }
        private Task OnMessagesBulkDeleted(MessageBulkDeleteEventArgs e)
        {
            return Task.CompletedTask;
        }
        private Task OnMessageUpdated(MessageUpdateEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
