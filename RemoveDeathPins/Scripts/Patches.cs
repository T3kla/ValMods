using UnityEngine;
using HarmonyLib;

namespace RemoveDeathPins
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
                __result = pin;

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
        public static void Player_OnDeath_Pre(Player __instance)
        {

            if (__instance.m_inventory.NrOfItems() > 0 || Globals.configAlwaysGen.Value == true)
                RDP.AddDeathPoint(__instance.GetCenterPoint());

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
        public static void Player_OnDeath_Post(Player __instance)
        {

            PlayerProfile playerProfile = Game.instance.GetPlayerProfile();
            playerProfile.GetWorldData(ZNet.instance.GetWorldUID()).m_haveDeathPoint = false;
            playerProfile.GetWorldData(ZNet.instance.GetWorldUID()).m_deathPoint = Vector3.zero;

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TombStone), nameof(TombStone.UpdateDespawn))]
        public static void TombStone_UpdateDespawn(TombStone __instance)
        {

            if (!__instance.m_nview.IsValid()) return;
            if (!__instance.m_nview.IsOwner()) return;
            if (__instance.m_container.IsInUse()) return;
            if (__instance.m_container.GetInventory().NrOfItems() > 0) return;

            Vector3 tombPoint = __instance.transform.position;
            RDP.RemoveDeathPin(tombPoint);

        }

    }

}
