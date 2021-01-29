using DSharpPlus;
using SyncordBot.BotConfigs;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using SyncordBot.Commands;
using Microsoft.Extensions.DependencyInjection;
using SyncordBot.Models;
using System.Net;
using EasyCommunication.Host.Connection;
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

        //Entry-point
        static void Main()
            => new Bot().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            //Load Discord Bot Configs
            LoadConfigs();

            //Instantiate Logger
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("log.txt",
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true)
                .CreateLogger();

            //Instantiate EasyHost
            EasyHost = new EasyHost(5000, Configs.Port, IPAddress.Any);

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
            Commands.RegisterCommands<ServerStatsCommand>();

            await Task.Delay(-1);
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
