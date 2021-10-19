using System.Collections.Generic;
using HarmonyLib;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(Location), nameof(Location.Awake))]
    public static class LocationAwake
    {
        public static List<Location> Locations = new();

        public static void Postfix(Location __instance)
            => Locations.Add(__instance);
    }
}
