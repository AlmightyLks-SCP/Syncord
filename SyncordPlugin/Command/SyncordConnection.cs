using Synapse.Command;
using SyncordPlugin.Syncord;

namespace SyncordPlugin.Command
{
    [CommandInformation(
        Name = "Syncord",
        Aliases = new string[] { "syn" },
        Description = "Reconnect your Server with the configured Discord Bot",
        Permission = "syncord.reconnect",
        Platforms = new Platform[] { Platform.RemoteAdmin, Platform.ServerConsole },
        Usage = "syncord connect / syncord disconnect"
        )]
    public class SyncordConnection : ISynapseCommand
    {
        public CommandResult Execute(CommandContext context)
        {
            var result = new CommandResult();

            if (context.Arguments.Count == 0)
            {
                result.Message = $"Syncord Connected: {CommunicationHandler.Singleton.EasyClient.ClientConnected}";
                result.State = CommandResultState.Ok;
                return result;
            }
            if (context.Arguments.Count > 1)
            {
                result.Message = $"Incorrect Usage";
                result.State = CommandResultState.Error;
                return result;
            }

            switch (context.Arguments.At(0))
            {
                case "connect":
                    {
                        if (CommunicationHandler.Singleton.EasyClient.ClientConnected)
                        {
                            result.Message = "Client already connected";
                            result.State = CommandResultState.Error;
                        }
                        else
                        {
                            var conRes = CommunicationHandler.ConnectToHost();
                            SynapseController.Server.Logger.Warn((conRes && CommunicationHandler.Singleton.EasyClient.ClientConnected).ToString());
                            result.Message = (conRes && CommunicationHandler.Singleton.EasyClient.ClientConnected) ? "Connection established" : $"Connection failed. Is the entered IP & Port ({SyncordPlugin.Config.DiscordBotAddress}:{SyncordPlugin.Config.DiscordBotPort}) valid?";
                            result.State = (conRes && CommunicationHandler.Singleton.EasyClient.ClientConnected) ? CommandResultState.Ok : CommandResultState.Error;
                        }
                    }
                    break;
                case "disconnect":
                    {
                        if (!CommunicationHandler.Singleton.EasyClient.ClientConnected)
                        {
                            result.Message = "Client already disconnected";
                            result.State = CommandResultState.Error;
                        }
                        else
                        {
                            CommunicationHandler.DisconnectFromHost();
                            result.Message = "Disconnected ";
                            result.State = CommandResultState.Ok;
                        }
                    }
                    break;
            }

            return result;
        }
    }
}
