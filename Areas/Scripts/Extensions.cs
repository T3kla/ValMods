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

    public static class MaterialExtensions
    {

        public static void ToOpaqueMode(this Material material)
        {

            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;

        }

        public static void ToFadeMode(this Material material)
        {

            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        }

    }

    public static class CharacterExtensions
    {

        public static string GetCleanName(this Character character)
        {
            return character.name.Replace("(Clone)", "").Trim();

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

}