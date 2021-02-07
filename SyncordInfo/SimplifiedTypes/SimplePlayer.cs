/*
    The following license applies to the entirety of this Repository and Solution.
    
    TLDR.: Don't use a damn thing from my work without crediting me, else I'll smite your arse.
    
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
        public SimpleCustomRole Role { get; set; }
        [ProtoMember(6)]
        public int MaxArtificialHealth { get; set; }
        [ProtoMember(7)]
        public float ArtificialHealth { get; set; }
        [ProtoMember(8)]
        public int MaxHealth { get; set; }
        [ProtoMember(9)]
        public float Health { get; set; }
        [ProtoMember(10)]
        public SimpleSynapseGroup SynapseGroup { get; set; }
        [ProtoMember(11)]
        public string IPAddress { get; set; }
        [ProtoMember(12)]
        public bool DoNotTrack { get; set; }
        [ProtoMember(13)]
        public bool IsCuffed { get; set; }
        public SimplePlayer()
        {
            Ping = -1;
            DisplayName = string.Empty;
            Nickname = string.Empty;
            UserId = string.Empty;
            Role = new SimpleCustomRole();
            MaxArtificialHealth = -1;
            ArtificialHealth = -1;
            MaxHealth = -1;
            Health = -1;
            SynapseGroup = null;
            IPAddress = string.Empty;
            IsCuffed = false;
        }
    }
}
