/*
    The following license applies to the entirety of this Repository and Solution.
    
    TLDR.: Don't use a damn thing from my work without crediting me, else I'll smite your arse.
    
    Copyright 2021 AlmightyLks

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the License for the specific language governing
    permissions and limitations under the License.
*/
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
