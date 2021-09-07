using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Areas.Containers;
using Areas.TYaml;
using UnityEngine;

namespace Areas
{

    public static class StringExtensions
    {

        public static int ToInt(this string str)
        {
            int.TryParse(str, out int y);
            return y;
        }

    }

    public static class GameObjectExtensions
    {

        public static string GetCleanName(this GameObject go) => go.name.Replace("(Clone)", "").Replace("(Evo)", "").Trim();
        public static string GetCleanNamePos(this GameObject go) => $"{go.name.Replace("(Clone)", "").Replace("(Evo)", "").Trim()}-{go.transform.position.ToString("F0")}";
        public static string GetCleanPos(this GameObject go) => go.transform.position.ToString("F0");

    }

    public static class CharacterExtensions
    {

        public static void SetCfg(this Character character, string cfg) => character?.m_nview?.GetZDO()?.Set("Areas Cfg", cfg);
        public static string GetCfg(this Character character) => character?.m_nview?.GetZDO()?.GetString("Areas Cfg");

        public static void SetVariant(this Character character, string variant) => character?.m_nview?.GetZDO()?.Set("Areas Variant", variant);
        public static string GetVariant(this Character character) => character?.m_nview?.GetZDO()?.GetString("Areas Variant");

        public static void SetDamageMulti(this Character character, float damageMulti) => character?.m_nview?.GetZDO()?.Set("Areas DamageMulti", damageMulti);
        public static float? GetDamageMulti(this Character character) => character?.m_nview?.GetZDO()?.GetFloat("Areas DamageMulti", 1f);

        public static void SetCustomHealthPercentage(this Character character, float percent) => character?.m_nview?.GetZDO()?.Set("Areas Health Percentage", percent);
        public static float? GetCustomHealthPercentage(this Character character) => character?.m_nview?.GetZDO()?.GetFloat("Areas Health Percentage");

        public static CTData GetCtData(this Character character)
        {
            string cfg = character?.GetCfg();
            if (string.IsNullOrEmpty(cfg)) return null;

            string _variant = character?.GetVariant();
            string _ctName = !string.IsNullOrEmpty(_variant) ? _variant : character?.m_nview?.gameObject.GetCleanName();
            if (string.IsNullOrEmpty(_ctName)) return null;

            var data = (from a in Globals.CurrentData.CTMods
                        where a.Key == cfg
                        from b in Globals.CurrentData.CTMods[cfg]
                        where b.Key == _ctName
                        select b.Value).FirstOrDefault();

            return data;
        }

    }

    public static class CreatureSpawnerExtensions
    {

        public static void SetCtName(this CreatureSpawner cs, string ctName) => cs?.m_nview?.GetZDO()?.Set("Areas CustomCS CtName", ctName);
        public static string GetCtName(this CreatureSpawner cs) => cs?.m_nview?.GetZDO()?.GetString("Areas CustomCS CtName");

        public static void SetData(this CreatureSpawner cs, CSData data) => cs?.m_nview?.GetZDO()?.Set("Areas CustomCS Data", Serialization.Serialize(data));
        public static CSData GetData(this CreatureSpawner cs) => Serialization.Deserialize<CSData>(cs?.m_nview?.GetZDO()?.GetString("Areas CustomCS Data"));

    }

    public static class DungeonGeneratorExtensions
    {

        public static string GetCleanName(this DungeonGenerator dg) => dg.name.Replace("(Clone)", "").Replace("(DungeonGenerator)", "").Trim() + dg.transform.position.ToString("F0");

        public static void SetRegenAtSecond(this DungeonGenerator dg, long value) => dg?.m_nview?.GetZDO()?.Set("Areas RegenAtSecond", value);
        public static long GetRegenAtSecond(this DungeonGenerator dg)
        {
            long value = dg?.m_nview?.GetZDO()?.GetLong("Areas RegenAtSecond") ?? 0L;
            return value < 0L ? 0L : value;
        }

        public static long GetRegenRemainder(this DungeonGenerator dg)
        {
            long remainder = dg.GetRegenAtSecond() - (long)ZNet.instance.GetTimeSeconds();
            return remainder < 0L ? 0L : remainder;
        }

    }

    public static class DicExtensions
    {

        public static int RemoveAll<T, T2>(this Dictionary<T, T2> dictionary, IEnumerable<T> keys)
        {
            int count = 0;

            foreach (var key in keys)
                if (dictionary.Remove(key))
                    count++;

            return count;
        }

    }

    public static class FloatArrayExtensions
    {

        public static Vector2 ToVector2(this float[] ar)
        {

            Vector2 v = Vector2.zero;

            try
            {
                v = new Vector2(ar[0], ar[1]);
                return v;
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"Failed Conversion \"float[]\" to \"Vector2\"\n{e.Message}\n{e.StackTrace}");
                return v;
            }

        }

    }

    public static class Vector3Extensions
    {

        public static Vector2 ToXZ(this Vector3 v3) => new(v3.x, v3.z);

    }

}
