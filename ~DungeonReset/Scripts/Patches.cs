using HarmonyLib;

namespace DungeonReset
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.Load))]
        public static void DungeonGenerator_Load_Post(DungeonGenerator __instance)
        {
            Main.Log.LogInfo($"Loaded dungeon '{__instance.GetCleanName()}' with theme {__instance.m_themes}\n");

            if (Configs.AllowedThemes.Value.Contains(__instance.m_themes.ToString()))
                Dungeons.OnDungeonLoad(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZNet), nameof(ZNet.OnDestroy))]
        public static void ZNet_OnDestroy_Post()
            => Dungeons.UnscheduleAll();
    }
}
