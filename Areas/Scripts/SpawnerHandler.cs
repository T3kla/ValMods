using System.Collections.Generic;
using UnityEngine;
using Areas.Containers;
using Jotunn.Managers;
using System.Linq;

namespace Areas
{

    public static class SpawnerHandler
    {

        private static bool SS_DataDicFlag = false;
        private static Dictionary<string, List<SpawnSystem.SpawnData>> SS_DataDic = new Dictionary<string, List<SpawnSystem.SpawnData>>();
        private static HashSet<GameObject> CheckedSS = new HashSet<GameObject>();
        private static HashSet<GameObject> CheckedCS = new HashSet<GameObject>();
        private static HashSet<GameObject> CheckedSA = new HashSet<GameObject>();

        public static void ProcessCapturedSS(SpawnSystem ss)
        {

            if (SS_DataDicFlag) GenerateSSDataDic(ss);

            if (CheckedSS.Contains(ss.gameObject)) return;
            CheckedSS.Add(ss.gameObject);

            foreach (var area in AreaHandler.GetAreas(ss.transform.position.ToXZ()))
                if (SS_DataDic.ContainsKey(area.cfg))
                {
                    ss.m_spawners = SS_DataDic[area.cfg];
                    Main.GLog.LogInfo($"Modifying SpawnSystem \"{ss.GetCleanName()}\" in area \"{area.name}\" ");
                    return;
                }

        }

        public static void ProcessCapturedCS(CreatureSpawner cs)
        {

            if (CheckedCS.Contains(cs.gameObject)) return;
            CheckedCS.Add(cs.gameObject);

            string name = cs.GetCleanName(), area = "", cfg = "";
            CSData data = null;
            bool custom = false;

            if (cs.GetCustomCS(out var ctName, out data))
            {
                custom = true;
                var critter = PrefabManager.Instance.GetPrefab(ctName);
                if (critter == null) { GameObject.Destroy(cs.gameObject); return; }
                cs.m_creaturePrefab = critter;
            }
            else
            {
                data = AreaHandler.GetCSDataFromPos(name, cs.transform.position.ToXZ(), out area, out cfg);
            }

            if (data == null) return;
            Main.GLog.LogInfo($"Modifying {(custom ? "custom" : "")} CreatureSpawner \"{name}\" in area \"{area}\" with config \"{cfg}\"");

            ApplyCSData(cs, data);

        }

        public static void ApplyCSData(CreatureSpawner cs, CSData data)
        {
            if (cs == null || data == null) return;
            cs.m_respawnTimeMinuts = data.respawn_time_minutes.HasValue ? data.respawn_time_minutes.Value : cs.m_respawnTimeMinuts;
            cs.m_triggerDistance = data.trigger_distance.HasValue ? data.trigger_distance.Value : cs.m_triggerDistance;
            cs.m_triggerNoise = data.trigger_noise.HasValue ? data.trigger_noise.Value : cs.m_triggerNoise;
            cs.m_spawnAtDay = data.spawn_at_day.HasValue ? data.spawn_at_day.Value : cs.m_spawnAtDay;
            cs.m_spawnAtNight = data.spawn_at_night.HasValue ? data.spawn_at_night.Value : cs.m_spawnAtNight;
            cs.m_requireSpawnArea = data.require_spawn_area.HasValue ? data.require_spawn_area.Value : cs.m_requireSpawnArea;
            cs.m_spawnInPlayerBase = data.spawn_in_player_base.HasValue ? data.spawn_in_player_base.Value : cs.m_spawnInPlayerBase;
            cs.m_setPatrolSpawnPoint = data.set_patrol_spawn_point.HasValue ? data.set_patrol_spawn_point.Value : cs.m_setPatrolSpawnPoint;
        }

