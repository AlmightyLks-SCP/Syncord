using Synapse.Command;
using SyncordPlugin.Syncord;
using System;

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
            try
            {
                if (context.Arguments == null || context.Arguments.Count > 1)
                {
                    result.Message = $"Incorrect usage.";
                    result.State = CommandResultState.Error;
                    return result;
                }
                if (context.Arguments.Count == 0 || context.Arguments.At(0).ToLower() != "connect" && context.Arguments.At(0).ToLower() != "disconnect")
                {
                    result.Message = $"Syncord Connected: {CommunicationHandler.Singleton.EasyClient.ClientConnected}";
                    result.State = CommandResultState.Ok;
                    return result;
                }

                switch (context.Arguments.At(0).ToLower())
                {
                    case "connect":
                        {
                            if (!CommunicationHandler.Singleton.EasyClient.ClientConnected)
                            {
                                var conRes = CommunicationHandler.ConnectToHost();
                                SynapseController.Server.Logger.Warn((conRes && CommunicationHandler.Singleton.EasyClient.ClientConnected).ToString());
                                result.Message = (conRes && CommunicationHandler.Singleton.EasyClient.ClientConnected) ? "Connection established" : $"Connection failed. Is the entered IP & Port ({SyncordPlugin.Config.DiscordBotAddress}:{SyncordPlugin.Config.DiscordBotPort}) valid?";
                                result.State = (conRes && CommunicationHandler.Singleton.EasyClient.ClientConnected) ? CommandResultState.Ok : CommandResultState.Error;
                            }
                            else
                            {
                                result.Message = "Client already connected";
                                result.State = CommandResultState.Error;
                            }
                        }
                        break;
                    case "disconnect":
                        {
                            if (CommunicationHandler.Singleton.EasyClient.ClientConnected)
                            {
                                CommunicationHandler.DisconnectFromHost();
                                result.Message = "Disconnected ";
                                result.State = CommandResultState.Ok;
                            }
                            else
                            {
                                result.Message = "Client already disconnected";
                                result.State = CommandResultState.Error;
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                result.Message = $"Exception:\n{e}";
                result.State = CommandResultState.Error;
            }
            return result;
        }
    }
}
