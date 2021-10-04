using HarmonyLib;
using UnityEngine;
using static Minimap;

namespace QoLPins
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Minimap.AddPin))]
        public static void Minimap_AddPin_Post(Minimap __instance, ref PinData __result)
            => Pins.ColorPin(__result);

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
            Main.Log.LogInfo($"Not generating pin at '{p:F0}' because inventory was empty");
            Pins.RemoveDeathPin(p);

            var pp = Game.instance.GetPlayerProfile();
            pp.GetWorldData(ZNet.instance.GetWorldUID()).m_haveDeathPoint = false;
            pp.GetWorldData(ZNet.instance.GetWorldUID()).m_deathPoint = Vector3.zero;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(TombStone), nameof(TombStone.UpdateDespawn))]
        public static void TombStone_UpdateDespawn(TombStone __instance)
        {
            if (!Configs.RemoveAtRetrieval.Value
            || !__instance.m_nview.IsValid()
            || !__instance.m_nview.IsOwner()
            || __instance.m_container.IsInUse()
            || __instance.m_container.GetInventory().NrOfItems() > 0)
                return;

            var p = __instance.transform.position;
            Main.Log.LogInfo($"Attempting to remove pin from retrieved Tombstone at '{p:F0}'");
            Pins.RemoveDeathPin(p);
        }
    }
}
