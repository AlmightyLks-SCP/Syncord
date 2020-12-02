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

namespace SyncordBot
{
    internal class Bot
    {
        public static Config Configs { get; set; }
        internal static DiscordClient Client { get; private set; }
        internal static SyncordBehaviour Syncord { get; set; }

        private static Logger logger;

        static void Main()
            => MainAsync().GetAwaiter().GetResult();
        private async static Task MainAsync()
        {
            //Instantiate Logger
            logger = new Logger();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(BotExiting);

            //Load Discord Bot Configs
            LoadConfigs();

            //Create Discord Client
            Client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Configs.BotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MessageCacheSize = 0
            });

            //Initialize Message Handling
            MessageHandler.Init(Client);

            Client.Ready += Discord_Ready;

            //Connect Discorc Client
            await Client.ConnectAsync();

            //Instantiate SyncordBehaviour
            Syncord = new SyncordBehaviour(Client, logger);

            //Run SyncordBehaviour
            _ = Syncord.Start();

            await Task.Delay(-1);
        }

        private static async Task Discord_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            var game = new DiscordGame("SCP: Secret Laboratory");
            await Client.UpdateStatusAsync(game, UserStatus.Idle, DateTimeOffset.UtcNow);
        }

        private static void BotExiting(object sender, EventArgs e)
            => logger.FileLogExceptions();

        private static void LoadConfigs()
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
