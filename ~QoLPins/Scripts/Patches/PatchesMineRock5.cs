using System;
using HarmonyLib;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(MineRock5))]
    public static class PatchesMineRock5
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(MineRock5.RPC_Damage))]
        public static void RPCDamage(Destructible __instance)
        {
            if (!Configs.EnableAutoPin.Value)
                return;

            if (__instance.name.Contains("Copper", StringComparison.OrdinalIgnoreCase))
            {
                if (PinAuto.AddPin(__instance.transform.position, PinAuto.CopData))
                    Main.Log.LogInfo($"Creating Copper pin at '{__instance.transform.position.ToString("F0")}'\n");
                return;
            }

            if (__instance.name.Contains("Silver", StringComparison.OrdinalIgnoreCase))
            {
                if (PinAuto.AddPin(__instance.transform.position, PinAuto.SilData))
                    Main.Log.LogInfo($"Creating Silver pin at '{__instance.transform.position.ToString("F0")}'\n");
                return;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(MineRock5.AllDestroyed))]
        public static void AllDestroyed(MineRock5 __instance, bool __result)
        {
            if (!Configs.EnableAutoPin.Value || !__result)
                return;

            if (__instance.name.Contains("Copper", StringComparison.OrdinalIgnoreCase))
            {
                if (PinAuto.RemovePin(__instance.transform.position, PinAuto.CopData))
                    Main.Log.LogInfo($"Removing Copper pin at '{__instance.transform.position.ToString("F0")}'\n");
                return;
            }

            if (__instance.name.Contains("Silver", StringComparison.OrdinalIgnoreCase))
            {
                if (PinAuto.RemovePin(__instance.transform.position, PinAuto.SilData))
                    Main.Log.LogInfo($"Removing Silver pin at '{__instance.transform.position.ToString("F0")}'\n");
                return;
            }
        }
    }
}
