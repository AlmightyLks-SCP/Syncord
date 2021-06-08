using Synapse.Api.Plugin;
using SyncordPlugin.Config;
using SyncordPlugin.EventHandler;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SyncordPlugin
{
    [PluginInformation(
        Author = "AlmightyLks",
        Description = "A way of connecting Discord with you SCP SL Server",
        Name = "SyncordPlugin",
        SynapseMajor = 2,
        SynapseMinor = 6,
        SynapsePatch = 0,
        Version = "0.9.2"
        )]
    public class SyncordPlugin : AbstractPlugin
    {
        [Synapse.Api.Plugin.Config(section = "Syncord")]
        public static SyncordConfig Config { get; set; }
        public static string ServerIPv4 { get; private set; }

        internal PluginEventHandler EventHandler { get; private set; }

        public override void Load()
        {
            ServerIPv4 = FetchIPv4()
                .GetAwaiter()
                .GetResult();
            if (!string.IsNullOrWhiteSpace(ServerIPv4))
            {
                EventHandler = new PluginEventHandler(Config.DiscordBotAddress, Config.DiscordBotPort);
            }
        }
        private static async Task<string> FetchIPv4()
        {
            try
            {
                Task waitTask;
                using (HttpClient client = new HttpClient())
                {
                    waitTask = Task.Delay(2500);
                    var response = client.GetAsync("https://api.ipify.org/");
                    int index = Task.WaitAny(response, waitTask);
                    if (index == 0)
                    {
                        return await response.Result.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Synapse.Api.Logger.Get.Error("[Syncord] Couldn't fetch the ip neccessary for communication in time");
                        return string.Empty;
                    }
                }
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"[Syncord] Couldn't fetch the ip neccessary for communication due to an error\n{e}");
                return string.Empty;
            }
        }
    }
}
