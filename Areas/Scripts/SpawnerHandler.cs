using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Areas.Containers;
using UnityEngine;

namespace Areas
{

    public static class SpawnerHandler
    {

        private static Dictionary<string, List<SpawnSystem.SpawnData>> SS_DataDic = new Dictionary<string, List<SpawnSystem.SpawnData>>();
        private static bool SS_DataDicFlag = false;

        private static HashSet<Transform> SS_List = new HashSet<Transform>();
        private static HashSet<Transform> CS_List = new HashSet<Transform>();
        private static HashSet<Transform> SA_List = new HashSet<Transform>();

        public static void Modify_SS(SpawnSystem ss)
        {

            if (SS_DataDicFlag) Generate_SSDataDic(ss);

            if (SS_List.Contains(ss.transform)) return;

            Area area = AreaHandler.GetArea(ss.transform.position);
            if (area == null) return;
            if (!Globals.SSMods.ContainsKey(area.cfg)) return;
            if (Globals.SSMods[area.cfg].Values.Count < 1) return;

            Dictionary<int, SSMods> ssmods = Globals.SSMods[area.cfg];
            Main.GLog.LogInfo($"Modifying SpawnSystem \"{ss.GetCleanName()}\"");

            if (SS_DataDic.ContainsKey(area.cfg)) { ss.m_spawners = SS_DataDic[area.cfg]; return; }
            SS_List.Add(ss.transform);

        }

        public static void Modify_CS(CreatureSpawner cs)
        {

            if (CS_List.Contains(cs.transform)) return;

            string name = cs.GetCleanName();

            Area area = AreaHandler.GetArea(cs.transform.position);
            if (area == null) return;
            if (!Globals.CSMods.ContainsKey(area.cfg)) return;
            if (!Globals.CSMods[area.cfg].ContainsKey(name)) return;

            CSMods mod = Globals.CSMods[area.cfg][name];
            Main.GLog.LogInfo($"Modifying CreatureSpawner \"{name}\" in area \"{area.id}\"");

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            cs.m_respawnTimeMinuts = mod.respawn_time_minutes.HasValue ? mod.respawn_time_minutes.Value : cs.m_respawnTimeMinuts;
            cs.m_triggerDistance = mod.trigger_distance.HasValue ? mod.trigger_distance.Value : cs.m_triggerDistance;
            cs.m_triggerNoise = mod.trigger_noise.HasValue ? mod.trigger_noise.Value : cs.m_triggerNoise;
            cs.m_spawnAtDay = mod.spawn_at_day.HasValue ? mod.spawn_at_day.Value : cs.m_spawnAtDay;
            cs.m_spawnAtNight = mod.spawn_at_night.HasValue ? mod.spawn_at_night.Value : cs.m_spawnAtNight;
            cs.m_requireSpawnArea = mod.require_spawn_area.HasValue ? mod.require_spawn_area.Value : cs.m_requireSpawnArea;
            cs.m_spawnInPlayerBase = mod.spawn_in_player_base.HasValue ? mod.spawn_in_player_base.Value : cs.m_spawnInPlayerBase;
            cs.m_setPatrolSpawnPoint = mod.set_patrol_spawn_point.HasValue ? mod.set_patrol_spawn_point.Value : cs.m_setPatrolSpawnPoint;

            CS_List.Add(cs.transform);

        }

        public static void Modify_SA(SpawnArea sa)
        {

            if (SA_List.Contains(sa.transform)) return;

            string name = sa.GetCleanName();

            Area area = AreaHandler.GetArea(sa.transform.position);
            if (area == null) return;
            if (!Globals.CSMods.ContainsKey(area.cfg)) return;
            if (!Globals.CSMods[area.cfg].ContainsKey(name)) return;

            SAMods mod = Globals.SAMods[area.cfg][name];
            Main.GLog.LogInfo($"Modifying SpawnArea: \"{name}\" in area \"{area.id}\"");

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            sa.m_spawnIntervalSec = mod.spawn_interval_sec.HasValue ? mod.spawn_interval_sec.Value : sa.m_spawnIntervalSec;
            sa.m_triggerDistance = mod.trigger_distance.HasValue ? mod.trigger_distance.Value : sa.m_triggerDistance;
            sa.m_setPatrolSpawnPoint = mod.set_patrol_spawn_point.HasValue ? mod.set_patrol_spawn_point.Value : sa.m_setPatrolSpawnPoint;
            sa.m_spawnRadius = mod.spawn_radius.HasValue ? mod.spawn_radius.Value : sa.m_spawnRadius;
            sa.m_nearRadius = mod.near_radius.HasValue ? mod.near_radius.Value : sa.m_nearRadius;
            sa.m_farRadius = mod.far_radius.HasValue ? mod.far_radius.Value : sa.m_farRadius;
            sa.m_maxNear = mod.max_near.HasValue ? mod.max_near.Value : sa.m_maxNear;
            sa.m_maxTotal = mod.max_total.HasValue ? mod.max_total.Value : sa.m_maxTotal;
            sa.m_onGroundOnly = mod.on_ground_only.HasValue ? mod.on_ground_only.Value : sa.m_onGroundOnly;


            // ----------------------------------------------------------------------------------------------------------------------------------- EXIT
            SA_List.Add(sa.transform);

        }

        public static void Set_SSDataDicFlag()
        {
            SS_DataDicFlag = true;
        }

        public static void Generate_SSDataDic(SpawnSystem ss)
        {

            List<SpawnSystem.SpawnData> ModifySSList(ref List<SpawnSystem.SpawnData> list, Dictionary<int, SSMods> mods)
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

            foreach (var cfg in Globals.SSMods)
            {
                List<SpawnSystem.SpawnData> newList = new List<SpawnSystem.SpawnData>(ss.m_spawners);
                ModifySSList(ref newList, cfg.Value);
                SS_DataDic.Add(cfg.Key, newList);
            }

            SS_DataDicFlag = false;

            Main.GLog.LogInfo($"SS_DataDic generated with count: \"{SS_DataDic.Count}\"");

        }

        public static void ResetData()
        {

            SS_DataDic.Clear();
            SS_List.Clear();
            CS_List.Clear();
            SA_List.Clear();

        }

    }

}
