using HarmonyLib;
using UnityEngine;

namespace QoLPins
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZNet), nameof(ZNet.Awake))]
        public static void ZNet_Awake_Post()
            => Pins.UpdateColorLib();

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Minimap), nameof(Minimap.UpdatePins))]
        public static void Minimap_UpdatePins_Post()
            => Pins.UpdatePinsColor();

        public static bool InvIsEmpty;

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
        public static void Player_OnDeath_Pre(Player __instance)
            => InvIsEmpty = __instance.m_inventory.NrOfItems() < 1;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
        public static void Player_OnDeath_Post(Player __instance)
        {
            if (!InvIsEmpty || !Configs.DontPinWhenInvIsEmpty.Value)
                return;

            var p = __instance.transform.position;
            Main.Log.LogInfo($"Negating pin at '{p.ToString("F0")}' because inventory was empty\n");
            Pins.RemoveDeathPin(p, false);

            var pp = Game.instance.GetPlayerProfile();
            pp.GetWorldData(ZNet.instance.GetWorldUID()).m_haveDeathPoint = false;
            pp.GetWorldData(ZNet.instance.GetWorldUID()).m_deathPoint = Vector3.zero;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TombStone), nameof(TombStone.UpdateDespawn))]
        public static void TombStone_UpdateDespawn_Pre(TombStone __instance)
        {
            if (!Configs.RemoveAtRetrieval.Value
            || !__instance.m_nview.IsValid()
            || !__instance.m_nview.IsOwner()
            || __instance.m_container.IsInUse()
            || __instance.m_container.GetInventory().NrOfItems() > 0)
                return;

            Pins.RemoveDeathPin(__instance.transform.position);
        }
    }
}
