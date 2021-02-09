using EasyCommunication.Connection;
using Synapse.Command;
using SyncordPlugin.Syncord;
using System;
using System.Net;

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
        private SyncordPlugin _plugin;
        public SyncordConnection(SyncordPlugin plugin)
        {
            _plugin = plugin;
        }
        public CommandResult Execute(CommandContext context)
        {
            EasyClient easyClient = _plugin.EventHandler.CommunicationHandler.EasyClient;
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
                    result.Message = $"Syncord Connected: {easyClient.ClientConnected}";
                    result.State = CommandResultState.Ok;
                    return result;
                }

                switch (context.Arguments.At(0).ToLower())
                {
                    case "connect":
                        {
                            if (!easyClient.ClientConnected)
                            {
                                bool worked = true;
                                try
                                {
                                    if (IPAddress.TryParse(SyncordPlugin.Config.DiscordBotAddress, out IPAddress botAddress))
                                        easyClient.ConnectToHost(botAddress, SyncordPlugin.Config.DiscordBotPort);
                                    else
                                        worked = false;   //Invalid IP
                                }
                                catch
                                {
                                    worked = false;       //Invalid Host
                                }
                                SynapseController.Server.Logger.Warn((worked && easyClient.ClientConnected).ToString());
                                result.Message = (worked && easyClient.ClientConnected) ? 
                                    "Connection established" : 
                                    $"Connection failed. Is the entered IP & Port ({SyncordPlugin.Config.DiscordBotAddress}:{SyncordPlugin.Config.DiscordBotPort}) valid?";
                                result.State = (worked && easyClient.ClientConnected) ? CommandResultState.Ok : CommandResultState.Error;
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
                            if (easyClient.ClientConnected)
                            {
                                easyClient.DisconnectFromHost();
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
