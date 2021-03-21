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
        public static void InteractPatch(CraftingStation __instance, Humanoid user, bool repeat)
        {
            try
            {
                // ----- Checks from HaveRepairableItems(), which are normally called to activate/deactivate repair button
                var currentCraftingStation = Player.m_localPlayer.GetCurrentCraftingStation();
                var inv = InventoryGui.instance;
                
                if (Player.m_localPlayer == null) return;
                if (currentCraftingStation == null && !Player.m_localPlayer.NoCostCheat()) return;
                if (currentCraftingStation && !currentCraftingStation.CheckUsable(Player.m_localPlayer, false)) return;
                // -----

                var itemList = new List<ItemDrop.ItemData>();
                Player.m_localPlayer.GetInventory().GetWornItems(itemList);

                var repairedObjects = new List<string>();
                var didRepair = false;

                foreach (var itemData in itemList.Where(itemData => inv.CanRepair(itemData)))
                {
                    itemData.m_durability = itemData.GetMaxDurability();
                    didRepair = true;
                    repairedObjects.Add(itemData.m_shared.m_name);
                }

                if (!didRepair) return;
                
                var count = repairedObjects.Count;
                var msg = count > 1 ? $"Repaired {count} objects!" : $"Repaired 1 object!";
                Debug.Log($"[AutoRepair] Repaired: {string.Join(" ,", repairedObjects)}");
                Player.m_localPlayer.Message(MessageHud.MessageType.Center, msg, 0, null);
            }
            catch
            {
                // ignored
            }
        }

    }

}
