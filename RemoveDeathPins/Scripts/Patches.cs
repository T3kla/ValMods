using UnityEngine;
using HarmonyLib;

namespace RemoveDeathPins.Patches
{
    [HarmonyPatch]
    public static class Patches
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Minimap), nameof(Minimap.GetClosestPin))]
        public static void GetClosestPinPatch(Minimap __instance, ref Minimap.PinData __result, Vector3 pos, float radius)
        {

            if (__result == null)
            {
                Minimap.PinData pin = __instance.m_pins.Find(x => x.m_type == Minimap.PinType.Death);
                if (pin != null)
                    if (Utils.DistanceXZ(pos, pin.m_pos) < radius)
                    {
                        PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
                        playerProfile.GetWorldData(ZNet.instance.GetWorldUID()).m_haveDeathPoint = false;
                        playerProfile.GetWorldData(ZNet.instance.GetWorldUID()).m_deathPoint = Vector3.zero;

                        __instance.m_pins.Remove(pin);
                        Debug.Log($"[RemoveDeathPins] Death pin removed!");
                    }
            }

        }

    }
}
