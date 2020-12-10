using DSharpPlus.Entities;
using SyncordBot.Logging;
using SyncordInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Timers;

namespace SyncordBot.Syncord
{
    public class Heartbeat
    {
        public Dictionary<int, int> Heartbeats { get; private set; }
        private BinaryFormatter binaryFormatter;
        private Timer heartbeatTimer;
        private Bot bot;
        private ILogger logger;

        public Heartbeat(ILogger logger, Bot bot)
        {
            this.logger = logger;
            this.bot = bot;
            Heartbeats = new Dictionary<int, int>();
            heartbeatTimer = new Timer();
            binaryFormatter = new BinaryFormatter();
        }

        public void Start()
        {
            heartbeatTimer.Interval = 5_000;
            heartbeatTimer.AutoReset = true;
            heartbeatTimer.Elapsed += HeartbeatTimer_Elapsed;
            heartbeatTimer.Start();
        }
        private async void HeartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
            => await CheckHeartbeats();
        private async Task CheckHeartbeats()
        {
            try
            {
                foreach (var connection in bot.Syncord.ClientConnections.ToArray())
                {
                    if (!Heartbeats.TryGetValue(connection.Key, out int val))
                        return;

                    if (val == 0) //If no hearbeats have been returned
                    {
                        logger.Warn($"No hearbeats received from port {connection.Key}. Connection closed.");

                        //Remove from storage
                        Heartbeats.Remove(connection.Key);
                        bot.Syncord.ClientConnections.Remove(connection.Key);

                        //Close connection
                        connection.Value.Client.Close();
                    }
                }

                foreach (var _ in Heartbeats.ToList())
                    Heartbeats[_.Key] = 0;

                UpdateBotActivity();
                await SendHeartbeats();
            }
            catch (Exception e)
            {
                logger.Error($"Exception in Check Heartbeats:\n{e}");
                logger.Exception($"Exception in Check Heartbeats:\n{e}");
            }
        }
        private async Task SendHeartbeats()
        {
            try
            {
                foreach (var connection in bot.Syncord.ClientConnections.ToArray())
                {
                    if (!connection.Value.Connected)
                        continue;

                    //Send out Heartbeats to every client
                    binaryFormatter.Serialize(connection.Value.GetStream(), new SharedInfo() { Port = bot.Configs.Port, RequestType = RequestType.Heartbeat, Content = "Heartbeat" });
                    logger.Info($"Sent {((IPEndPoint)(connection.Value.Client.RemoteEndPoint)).Port} heartbeat");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Error sending Heartbeats:\n{e}");
                logger.Exception($"Error sending Heartbeats:\n{e}");
            }
        }

        private void UpdateBotActivity()
            => bot.Client.UpdateStatusAsync(new DiscordActivity($"on {bot.Syncord.ClientConnections.Count} SCP SL Servers", ActivityType.Watching), UserStatus.DoNotDisturb).Wait();
    }
}
