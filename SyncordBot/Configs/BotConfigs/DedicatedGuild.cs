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
using SyncordBot.Models;
using System.Collections.Generic;

namespace SyncordBot.Configs.BotConfigs
{
    public class DedicatedGuild
    {
        public ulong GuildID { get; set; } = 0;
        public string SLFullAddress { get; set; } = "";
        public Dictionary<EventTypes, ulong> DedicatedChannels { get; set; } = new Dictionary<EventTypes, ulong>();
    }
}
