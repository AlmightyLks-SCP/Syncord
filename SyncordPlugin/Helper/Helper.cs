using SyncordInfo.ServerStats;
using SyncordPlugin.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncordPlugin.Helper
{
    public static class Helper
    {
        public static DateTime RoundToHour(this DateTime dateTime)
        {
            var updated = dateTime.AddMinutes(30);
            return new DateTime(updated.Year, updated.Month, updated.Day, updated.Hour, 0, 0, dateTime.Kind);
        }
        public static DateTime RoundToMinute(this DateTime dateTime)
        {
            var updated = dateTime.AddSeconds(30);
            return new DateTime(updated.Year, updated.Month, updated.Day, updated.Hour, updated.Minute, 0, dateTime.Kind);
        }
        public static DateTime RoundToSeconds(this DateTime dateTime)
        {
            var updated = dateTime.AddMilliseconds(500);
            return new DateTime(updated.Year, updated.Month, updated.Day, updated.Hour, updated.Minute, updated.Second, 0, dateTime.Kind);
        }

        public static IEnumerable<FpsStat> AverageFpsPerSecond(this List<FpsStat> fpsStats)
        {
            return fpsStats
                .GroupBy(_ => _.DateTime.RoundToSeconds())
                .Select(_ => new FpsStat() { DateTime = _.Key, Fps = (short)_.Average(e => e.Fps), IsIdle = _.Any(e => e.IsIdle) });
        }
        public static IEnumerable<FpsStat> AverageFpsPerSecond(this LimitedSizeStack<FpsStat> fpsStats)
        {
            return fpsStats
                .GroupBy(_ => _.DateTime.RoundToSeconds())
                .Select(_ => new FpsStat() { DateTime = _.Key, Fps = (short)_.Average(e => e.Fps), IsIdle = _.Any(e => e.IsIdle) });
        }
    }
}
