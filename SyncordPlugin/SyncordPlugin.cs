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
        SynapseMinor = 2,
        SynapsePatch = 0,
        Version = "0.9.1"
        )]
    public class SyncordPlugin : AbstractPlugin
    {
        [Synapse.Api.Plugin.Config(section = "Syncord")]
        public static SyncordConfig Config { get; set; }
        public static string IPv4 { get; private set; }

        private PluginEventHandler handler;

        public override void Load()
        {
            IPv4 = GetIPv4().Result;
            handler = new PluginEventHandler();
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
