using Synapse.Command;
using SyncordPlugin.Syncord;
using System;
using System.Linq;

namespace SyncordPlugin.Command
{
    [CommandInformation(
        Name = "Syncord",
        Aliases = new string[] { "syn" },
        Description = "Reconnect your Server with the configured Discord Bot",
        Permission = "syncord.reconnect",
        Platforms = new Platform[] { Platform.RemoteAdmin, Platform.ServerConsole },
        Usage = "syncord connect"
        )]
    public class Reconnect : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();

            if (!context.Player.HasPermission("syncord.reconnect"))
            {
                result.Message = "You don't have Permission to execute this Command (syncord.reconnect)";
                result.State = CommandResultState.NoPermission;
                return result;
            }

            if (context.Arguments.Count != 1)
            {
                result.Message = "Incorrect usage. Please use 'syncord connect'";
                result.State = CommandResultState.Error;
                return result;
            }

            var conRes = SyncordBehaviour.ConnectClient();

            result.Message = conRes.ToString();
            result.State = conRes == SendStatus.Successful ? CommandResultState.Ok : CommandResultState.Error;

            return result;
        }
    }
}
