using DSharpPlus;
using SyncordBot.Logging;
using SyncordBot.BotConfigs;
using SyncordBot.EventHandlers;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncordBot.Syncord;
using System;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using SyncordBot.Commands;
using Microsoft.Extensions.DependencyInjection;
using SyncordBot.Models;

namespace SyncordBot
{
    public class Bot
    {
        public Config Configs { get; set; }
        public DiscordClient Client { get; set; }
        public SyncordConnection Syncord { get; set; }
        public Heartbeat Heartbeat { get; set; }
        public CommandsNextExtension Commands { get; set; }
        public ServerStats ServerStats { get; set; }

        private IServiceProvider _service;
        private Logger logger;

        //Entry-point
        static void Main()
            => new Bot().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            //Load Discord Bot Configs
            LoadConfigs();

            //Instantiate Logger
            logger = new Logger();

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

            //Instantiate SyncordConnection
            Syncord = new SyncordConnection(logger, this);

            //Instantiate SyncordConnection
            Heartbeat = new Heartbeat(logger, this);

            //Adding singletons of the Bot, the Client & SyncordBehaviour
            _service = new ServiceCollection()
                .AddSingleton(this)
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

            //Run SyncordBehaviour
            Syncord.Start();

            //Run SyncordBehaviour
            Heartbeat.Start();

            await Task.Delay(-1);
        }

        private async Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs e)
            => await sender.UpdateStatusAsync(new DiscordActivity($"0 SCP SL Servers"), UserStatus.Online);

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
