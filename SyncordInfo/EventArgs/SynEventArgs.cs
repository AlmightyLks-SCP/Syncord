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
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordInfo.EventArgs
{
    [ProtoContract]
    [ProtoInclude(1001, typeof(PlayerJoinLeave))]
    [ProtoInclude(1002, typeof(RoundEnd))]
    [ProtoInclude(1003, typeof(PlayerDeath))]
    [ProtoInclude(1004, typeof(PlayerBan))]
    public class SynEventArgs
    {
        [ProtoMember(1)]
        public bool SameMachine { get; set; }
        [ProtoMember(2)]
        public string SLFullAddress { get; set; }
        [ProtoMember(3)]
        public DateTime Time { get; set; }
    }
}
