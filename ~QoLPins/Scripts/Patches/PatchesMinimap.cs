using HarmonyLib;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(Minimap), nameof(Minimap.UpdatePins))]
    public static class MinimapUpdatePins
    {
        public static void Postfix()
            => PinColor.UpdatePinsColor();
    }
}
