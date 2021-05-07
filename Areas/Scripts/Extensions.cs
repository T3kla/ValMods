using System;
using System.Collections.Generic;
using System.IO;
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

        public static string GetCleanName(this Character character)
        {
            return character.name.Replace("(Clone)", "").Trim();
        }

        public static bool SetEvolution(this Character character, Dictionary<int[], Stage> evolutions)
        {

            if (character.IsPlayer()) return false;

            ZNetView netView = character.GetComponent<ZNetView>();
            if (netView == null) return false;

            try
            {
                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();
                binFormatter.Serialize(mStream, evolutions);
                netView.GetZDO()?.Set($"Areas EvolutionSetter", mStream.ToArray());
                mStream.Close();
                return true;
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"Critter SetEvolutions failed for critter \"Lv.{character.GetCleanName()}\"\n{e.Message}\n{e.StackTrace}");
                return false;
            }

        }

        public static Dictionary<int[], Stage> GetEvolution(this Character character)
        {

            if (character.IsPlayer()) return null;

            ZNetView netView = character.GetComponent<ZNetView>();
            if (netView == null) return null;

            Byte[] array = netView.GetZDO().GetByteArray("Areas EvolutionSetter");
            if (array == null) return null;
            if (array.Length < 1) return null;

            try
            {
                var mStream = new MemoryStream();
                var binFormatter = new BinaryFormatter();
                mStream.Write(array, 0, array.Length);
                mStream.Position = 0;
                var evolutions = binFormatter.Deserialize(mStream) as Dictionary<int[], Stage>;
                mStream.Close();
                return evolutions;
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"Critter GetEvolution failed for critter \"Lv.{character.GetCleanName()}\"\n{e.Message}\n{e.StackTrace}");
                return null;
            }

        }

        public static bool SetDamageMulti(this Character character, float multi)
        {

            if (character.IsPlayer()) return false;

            ZNetView netView = character.GetComponent<ZNetView>();
            if (netView == null) return false;

            netView.GetZDO()?.Set("Areas CritterDmgMultiplier", multi);
            return true;

        }

        public static float GetDamageMulti(this Character character)
        {

            if (character.IsPlayer()) return 1f;

            ZNetView netView = character.GetComponent<ZNetView>();
            if (netView == null) return 1f;

            float multi = netView.GetZDO().GetFloat("Areas CritterDmgMultiplier", 1f);

            return multi;

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

}