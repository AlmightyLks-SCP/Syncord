using Synapse.Api.Events.SynapseEventArguments;
using Synapse.Api.Plugin;
using SyncordPlugin.Config;
using SyncordPlugin.EventHandler;
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
        public static string IPv4 { get; private set; }

        internal PluginEventHandler EventHandler { get; set; }

        public override void Load()
        {
            IPv4 = GetIPv4().Result;
            EventHandler = new PluginEventHandler();
        }

        private static async Task<string> GetIPv4()
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
                        return await response.Result.Content.ReadAsStringAsync();
                    else
                        return "127.0.0.1";
                }
            }
            catch
            {
                return "127.0.0.1";
            }
        }
    }
}
