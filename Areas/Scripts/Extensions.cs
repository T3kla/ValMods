using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Areas.Containers;
using UnityEngine;

namespace Areas.NetCode
{
    public enum ZNetType { Local, Client, Server }

    public static class ZNetExtensions
    {

        public static ZNetType GetInstanceType(this ZNet znet)
        {

            bool isServer = znet.IsServer();
            bool isDedicated = znet.IsDedicated();

            if (isServer && !isDedicated) return ZNetType.Local;
            if (!isServer && !isDedicated) return ZNetType.Client;
            if (isServer && isDedicated) return ZNetType.Server;

            return ZNetType.Server;

        }

    }

}

namespace Areas
{

    public static class StringExtensions
    {

        public static int ToInt(this string str)
        {
            int y = 0;
            int.TryParse(str, out y);
            return y;
        }

    }

    public static class CharacterExtensions
    {

        public static string GetCleanName(this Character character) => character.name.Replace("(Clone)", "").Trim();

        public static bool SetCTDataStr(this Character character, string cfg, string ctName)
        {
            if (character.IsPlayer()) return false;

            ZDO zDO = character.GetComponent<ZNetView>()?.GetZDO();
            if (zDO == null) return false;

            zDO.Set("Areas CTDataStr", $"{cfg}={ctName}");

            return true;

        }

        public static bool GetCTDataStr(this Character character, out (string cfg, string name) pair)
        {

            pair = ("", "");

            if (character.IsPlayer()) return false;

            ZDO zDO = character.GetComponent<ZNetView>()?.GetZDO();
            if (zDO == null) return false;

            var str = zDO.GetString("Areas CTDataStr");
            if (string.IsNullOrEmpty(str)) return false;
            var split = str.Split('=');
            if (split.Length != 2) return false;

            pair = (split[0], split[1]);
            return true;

        }

        public static CTData GetCTData(this Character character)
        {

            if (!character.GetCTDataStr(out var pair)) return null;

            var data = (from a in Globals.CurrentData.CTMods
                        where a.Key == pair.cfg
                        from b in Globals.CurrentData.CTMods[pair.cfg]
                        where b.Key == pair.name
                        select b.Value).FirstOrDefault();

            return data;

        }

    }

    public static class SpawnerSystemExtensions
    {

        public static string GetCleanName(this SpawnSystem ss)
        {
            return ss.name.Replace("(Clone)", "").Trim() + ss.transform.position.ToString("F0");
        }

    }

    public static class CreatureSpawnerExtensions
    {

        public static string GetCleanName(this CreatureSpawner cs)
        {
            return cs.name.Replace("(Clone)", "").Trim() + cs.transform.position.ToString("F0");
        }

        public static void SetCustomCS(this CreatureSpawner cs, string ctName, CSData data)
        {

            ZDO zDO = cs.GetComponent<ZNetView>()?.GetZDO();
            if (zDO == null) return;

            try
            {
                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();
                binFormatter.Serialize(mStream, data);
                zDO.Set("Areas CustomCS", ctName);
                zDO.Set("Areas CustomCSData", mStream.ToArray());
                mStream.Close();
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"SetCustomCS failed!\n{e.Message}\n{e.StackTrace}");
            }

        }

        public static bool GetCustomCS(this CreatureSpawner cs, out string ctName, out CSData data)
        {

            ctName = "";
            data = null;

            ZDO zDO = cs.GetComponent<ZNetView>()?.GetZDO();
            if (zDO == null) return false;

            var str = zDO.GetString("Areas CustomCS");
            if (string.IsNullOrEmpty(str)) return false;

            Byte[] array = zDO.GetByteArray("Areas CustomCSData");
            if (array == null) return false;

            try
            {
                var mStream = new MemoryStream();
                var binFormatter = new BinaryFormatter();
                mStream.Write(array, 0, array.Length);
                mStream.Position = 0;
                ctName = str;
                data = (CSData)binFormatter.Deserialize(mStream);
                mStream.Close();
                return true;
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"GetCustomCS failed!\n{e.Message}\n{e.StackTrace}");
                return false;
            }

        }

        // public static bool SetEvolution(this Character character, Dictionary<int[], Stage> evolutions)
        // {

        //     if (character.IsPlayer()) return false;

        //     ZNetView netView = character.GetComponent<ZNetView>();
        //     if (netView == null) return false;

        //     try
        //     {
        //         var binFormatter = new BinaryFormatter();
        //         var mStream = new MemoryStream();
        //         binFormatter.Serialize(mStream, evolutions);
        //         netView.GetZDO()?.Set($"Areas EvolutionSetter", mStream.ToArray());
        //         mStream.Close();
        //         return true;
        //     }
        //     catch (Exception e)
        //     {
        //         Main.GLog.LogError($"Critter SetEvolutions failed for critter \"Lv.{character.GetCleanName()}\"\n{e.Message}\n{e.StackTrace}");
        //         return false;
        //     }

        // }

        // public static Dictionary<int[], Stage> GetEvolution(this Character character)
        // {

        //     if (character.IsPlayer()) return null;

        //     ZNetView netView = character.GetComponent<ZNetView>();
        //     if (netView == null) return null;

        //     Byte[] array = netView.GetZDO().GetByteArray("Areas EvolutionSetter");
        //     if (array == null) return null;
        //     if (array.Length < 1) return null;

        //     try
        //     {
        //         var mStream = new MemoryStream();
        //         var binFormatter = new BinaryFormatter();
        //         mStream.Write(array, 0, array.Length);
        //         mStream.Position = 0;
        //         var evolutions = binFormatter.Deserialize(mStream) as Dictionary<int[], Stage>;
        //         mStream.Close();
        //         return evolutions;
        //     }
        //     catch (Exception e)
        //     {
        //         Main.GLog.LogError($"Critter GetEvolution failed for critter \"Lv.{character.GetCleanName()}\"\n{e.Message}\n{e.StackTrace}");
        //         return null;
        //     }

        // }

    }

    public static class SpawnAreaExtensions
    {

        public static string GetCleanName(this SpawnArea sa)
        {
            return sa.name.Replace("(Clone)", "").Trim() + sa.transform.position.ToString("F0");
        }

    }

    public static class DungeonGeneratorExtensions
    {

        public static string GetCleanName(this DungeonGenerator dg)
        {
            return dg.name.Replace("(Clone)", "").Replace("(DungeonGenerator)", "").Trim() + dg.transform.position.ToString("F0");
        }

        public static long GetRegenAtSecond(this DungeonGenerator dg)
        {
            long regenAtSecond = dg.GetComponent<ZNetView>().GetZDO().GetLong("Areas RegenAtSecond");
            return regenAtSecond < 0L ? 0L : regenAtSecond;
        }

        public static long GetRegenRemainder(this DungeonGenerator dg)
        {
            long currentSecond = (long)ZNet.instance.GetTimeSeconds();
            long regenAtSecond = dg.GetComponent<ZNetView>().GetZDO().GetLong("Areas RegenAtSecond");
            long remainder = regenAtSecond - currentSecond;
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

        public static Vector2 ToXZ(this Vector3 v3) => new Vector2(v3.x, v3.z);

    }

}
