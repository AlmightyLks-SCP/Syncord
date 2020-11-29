using DSharpPlus;
using SyncordBot.BotConfigs;
using SyncordBot.EventHandlers;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncordBot.Syncord;

namespace SyncordBot
{
    internal class Bot
    {
        internal static DiscordClient Client { get; private set; }
        public static Config Configs { get; set; }
        internal static SyncordBehaviour syncord { get; set; }

        static void Main()
            => MainAsync().GetAwaiter().GetResult();
        private async static Task MainAsync()
        {
            LoadConfigs();
            
            var discord = new DiscordClient(new DiscordConfiguration()
            {
                Token = Configs.BotToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MessageCacheSize = 0
            });

            MessageHandler.Init(discord);

            await discord.ConnectAsync();

            syncord = new SyncordBehaviour(discord);

            _ = syncord.Start();

            await Task.Delay(-1);
        }
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
