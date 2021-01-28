using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public class SimplePlayer
    {
        [ProtoMember(1)]
        public int Ping { get; set; }
        [ProtoMember(2)]
        public string DisplayName { get; set; }
        [ProtoMember(3)]
        public string Nickname { get; set; }
        [ProtoMember(4)]
        public string UserId { get; set; }
        [ProtoMember(5)]
        public int RoleType { get; set; }
        [ProtoMember(6)]
        public int RoleID { get; set; }
        [ProtoMember(7)]
        public int MaxArtificialHealth { get; set; }
        [ProtoMember(8)]
        public float ArtificialHealth { get; set; }
        [ProtoMember(9)]
        public int MaxHealth { get; set; }
        [ProtoMember(10)]
        public float Health { get; set; }
        [ProtoMember(11)]
        public SimpleSynapseGroup SynapseGroup { get; set; }
        [ProtoMember(12)]
        public string IPAddress { get; set; }
        [ProtoMember(13)]
        public bool DoNotTrack { get; set; }
        public SimplePlayer()
        {
            Ping = -1;
            DisplayName = string.Empty;
            Nickname = string.Empty;
            UserId = string.Empty;
            RoleType = -1;
            RoleID = -1;
            MaxArtificialHealth = -1;
            ArtificialHealth = -1;
            MaxHealth = -1;
            Health = -1;
            SynapseGroup = null;
            IPAddress = string.Empty;
        }
    }
}
