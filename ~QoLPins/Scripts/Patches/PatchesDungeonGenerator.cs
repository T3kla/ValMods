using System.Collections.Generic;
using HarmonyLib;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.Awake))]
    public static class DungeonGeneratorAwake
    {
        public static List<DungeonGenerator> Dungeons = new();

        public static void Postfix(DungeonGenerator __instance)
            => Dungeons.Add(__instance);
    }
}
