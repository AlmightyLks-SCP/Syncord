using Mirror.LiteNetLib4Mirror;
using Synapse;
using Synapse.Api;
using Synapse.Api.Events.SynapseEventArguments;
using Synapse.Api.Roles;
using Synapse.Config;
using Synapse.Permission;
using SyncordInfo.Communication;
using SyncordInfo.EventArgs;
using SyncordInfo.SimplifiedTypes;
using System;
using System.Net.Http;
using UnityEngine;

namespace SyncordPlugin.Syncord
{
    public static class ParseHelper
    {
        public static DataBase GetDataBase()
        {
            return new DataBase()
            {
                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                Time = DateTime.Now
            };
        }
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
        public static SimpleDamageType Parse(this DamageTypes.DamageType damageType)
        {
            return new SimpleDamageType()
            {
                IsScp = damageType.isScp,
                IsWeapon = damageType.isWeapon,
                Name = damageType.name,
                WeaponId = damageType.weaponId
            };
        }
        public static SimpleHitInfo Parse(this PlayerStats.HitInfo hitInfo)
        {
            return new SimpleHitInfo()
            {
                Amount = hitInfo.Amount,
                Attacker = hitInfo.Attacker,
                Tool = hitInfo.Tool,
                DamageType = hitInfo.GetDamageType().Parse(),
                Time = hitInfo.Time
            };
        }
        public static Vector3 Parse(this SerializedVector3 simpleVector3)
            => new Vector3(simpleVector3.X, simpleVector3.Y, simpleVector3.Z);
        public static SerializedVector3 Parse(this Vector3 vector)
            => new SerializedVector3(vector.x, vector.y, vector.z);

        public static SimpleCustomRole Parse(this RoleType role, Team team)
        {
            return new SimpleCustomRole()
            {
                Role = (role.ToString(), (int)role),
                Team = (team.ToString(), (byte)team)
            };
        }
        public static SimpleCustomRole Parse(this IRole role)
        {
            return new SimpleCustomRole()
            {
                Role = (role.GetRoleName(), role.GetRoleID()),
                Team = (Server.Get.TeamManager.GetTeam(role.GetTeamID()).Info.Name, role.GetTeamID())
            };
        }
        public static bool TryParse(this IRole role, out SimpleCustomRole simpleCustomRole)
        {
            try
            {
                simpleCustomRole = role.Parse();
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                simpleCustomRole = null;
                return false;
            }
            return true;
        }

        public static SimplePlayer Parse(this Player player, bool isLeaving = false)
        {
            return new SimplePlayer()
            {
                Ping = isLeaving ? -1 : player.Ping,
                DisplayName = player.DisplayName,
                Nickname = player.NickName,
                Health = player.Health,
                MaxHealth = player.MaxHealth,
                ArtificialHealth = player.ArtificialHealth,
                MaxArtificialHealth = player.MaxArtificialHealth,
                Role = player.CustomRole == null ? player.RoleType.Parse(player.Team) : player.CustomRole.Parse(),
                UserId = player.UserId,
                SynapseGroup = player.SynapseGroup.Parse(),
                DoNotTrack = player.DoNotTrack,
                IPAddress = player.DoNotTrack ? string.Empty : player.IpAddress,
                IsCuffed = player.IsCuffed
            };
        }
        public static bool TryParse(this Player player, out SimplePlayer simplePlayer)
        {
            try
            {
                simplePlayer = player.Parse();
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                simplePlayer = null;
                return false;
            }
            return true;
        }

        public static SimpleRoundSummary GetSimpleRoundSummary(int totalKills)
        {
            return new SimpleRoundSummary()
            {
                RoundTime = RoundSummary.roundTime,
                TotalEscapedDClass = RoundSummary.escaped_ds,
                TotalEscapedScientists = RoundSummary.escaped_scientists,
                TotalKills = totalKills,
                TotalKillsByFragGrenade = RoundSummary.kills_by_frag,
                TotalKillsByScps = RoundSummary.kills_by_scp,
                TurnedIntoZombies = RoundSummary.changed_into_zombies
            };
        }

        public static RoundEnd Parse(this SimpleRoundSummary ev)
        {
            return new RoundEnd()
            {
                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                Time = DateTime.Now,
                MessageType = MessageType.Event,
                RoundSummary = ev
            };
        }
        public static bool TryParse(this SimpleRoundSummary ev, out RoundEnd roundEnd)
        {
            try
            {
                roundEnd = ev.Parse();
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                roundEnd = null;
                return false;
            }
            return true;
        }

        public static PlayerJoinLeave Parse(this PlayerJoinEventArgs ev)
        {
            return new PlayerJoinLeave()
            {
                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                Time = DateTime.Now,
                MessageType = MessageType.Event,
                Identifier = "join",
                Player = ev.Player?.Parse() ?? SimplePlayer.Unknown
            };
        }
        public static bool TryParse(this PlayerJoinEventArgs ev, out PlayerJoinLeave joined)
        {
            try
            {
                joined = ev.Parse();
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                joined = null;
                return false;
            }
            return true;
        }

        public static PlayerJoinLeave Parse(this PlayerLeaveEventArgs ev)
        {
            return new PlayerJoinLeave()
            {
                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                Time = DateTime.Now,
                MessageType = MessageType.Event,
                Identifier = "leave",
                Player = ev.Player?.Parse(true) ?? SimplePlayer.Unknown
            };
        }
        public static bool TryParse(this PlayerLeaveEventArgs ev, out PlayerJoinLeave left)
        {
            try
            {
                left = ev.Parse();
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                left = null;
                return false;
            }
            return true;
        }

        public static PlayerDeath Parse(this PlayerDeathEventArgs ev)
        {
            return new PlayerDeath()
            {
                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                Time = DateTime.Now,
                MessageType = MessageType.Event,
                HitInfo = ev.HitInfo.Parse(),
                Killer = ev.Killer?.Parse() ?? SimplePlayer.Unknown,
                Victim = ev.Victim?.Parse() ?? SimplePlayer.Unknown
            };
        }
        public static bool TryParse(this PlayerDeathEventArgs ev, out PlayerDeath death)
        {
            try
            {
                death = ev.Parse();
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                death = null;
                return false;
            }
            return true;
        }

        public static PlayerBan Parse(this PlayerBanEventArgs ev)
        {
            return new PlayerBan()
            {
                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                SLFullAddress = $"{SyncordPlugin.ServerIPv4}:{Server.Get.Port}",
                MessageType = MessageType.Event,
                Time = DateTime.Now,
                BannedPlayer = ev.BannedPlayer?.Parse() ?? SimplePlayer.Unknown,
                BanningPlayer = ev.Issuer?.Parse() ?? SimplePlayer.Unknown,
                Duration = ev.Duration,
                Reason = ev.Reason
            };
        }
        public static bool TryParse(this PlayerBanEventArgs ev, out PlayerBan ban)
        {
            try
            {
                ban = ev.Parse();
            }
            catch (Exception e)
            {
                Synapse.Api.Logger.Get.Error($"{e}");
                ban = null;
                return false;
            }
            return true;
        }
    }
}
