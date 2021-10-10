using HarmonyLib;
using UnityEngine;
using static Minimap;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
    public static class PlayerOnDeath
    {
        public static bool InvIsEmpty;

        public static void Prefix(Player __instance)
            => InvIsEmpty = __instance.m_inventory.NrOfItems() < 1;

        public static void Postfix(Player __instance)
        {
            if (!InvIsEmpty || !Configs.DontPinWhenInvIsEmpty.Value)
                return;

            var pos = __instance.transform.position;
            Main.Log.LogInfo($"Negating pin at '{pos.ToString("F0")}' because inventory was empty\n");
            PinAuto.RemovePin(pos, PinType.Death);

            var pp = Game.instance.GetPlayerProfile();
            pp.GetWorldData(ZNet.instance.GetWorldUID()).m_haveDeathPoint = false;
            pp.GetWorldData(ZNet.instance.GetWorldUID()).m_deathPoint = Vector3.zero;
        }
    }
}
