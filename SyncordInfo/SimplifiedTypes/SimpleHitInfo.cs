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

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public struct SimpleHitInfo
    {
        [ProtoMember(1)]
        public int Tool { get; set; }
        [ProtoMember(2)]
        public int Time { get; set; }
        [ProtoMember(3)]
        public string Attacker { get; set; }
        [ProtoMember(4)]
        public float Amount { get; set; }
        [ProtoMember(5)]
        public SimpleDamageType DamageType { get; set; }
    }
}
