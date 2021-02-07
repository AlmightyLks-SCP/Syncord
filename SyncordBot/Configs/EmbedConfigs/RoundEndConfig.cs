﻿/*
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncordBot.Configs.EmbedConfigs
{
    public struct RoundEndConfig
    {
        public bool ShowRoundLength { get; set; }
        public bool ShowTotalKills { get; set; }
        public bool ShowTotalScpKills { get; set; }
        public bool ShowTotalFragGrenadeKills { get; set; }
        public bool ShowTotalEscapedDClass { get; set; }
        public bool ShowTotalEscapedScientists { get; set; }
    }
}
