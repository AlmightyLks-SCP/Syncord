using DSharpPlus;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.EventHandlers
{
    internal static class MessageHandler
    {
        internal static void Init(DiscordClient discord)
        {
            discord.MessageCreated += OnMessageCreated;
            discord.MessageAcknowledged += OnMessageAcknowledged;
            discord.MessagesBulkDeleted += OnMessagesBulkDeleted;
            discord.MessageUpdated += OnMessageUpdated;
        }

        //Message Handling
        private static Task OnMessageCreated(MessageCreateEventArgs e)
        {
            if (e.Message.Author.IsBot)
                return Task.CompletedTask;

            if (e.Message.Content.Substring(0, Bot.Configs.Prefix.Length) != Bot.Configs.Prefix)
                return Task.CompletedTask;

            return Task.CompletedTask;
        }



        private static Task OnMessageAcknowledged(MessageAcknowledgeEventArgs e)
        {
            Console.WriteLine("Message Acknowledged");
            return Task.CompletedTask;
        }
        private static Task OnMessagesBulkDeleted(MessageBulkDeleteEventArgs e)
        {
            return Task.CompletedTask;
        }
        private static Task OnMessageUpdated(MessageUpdateEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
