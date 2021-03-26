using UnityEngine;
using HarmonyLib;
using Areas.Containers;
using System.Linq;

namespace Areas.Patches
{

    [HarmonyPatch]
    public static class Patches
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Spawn))]
        public static bool SpawnSystem_Spawn(SpawnSystem __instance, SpawnSystem.SpawnData critter, Vector3 spawnPoint, bool eventSpawner)
        {

            GameObject newCritter = UnityEngine.Object.Instantiate<GameObject>(critter.m_prefab, spawnPoint, Quaternion.identity);
            BaseAI baseAI = newCritter.GetComponent<BaseAI>();

            if (baseAI == null) return false;
            if (critter.m_huntPlayer) baseAI.SetHuntPlayer(true);
            if (critter.m_maxLevel > 1)
                if (critter.m_levelUpMinCenterDistance <= 0f || spawnPoint.magnitude > critter.m_levelUpMinCenterDistance)
                {
                    Character character = newCritter.GetComponent<Character>();

                    if (character != null)
                    {
                        Area area = AreaHandler.GetArea(spawnPoint);
                        CritterCfg cfg = area.cfg.general;
                        if (area.cfg.specific != null)
                            if (area.cfg.specific.Count > 0)
                                area.cfg.specific.ForEach(a => { if (a.name == critter.m_prefab.name) { cfg = a; } });

                        if (cfg != null)
                        {
                            CritterHandler.ModifyCritter(__instance, critter, ref character, cfg);
                            Debug.Log($"[Areas] Spawning critter {critter.m_prefab.name} in area {area.id}");
                        }
                        else
                        {
                            int newLvl = critter.m_minLevel;
                            while (newLvl < critter.m_maxLevel && Random.Range(0f, 100f) <= __instance.m_levelupChance)
                                newLvl++;
                            character.SetLevel(newLvl > 1 ? newLvl : 1);
                        }
                    }
                }

            MonsterAI monsterAI = baseAI as MonsterAI;
            if (!monsterAI) return false;
            if (!critter.m_spawnAtDay)
                monsterAI.SetDespawnInDay(true);
            if (eventSpawner)
                monsterAI.SetEventCreature(true);

            return false;

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