        public static void ProcessCapturedSA(SpawnArea sa)
        {

            if (CheckedSA.Contains(sa.gameObject)) return;
            CheckedSA.Add(sa.gameObject);

            string name = sa.GetCleanName();

            SAData data = AreaHandler.GetSADataFromPos(name, sa.transform.position.ToXZ(), out var area, out var cfg);
            if (data == null) return;

            Main.GLog.LogInfo($"Modifying SpawnArea \"{name}\" in area \"{area}\" with config \"{cfg}\"");

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            sa.m_spawnIntervalSec = data.spawn_interval_sec.HasValue ? data.spawn_interval_sec.Value : sa.m_spawnIntervalSec;
            sa.m_triggerDistance = data.trigger_distance.HasValue ? data.trigger_distance.Value : sa.m_triggerDistance;
            sa.m_setPatrolSpawnPoint = data.set_patrol_spawn_point.HasValue ? data.set_patrol_spawn_point.Value : sa.m_setPatrolSpawnPoint;
            sa.m_spawnRadius = data.spawn_radius.HasValue ? data.spawn_radius.Value : sa.m_spawnRadius;
            sa.m_nearRadius = data.near_radius.HasValue ? data.near_radius.Value : sa.m_nearRadius;
            sa.m_farRadius = data.far_radius.HasValue ? data.far_radius.Value : sa.m_farRadius;
            sa.m_maxNear = data.max_near.HasValue ? data.max_near.Value : sa.m_maxNear;
            sa.m_maxTotal = data.max_total.HasValue ? data.max_total.Value : sa.m_maxTotal;
            sa.m_onGroundOnly = data.on_ground_only.HasValue ? data.on_ground_only.Value : sa.m_onGroundOnly;

        }

        public static void OnDataLoaded()
        {
            SS_DataDicFlag = true;
        }

        public static void GenerateSSDataDic(SpawnSystem ss)
        {

            List<SpawnSystem.SpawnData> ModifySSList(ref List<SpawnSystem.SpawnData> list, Dictionary<int, SSData> mods)
            {
                foreach (var mod in mods)
                {
                    if (mod.Key < list.Count) continue;
                    SpawnSystem.SpawnData spawnData = list[mod.Key];
                    spawnData.m_groupSizeMin = mod.Value.group_size_min.HasValue ? mod.Value.group_size_min.Value : spawnData.m_groupSizeMin;
                    spawnData.m_groupSizeMax = mod.Value.group_size_max.HasValue ? mod.Value.group_size_max.Value : spawnData.m_groupSizeMax;
                    spawnData.m_groupRadius = mod.Value.group_radius.HasValue ? mod.Value.group_radius.Value : spawnData.m_groupRadius;
                    spawnData.m_spawnAtNight = mod.Value.spawn_at_night.HasValue ? mod.Value.spawn_at_night.Value : spawnData.m_spawnAtNight;
                    spawnData.m_spawnAtDay = mod.Value.spawn_at_day.HasValue ? mod.Value.spawn_at_day.Value : spawnData.m_spawnAtDay;
                    spawnData.m_minAltitude = mod.Value.min_altitude.HasValue ? mod.Value.min_altitude.Value : spawnData.m_minAltitude;
                    spawnData.m_maxAltitude = mod.Value.group_size_min.HasValue ? mod.Value.group_size_min.Value : spawnData.m_maxAltitude;
                }
                return list;
            }

            if (ss == null) return;

            foreach (var cfg in Globals.CurrentData.SSMods)
            {
                var newList = new List<SpawnSystem.SpawnData>(ss.m_spawners);
                ModifySSList(ref newList, cfg.Value);
                SS_DataDic.Add(cfg.Key, newList);
            }

            SS_DataDicFlag = false;

            Main.GLog.LogInfo($"SS_DataDic generated with count: \"{SS_DataDic.Count}\"");

        }

        public static void OnDataReset()
        {

            SS_DataDic.Clear();
            CheckedSS.Clear();
            CheckedCS.Clear();
            CheckedSA.Clear();

        }

    }

}
