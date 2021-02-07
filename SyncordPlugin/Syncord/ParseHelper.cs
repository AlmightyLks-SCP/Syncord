/*
    The following license applies to the entirety of this Repository and Solution.
    
    
    
    Copyright 2021 AlmightyLks

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
    or implied. See the License for the specific language governing
    permissions and limitations under the License.
*/
using Mirror.LiteNetLib4Mirror;
using Synapse;
using Synapse.Api;
using Synapse.Api.Events.SynapseEventArguments;
using Synapse.Api.Roles;
using Synapse.Permission;
using SyncordInfo.EventArgs;
using SyncordInfo.SimplifiedTypes;
using System;
using System.Net.Http;
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
        public static Vector3 Parse(this SimpleVector3 simpleVector3)
            => new Vector3(simpleVector3.X, simpleVector3.Y, simpleVector3.Z);
        public static SimpleVector3 Parse(this Vector3 vector)
            => new SimpleVector3(vector.x, vector.y, vector.z);

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
                Team = (role.GetTeam().ToString(), (byte)role.GetTeam())
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
                SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                RoundSummary = ev
            };
        }
        public static bool TryParse(this SimpleRoundSummary ev, out RoundEnd roundEnd)
        {
            try
            {
                roundEnd = new RoundEnd()
                {
                    SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                    SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                    RoundSummary = ev,
                    Time = DateTime.Now
                };
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
                SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                Identifier = "join",
                Player = ev.Player.Parse(),
                Time = DateTime.Now
            };
        }
        public static bool TryParse(this PlayerJoinEventArgs ev, out PlayerJoinLeave joined)
        {
            try
            {
                joined = new PlayerJoinLeave()
                {
                    SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                    //SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                    SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                    Identifier = "join",
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

        public static PlayerJoinLeave Parse(this PlayerLeaveEventArgs ev)
        {
            return new PlayerJoinLeave()
            {
                SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                Identifier = "leave",
                Player = ev.Player.Parse(true),
                Time = DateTime.Now
            };
        }
        public static bool TryParse(this PlayerLeaveEventArgs ev, out PlayerJoinLeave left)
        {
            try
            {
                left = new PlayerJoinLeave()
                {
                    SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                    SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                    Identifier = "leave",
                    Player = ev.Player.Parse(true),
                    Time = DateTime.Now
                };
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
                SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                HitInfo = ev.HitInfo.Parse(),
                Killer = ev.Killer.Parse(),
                Victim = ev.Victim.Parse(),
                Time = DateTime.Now
            };
        }
        public static bool TryParse(this PlayerDeathEventArgs ev, out PlayerDeath death)
        {
            try
            {
                death = new PlayerDeath()
                {
                    SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                    SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                    Killer = ev.Killer.Parse(),
                    Victim = ev.Victim.Parse(),
                    HitInfo = ev.HitInfo.Parse(),
                    Time = DateTime.Now
                };
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
                SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                Time = DateTime.Now,
                BannedPlayer = ev.BannedPlayer.Parse(),
                BanningPlayer = ev.Issuer.Parse(),
                Duration = ev.Duration,
                Reason = ev.Reason
            };
        }
        public static bool TryParse(this PlayerBanEventArgs ev, out PlayerBan ban)
        {
            try
            {
                ban = new PlayerBan()
                {
                    SameMachine = SyncordPlugin.Config.DiscordBotAddress == "127.0.0.1",
                    SLFullAddress = $"{SyncordPlugin.IPv4}:{Server.Get.Port}",
                    Time = DateTime.Now,
                    BannedPlayer = ev.BannedPlayer.Parse(),
                    BanningPlayer = ev.Issuer.Parse(),
                    Duration = ev.Duration,
                    Reason = ev.Reason
                };
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
