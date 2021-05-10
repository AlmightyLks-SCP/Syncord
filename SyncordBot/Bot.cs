using DSharpPlus;
using System.Threading.Tasks;
using System;
using System.Linq;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using EasyCommunication;
using Serilog;
using SyncordBot.SyncordCommunication;
using SyncordBot.Configs;
using EasyCommunication.Connection;
using System.IO;

namespace SyncordBot
{
    public sealed class Bot
    {
        public static BotConfig BotConfig { get; private set; }
        public static TranslationConfig TranslationConfig { get; private set; }
        public static GuildConfig GuildConfig { get; private set; }
        public static AliasConfig AliasConfig { get; private set; }

        public DiscordClient Client { get; set; }
        public CommandsNextExtension Commands { get; set; }
        public EasyHost EasyHost { get; set; }
        public CommunicationHandler CommunicationHandler { get; set; }
        public string PresenceString { get; set; }

        private IServiceProvider _service;
        private ILogger _logger;
        private Random random;

        //Entry-point
        static void Main()
            => new Bot().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        {
            Console.Title = "Syncord";

            if (!Directory.Exists("logs"))
            {
                Directory.CreateDirectory("logs");
            }

            //Instantiate Logger
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt",
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 24)
                .CreateLogger();

            //Load Discord Bot Configs
            LoadConfigs();

            random = new Random();

            PresenceString = BotConfig.DiscordActivity.Name;

            _logger.Information($"Loaded Translation: {TranslationConfig.Translation.Language}.");

            //Instantiate EasyHost
            EasyHost = new EasyHost(2500, BotConfig.Port, BotConfig.RemoteConnection ? IPAddress.Any : IPAddress.Loopback)
            {
                BufferSize = 16384
            };

            await SetupDiscordClient();

            //Adding Singletons of the Bot & EasyHost
            _service = new ServiceCollection()
                .AddSingleton(this)
                .AddSingleton(EasyHost)
                .BuildServiceProvider();

            LoadCommands();

            //Fire and forget
            new Task(async () => await UpdatePresence()).Start();

            //Instatiate CommunicationHandler
            CommunicationHandler = new CommunicationHandler(EasyHost, this, _logger);

            await CommunicationHandler.CreateChannelEmbedQueues();

            await Task.Delay(-1);
        }

        private async Task SetupDiscordClient()
        {
            //Create Discord Client
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = BotConfig.BotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MessageCacheSize = 0
            });

            //Connect Discord Client
            await Client.ConnectAsync();
        }

        private void LoadCommands()
        {
            //Create Command Configs
            var cmdCfg = new CommandsNextConfiguration
            {
                CaseSensitive = false,
                EnableDefaultHelp = true,
                StringPrefixes = new[] { BotConfig.Prefix },
                IgnoreExtraArguments = true,
                EnableMentionPrefix = false,
                EnableDms = true,
                Services = _service
            };

            //Register Commandhandler thingy
            Commands = Client.UseCommandsNext(cmdCfg);

            //Register Command
            //Commands.RegisterCommands<Test>();
        }

        private void LoadConfigs()
        {
            try
            {
                BotConfig = BotConfig.Load();
                GuildConfig = GuildConfig.Load();
                TranslationConfig = TranslationConfig.Load();
                AliasConfig = AliasConfig.Load();
            }
            catch (Exception e)
            {
                _logger.Error($"Error loading config:\n{e}\n\nPress any key to continue");
                Console.ReadKey();
            }
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
                        ActivityType = BotConfig.DiscordActivity.Activity,
                        Name = PresenceString
                            .Replace("{SLCount}", EasyHost.ClientConnections.Count.ToString())
                    });
                }
                catch (Exception e)
                {
                    _logger.Error($"UpdatePresence threw:\n{e}");
                }
            }
        }
    }
}
