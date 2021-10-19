using HarmonyLib;
using UnityEngine;

namespace QoLPins.Patches
{
    [HarmonyPatch(typeof(Teleport), nameof(Teleport.Interact))]
    public static class TeleportInteract
    {
        public static void Postfix(Teleport __instance)
        {
            if (!Configs.EnableAutoPin.Value)
                return;

            LocationAwake.Locations.RemoveAll(a => a == null);
            DungeonGeneratorAwake.Dungeons.RemoveAll(a => a == null);

            foreach (var dun in DungeonGeneratorAwake.Dungeons)
                if (Utils.DistanceXZ(dun.transform.position, __instance.transform.position) < 10f)
                {
                    AddPin(dun.transform.position, dun.m_themes.ToString());
                    return;
                }

            foreach (var loc in LocationAwake.Locations)
                if (Utils.DistanceXZ(loc.transform.position, __instance.transform.position) < 10f)
                {
                    if (loc.name.Contains("troll", System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        AddPin(loc.transform.position, "TrollCave");
                        return;
                    }
                }

            static void AddPin(Vector3 position, string msg)
                => PinAuto.AddSafe(
                    position,
                    new AutoPin(
                        PinAuto.DunData.type,
                        PinAuto.DunData.name == "" ? msg : PinAuto.DunData.name));
        }
    }
}
