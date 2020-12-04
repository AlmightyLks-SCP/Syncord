using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Threading.Tasks;

namespace SyncordBot.Commands
{
    public class ServerStats : BaseCommandModule
    {
        public Bot Bot { private get; set; }

        [Command("Set")]
        public async Task SetServer(CommandContext ctx, string server)
        {

        }
    }
}
