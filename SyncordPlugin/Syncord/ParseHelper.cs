using Synapse;
using Synapse.Api;
using Synapse.Api.Events.SynapseEventArguments;
using Synapse.Permission;
using SyncordInfo.EventArgs;
using SyncordInfo.SimplifiedTypes;
using System;
using UnityEngine;

namespace SyncordPlugin.Syncord
{
    public static class ParseHelper
    {
        public static SimpleSynapseGroup Parse(this SynapseGroup group)
        {
            return new SimpleSynapseGroup()
            {
                Default = group.Default,
                RemoteAdmin = group.RemoteAdmin,
                Northwood = group.Northwood,
                Hidden = group.Hidden,
                KickPower = group.KickPower,
                RequiredKickPower = group.RequiredKickPower,
                Members = group.Members,
                Permissions = group.Permissions,
                Badge = group.Badge,
                Color = group.Color,
                Cover = group.Cover
            };
        }
        public static Vector3 Parse(this SimpleVector3 simpleVector3)
            => new Vector3(simpleVector3.X, simpleVector3.Y, simpleVector3.Z);
        public static SimpleVector3 Parse(this Vector3 vector)
            => new SimpleVector3(vector.x, vector.y, vector.z);

        public static SimplePlayer Parse(this Player player)
        {
            return new SimplePlayer()
            {
                Ping = player.Ping,
                DisplayName = player.DisplayName,
                Nickname = player.NickName,
                Health = player.Health,
                MaxHealth = player.MaxHealth,
                ArtificialHealth = player.ArtificialHealth,
                MaxArtificialHealth = player.MaxArtificialHealth,
                RoleID = player.RoleID,
                RoleType = (int)player.RoleType,
                UserId = player.UserId,
                SynapseGroup = player.SynapseGroup.Parse(),
                DoNotTrack = player.DoNotTrack,
                IPAddress = player.DoNotTrack ? string.Empty : player.IpAddress
            };
        }
        public static PlayerJoined Parse(this PlayerJoinEventArgs ev)
        {
            return new PlayerJoined()
            {
                ServerPort = Server.Get.Port,
                Player = ev.Player.Parse(),
                Time = DateTime.Now
            };
        }
        public static bool TryParse(this PlayerJoinEventArgs ev, out PlayerJoined joined)
        {
            try
            {
                joined = new PlayerJoined()
                {
                    ServerPort = Server.Get.Port,
                    Player = ev.Player.Parse(),
                    Time = DateTime.Now
                };
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                joined = null;
                return false;
            }
            return true;
        }
    }
}
