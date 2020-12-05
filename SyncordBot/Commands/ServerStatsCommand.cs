using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using SyncordBot.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyncordBot.Commands
{
    public class ServerStatsCommand : BaseCommandModule
    {
        public Bot Bot { private get; set; }

        [Command("Set")]
        public async Task SetServer(CommandContext ctx, string server)
        {
            if (server == "all") //Stats for all servers
            {
                //If there is no scp server
                if (Bot.Syncord.Heartbeats.Count == 0)
                {
                    await ctx.Message.RespondAsync("Not connected to any server.");
                    return;
                }

                //Look for ServerStats Entry for this channel
                var serverEntry = Bot.ServerStats.Servers.FirstOrDefault((_) => _.ChannelID == ctx.Channel.Id);

                //If doesn't exist
                if (serverEntry is null)
                {
                    var channelServer = new ChannelServer()
                    {
                        GuildID = ctx.Guild.Id,
                        ChannelID = ctx.Channel.Id,
                        ServerPorts = new HashSet<int>()
                    };

                    foreach (var port in Bot.Syncord.Heartbeats.Keys)
                        channelServer.ServerPorts.Add(port);

                    Bot.ServerStats.Servers.Add(channelServer);
                }
                else //If exists, add all ports to it
                {
                    foreach (var port in Bot.Syncord.Heartbeats.Keys)
                        serverEntry.ServerPorts.Add(port);
                }
            }
            else if (int.TryParse(server, out int serverPort)) //Stats for a specific server
            {
                //If there is no scp server with that port
                if (!Bot.Syncord.Heartbeats.ContainsKey(serverPort))
                {
                    await ctx.Message.RespondAsync("No Server with that port connected.");
                    return;
                }

                //Look for ServerStats Entry for this channel
                var serverEntry = Bot.ServerStats.Servers.FirstOrDefault((_) => _.ServerPorts.Any((e) => e == serverPort));
                
                //If entry doesn't exist
                if (serverEntry is null)
                {
                    var channelServer = new ChannelServer()
                    {
                        GuildID = ctx.Guild.Id,
                        ChannelID = ctx.Channel.Id,
                        ServerPorts = new HashSet<int>()
                    };

                    foreach (var port in Bot.Syncord.Heartbeats.Keys)
                        channelServer.ServerPorts.Add(port);

                    Bot.ServerStats.Servers.Add(channelServer);
                }
                else //If entry does exist
                {
                    serverEntry.ServerPorts.Add(serverPort);
                }
            }
        }
    }
}
