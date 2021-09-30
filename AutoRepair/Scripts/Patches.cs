using HarmonyLib;

namespace AutoRepair.Patches
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CraftingStation), nameof(CraftingStation.Interact))]
        public static void CraftingStation_Interact(CraftingStation __instance, Humanoid user, bool repeat, bool alt)
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
                Player.m_localPlayer.Message(MessageHud.MessageType.Center, " ", 0, null);
                Main.Log.LogInfo($"[AutoRepair] Repaired {count} objects!\n");
            }
        }
    }
}
