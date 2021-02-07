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

namespace SyncordInfo.SimplifiedTypes
{
    [ProtoContract]
    public sealed class SimpleCustomRole
    {
        [ProtoMember(1)]
        public (string Name, int Id) Role { get; set; }
        [ProtoMember(2)]
        public (string Name, int Id) Team { get; set; }
        public SimpleCustomRole()
        {
            Role = (string.Empty, 0);
            Team = (string.Empty, 0);
        }
    }
}
