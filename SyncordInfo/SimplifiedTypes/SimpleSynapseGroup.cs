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
    public sealed class SimpleSynapseGroup
    {
        [ProtoMember(1)]
        public bool Default { get; set; }
        [ProtoMember(2)]
        public bool Northwood { get; set; }
        [ProtoMember(3)]
        public bool RemoteAdmin { get; set; }
        [ProtoMember(4)]
        public string Badge { get; set; }
        [ProtoMember(5)]
        public string Color { get; set; }
        [ProtoMember(6)]
        public bool Cover { get; set; }
        [ProtoMember(7)]
        public bool Hidden { get; set; }
        [ProtoMember(8)]
        public byte KickPower { get; set; }
        [ProtoMember(9)]
        public byte RequiredKickPower { get; set; }
        [ProtoMember(10)]
        public List<string> Permissions { get; set; }
        [ProtoMember(11)]
        public List<string> Members { get; set; }
    }
}
