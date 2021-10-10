using System;
using HarmonyLib;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(Destructible))]
    public static class PatchesDestructible
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Destructible.RPC_Damage))]
        public static void RPCDamage(Destructible __instance)
        {
            if (!Configs.EnableAutoPin.Value)
                return;

            if (__instance.name.Contains("Tin", StringComparison.OrdinalIgnoreCase))
            {
                if (PinAuto.AddPin(__instance.transform.position, PinAuto.TinData))
                    Main.Log.LogInfo($"Creating Tin pin at '{__instance.transform.position.ToString("F0")}'\n");
                return;
            }

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
        [HarmonyPatch(nameof(Destructible.Destroy))]
        public static void Destroy(Destructible __instance)
        {
            if (!Configs.EnableAutoPin.Value)
                return;

            if (__instance.name.Contains("Tin", StringComparison.OrdinalIgnoreCase))
            {
                if (PinAuto.RemovePin(__instance.transform.position, PinAuto.TinData))
                    Main.Log.LogInfo($"Removing Tin pin at '{__instance.transform.position.ToString("F0")}'\n");
                return;
            }
        }
    }
}
