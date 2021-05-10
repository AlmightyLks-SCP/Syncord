using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.Models.DTO
{
    public class PlayerCountDto
    {
        public long Id { get; set; }
        public ScpSlServer Server { get; set; }
        public ushort MaxPlayers { get; set; }
        public ushort PlayerCount { get; set; }
        public DateTime DateTime { get; set; }

        public PlayerCountDto()
        {
            Id = default;
            Server = default;
            MaxPlayers = default;
            PlayerCount = default;
            DateTime = default;
        }
        public PlayerCountDto(ushort maxPlayers, ushort playerCount, DateTime dateTime, ScpSlServer server)
        {
            Id = default;
            Server = server;
            MaxPlayers = maxPlayers;
            PlayerCount = playerCount;
            DateTime = dateTime;
        }
        public PlayerCountDto(long id, ushort maxPlayers, ushort playerCount, DateTime dateTime, ScpSlServer server)
        {
            Id = id;
            Server = server;
            MaxPlayers = maxPlayers;
            PlayerCount = playerCount;
            DateTime = dateTime;
        }
    }
}
