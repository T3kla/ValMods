using UnityEngine;
using HarmonyLib;

namespace DifficultyAreas.Patches
{

    [HarmonyPatch]
    public static class Patches
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Spawn))]
        public static void Spawn_Patch(SpawnSystem __instance, SpawnSystem.SpawnData critter, Vector3 spawnPoint, bool eventSpawner)
        {


        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
        public static void Player_OnSpawned(Player __instance)
        {

            if (AreaHandler.PlayerZoneLookupCorou == null)
            {
                Debug.Log($"[Areas] ZoneLookup start coroutine");
                AreaHandler.PlayerZoneLookupCorou = Main.Instance.StartCoroutine(AreaHandler.ZoneLookup(__instance));
            }

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnRespawn))]
        public static void Player_OnRespawn(Player __instance)
        {

            if (AreaHandler.PlayerZoneLookupCorou == null)
            {
                Debug.Log($"[Areas] ZoneLookup start coroutine");
                AreaHandler.PlayerZoneLookupCorou = Main.Instance.StartCoroutine(AreaHandler.ZoneLookup(__instance));
            }

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
        public static void Player_OnDeath(Player __instance)
        {

            if (AreaHandler.PlayerZoneLookupCorou != null)
            {
                Debug.Log($"[Areas] ZoneLookup stop coroutine");
                Main.Instance.StopCoroutine(AreaHandler.PlayerZoneLookupCorou);
                AreaHandler.PlayerZoneLookupCorou = null;
            }

        }

    }

}
