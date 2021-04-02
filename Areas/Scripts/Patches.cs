using HarmonyLib;

namespace Areas.Patches
{

    [HarmonyPatch]
    public static class Patches
    {

        // ----------------------------------------------------------------------------------------------------------------------------------- PLAYER
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
        public static void Player_OnSpawned(Player __instance)
        {

            if (AreaHandler.PlayerAreaLookupCorou != null) return;
            AreaHandler.PlayerAreaLookupCorou = Main.Instance.StartCoroutine(AreaHandler.ZoneLookup(__instance));

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnRespawn))]
        public static void Player_OnRespawn(Player __instance)
        {

            if (AreaHandler.PlayerAreaLookupCorou != null) return;
            AreaHandler.PlayerAreaLookupCorou = Main.Instance.StartCoroutine(AreaHandler.ZoneLookup(__instance));

        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
        public static void Player_OnDeath(Player __instance)
        {

            if (AreaHandler.PlayerAreaLookupCorou == null) return;
            Main.Instance.StopCoroutine(AreaHandler.PlayerAreaLookupCorou);
            AreaHandler.PlayerAreaLookupCorou = null;

        }


        // ----------------------------------------------------------------------------------------------------------------------------------- CRITTERS
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
        public static void Character_Awake_Post(Character __instance)
        {

            if (__instance.IsPlayer()) return;
            if (__instance == null) return;
            if (__instance.name != "modified") return;

            CritterHandler.Modify(__instance);

        }


        // ----------------------------------------------------------------------------------------------------------------------------------- SPAWNERS
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Awake))]
        public static void SpawnSystem_Awake_Post(SpawnSystem __instance)
        {

            if (__instance.name != "modified") return;

            SpawnerHandler.Modify_SS(__instance);

        }


    }

}
