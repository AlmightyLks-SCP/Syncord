﻿using System;

namespace SyncordInfo.ServerStats
{
    public struct FpsStat
    {
        public bool IsIdle { get; set; }
        public DateTime DateTime { get; set; }
        public short Fps { get; set; }
    }
}
