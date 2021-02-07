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
using DSharpPlus;
using SyncordBot.Configs.BotConfigs;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using SyncordBot.Models;
using System.Net;
using EasyCommunication.Connection;
using Serilog;
using SyncordBot.SyncordCommunication;

namespace SyncordBot
{
    public class Bot
    {
        public static Config Configs { get; set; }
        public DiscordClient Client { get; set; }
        public CommandsNextExtension Commands { get; set; }
        public ServerStats ServerStats { get; set; }
        public EasyHost EasyHost { get; set; }
        public CommunicationHandler CommunicationHandler { get; set; }

        private IServiceProvider _service;
        private ILogger _logger;
        private Random random;

        //Entry-point
        static void Main()
            => new Bot().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            Console.Title = "Syncord";

            random = new Random();

            //Load Discord Bot Configs
            LoadConfigs();

            //Instantiate Logger
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true)
                .CreateLogger();

            _logger.Information($"Loaded Translation: {Configs.TranslationConfig.Translation.Language}.");

            //Instantiate EasyHost
            EasyHost = new EasyHost(5000, Configs.Port, IPAddress.Any)
            {
                BufferSize = 2048
            };

            //Instatiate CommunicationHandler
            CommunicationHandler = new CommunicationHandler(EasyHost, this, _logger);

            //Instantiate ServerStats
            ServerStats = new ServerStats();

            //Create Discord Client
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Configs.BotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MessageCacheSize = 0
            });

            Client.Ready += Client_Ready;

            //Connect Discord Client
            await Client.ConnectAsync();

            await CommunicationHandler.CreateChannelEmbedQueues();

            //Adding Singletons of the Bot & EasyHost
            _service = new ServiceCollection()
                .AddSingleton(this)
                .AddSingleton(EasyHost)
                .BuildServiceProvider();

            //Create Command Configs
            var cmdCfg = new CommandsNextConfiguration
            {
                CaseSensitive = false,
                EnableDefaultHelp = true,
                StringPrefixes = new[] { Configs.Prefix },
                IgnoreExtraArguments = true,
                EnableMentionPrefix = false,
                EnableDms = true,
                Services = _service
            };

            //Register Commandhandler thingy
            Commands = Client.UseCommandsNext(cmdCfg);

            //Register Command
            //Commands.RegisterCommands<ServerStatsCommand>();

            //Fire and forget
            new Task(async () => await UpdatePresence()).Start();

            await Task.Delay(-1);
        }
        private async Task UpdatePresence()
        {
            while (true)
            {
                try
                {
                    //  Presence updates have a ratelimit of ~5 times every 60 seconds, 20 seconds being the quickest you may update your presence.
                    //  Additionally, it is not well-accepted to be spamming the API every exact X seconds.
                    //  I will not edge the limits, so it's 21 seconds min and a safe 26.5 seconds max
                    await Task.Delay(random.Next(21000, 26500));
                    await Client.UpdateStatusAsync(new DiscordActivity()
                    {
                        ActivityType = Configs.DiscordActivity.Activity,
                        Name = Configs.DiscordActivity.Name.Replace("{SLCount}", EasyHost.ClientConnections.Count.ToString())
                    });
                }
                catch (Exception e)
                {
                    _logger.Error($"UpdatePresence threw:\n{e}");
                }
            }
        }
        private async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
            => await sender.UpdateStatusAsync(new DiscordActivity($"0 SCP SL Servers", ActivityType.Watching), UserStatus.Online);

        private void LoadConfigs()
        {
            Configs = new Config();

            string configPath = Path.Combine(Directory.GetCurrentDirectory(), "Config.json");

            if (!File.Exists(configPath))
                File.WriteAllText(configPath, JsonConvert.SerializeObject(Configs));
            else
                Configs = JsonConvert.DeserializeObject<Config>(File.ReadAllText(configPath));
        }
    }
}
