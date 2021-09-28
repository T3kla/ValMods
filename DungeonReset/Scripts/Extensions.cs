using System;
using System.Collections.Generic;

namespace DungeonReset
{
    public static class DungeonExtensions
    {
        public static string GetCleanName(this DungeonGenerator dg)
            => $"{dg.name.Replace("(Clone)", "").Replace("(DungeonGenerator)", "").Trim()} {dg.transform.position:F0}";

        public static void SetLastReset(this DungeonGenerator dg, DateTimeOffset dt)
            => dg?.m_nview?.GetZDO()?.Set("Areas LastReset", dt.ToUnixTimeSeconds());

        public static DateTimeOffset GetLastReset(this DungeonGenerator dg)
            => DateTimeOffset.FromUnixTimeSeconds(dg?.m_nview?.GetZDO()?.GetLong("Areas LastReset") ?? 0L);

        public static float Remaining(this DungeonGenerator dungeon)
        {
            var since = (float)(DateTimeOffset.Now - dungeon.GetLastReset()).TotalSeconds;
            var interval = Global.Config.DungeonResetInterval.Value;
            return since > interval ? 0f : interval - since;
        }

        public static bool IsOverdue(this DungeonGenerator dungeon)
            => Remaining(dungeon) == 0f;
    }

    public static class DicExtensions
    {
        public static bool RemoveAll<T, T2>(this Dictionary<T, T2> dictionary, T key)
        {
            int count = 0;

            while (dictionary.ContainsKey(key))
                if (dictionary.Remove(key))
                    count++;

            return count > 0;
        }
    }
}
