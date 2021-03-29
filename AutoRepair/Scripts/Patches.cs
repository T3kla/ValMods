using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;

namespace AutoRepair.Patches
{
    [HarmonyPatch]
    public static class Patches
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.Interact))]
        public static void CraftingStation_Interact(CraftingStation __instance, Humanoid user, bool repeat)
        {

            var count = 0;

            var inv = InventoryGui.instance;
            while (inv.HaveRepairableItems())
            {
                count++;
                inv.RepairOneItem();
            }

            if (count > 0)
            {
                __instance.m_repairItemDoneEffects.Create(__instance.transform.position, Quaternion.identity, null, 1f);
                Player.m_localPlayer.Message(MessageHud.MessageType.Center, " ", 0, null);
                Debug.Log($"[AutoRepair] Repaired {count} objects!");
            }

        }

    }

}
