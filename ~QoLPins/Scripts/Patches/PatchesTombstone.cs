using HarmonyLib;
using static Minimap;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(TombStone), nameof(TombStone.UpdateDespawn))]
    public static class TombStoneUpdateDespawn
    {
        public static void Postfix(TombStone __instance)
        {
            if (!Configs.RemoveAtRetrieval.Value
            || !__instance.m_nview.IsValid()
            || !__instance.m_nview.IsOwner()
            || __instance.m_container.IsInUse()
            || __instance.m_container.GetInventory().NrOfItems() > 0)
                return;

            PinAuto.RemovePin(__instance.transform.position, PinType.Death);
        }
    }
}
