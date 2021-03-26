using UnityEngine;
using HarmonyLib;

namespace RemoveDeathPins.Patches
{
    [HarmonyPatch]
    public static class Patches
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Minimap), nameof(Minimap.GetClosestPin))]
        public static void Minimap_GetClosestPin(Minimap __instance, ref Minimap.PinData __result, Vector3 pos, float radius)
        {

            if (__result != null) return;

            Minimap.PinData pin = __instance.m_pins.Find(x => x.m_type == Minimap.PinType.Death);
            if (pin == null) return;
            if (Utils.DistanceXZ(pos, pin.m_pos) < radius)
                RemoveDeathPin();

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TombStone), nameof(TombStone.UpdateDespawn))]
        public static void TombStone_UpdateDespawn(TombStone __instance)
        {

            if (!__instance.m_nview.IsValid()) return;
            if (!__instance.m_nview.IsOwner()) return;
            if (__instance.m_container.IsInUse()) return;
            if (__instance.m_container.GetInventory().NrOfItems() > 0) return;
            RemoveDeathPin();

        }

        public static void RemoveDeathPin()
        {

            Minimap.PinData pin = Minimap.instance.m_pins.Find(x => x.m_type == Minimap.PinType.Death);
            if (pin == null) return;

            PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
            playerProfile.GetWorldData(ZNet.instance.GetWorldUID()).m_haveDeathPoint = false;
            playerProfile.GetWorldData(ZNet.instance.GetWorldUID()).m_deathPoint = Vector3.zero;
            Minimap.instance.m_pins.Remove(pin);

            Debug.Log($"[RemoveDeathPins] Death pin removed!");

        }

    }
}
