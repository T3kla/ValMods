using System.Collections.Generic;
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
                CraftingStation currentCraftingStation = Player.m_localPlayer.GetCurrentCraftingStation();
                InventoryGui inv = InventoryGui.instance;
                if (Player.m_localPlayer == null)
                    return;
                if (currentCraftingStation == null && !Player.m_localPlayer.NoCostCheat())
                    return;
                if (currentCraftingStation && !currentCraftingStation.CheckUsable(Player.m_localPlayer, false))
                    return;
                // -----

                List<ItemDrop.ItemData> itemList = new List<ItemDrop.ItemData>();
                Player.m_localPlayer.GetInventory().GetWornItems(itemList);

                List<string> repairedObjects = new List<string>();
                bool didRepair = false;

                foreach (ItemDrop.ItemData itemData in itemList)
                    if (inv.CanRepair(itemData))
                    {
                        itemData.m_durability = itemData.GetMaxDurability();
                        didRepair = true;
                        repairedObjects.Add(itemData.m_shared.m_name);
                    }

                if (didRepair)
                {
                    int count = repairedObjects.Count;
                    string msg = count > 1 ? $"Repaired {count} objects!" : $"Repaired 1 object!";
                    Debug.Log($"[AutoRepair] Repaired: {string.Join(" ,", repairedObjects)}");
                    Player.m_localPlayer.Message(MessageHud.MessageType.Center, msg, 0, null);
                }
            }
            catch
            { }
        }

    }

}
