using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace SyncordBot.EventHandlers
{
    internal class MessageHandler
    {
        public Bot Bot { private get; set; }
        internal static void Init(DiscordClient discord)
        {
            //discord.MessageAcknowledged += OnMessageAcknowledged;
            //discord.MessagesBulkDeleted += OnMessagesBulkDeleted;
            //discord.MessageUpdated += OnMessageUpdated;
        }

        private Task OnMessageAcknowledged(MessageAcknowledgeEventArgs e)
        {
            Console.WriteLine("Message Acknowledged");
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
