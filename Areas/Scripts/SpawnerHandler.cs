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

        private static Dictionary<string, List<SpawnSystem.SpawnData>> SS_Dic = new Dictionary<string, List<SpawnSystem.SpawnData>>();
        private static HashSet<Transform> SS_List = new HashSet<Transform>();
        private static HashSet<Transform> CS_List = new HashSet<Transform>();
        private static HashSet<Transform> SA_List = new HashSet<Transform>();

        public static void Modify_SS(SpawnSystem ss)
        {

            if (SS_List.Contains(ss.transform)) return;

            Area area = AreaHandler.GetArea(ss.transform.position);
            if (area == null) return;
            if (!Globals.SSMods.ContainsKey(area.cfg)) return;
            if (Globals.SSMods[area.cfg].Values.Count < 1) return;

            Dictionary<int, SSMods> ssmods = Globals.SSMods[area.cfg];
            Debug.Log($"[Areas] Modifying SpawnSystem in \"{ss.transform.position}\" in area \"{area.id}\" with config \"{area.cfg}\"");

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            if (SS_Dic.ContainsKey(area.cfg)) { ss.m_spawners = new List<SpawnSystem.SpawnData>(SS_Dic[area.cfg]); return; }

            foreach (var mod in ssmods)
            {
                if (mod.Key >= ss.m_spawners.Count) continue;
                SpawnSystem.SpawnData spawnData = ss.m_spawners[mod.Key];
                spawnData.m_groupSizeMin = mod.Value.group_size_min ?? spawnData.m_groupSizeMin;
                spawnData.m_groupSizeMax = mod.Value.group_size_max ?? spawnData.m_groupSizeMax;
                spawnData.m_groupRadius = mod.Value.group_radius ?? spawnData.m_groupRadius;
                spawnData.m_spawnAtNight = mod.Value.spawn_at_night ?? spawnData.m_spawnAtNight;
                spawnData.m_spawnAtDay = mod.Value.spawn_at_day ?? spawnData.m_spawnAtDay;
                spawnData.m_minAltitude = mod.Value.min_altitude ?? spawnData.m_minAltitude;
                spawnData.m_maxAltitude = mod.Value.group_size_min ?? spawnData.m_maxAltitude;
            }

            // ----------------------------------------------------------------------------------------------------------------------------------- EXIT
            if (!SS_Dic.ContainsKey(area.cfg)) SS_Dic.Add(area.cfg, new List<SpawnSystem.SpawnData>(ss.m_spawners));
            SS_List.Add(ss.transform);

        }

        public static void Modify_CS(CreatureSpawner cs)
        {

            if (CS_List.Contains(cs.transform)) return;

            string name = cs.name.Replace("(Clone)", "");

            Area area = AreaHandler.GetArea(cs.transform.position);
            if (area == null) return;
            if (!Globals.CSMods.ContainsKey(area.cfg)) return;
            if (!Globals.CSMods[area.cfg].ContainsKey(name)) return;

            CSMods csmod = Globals.CSMods[area.cfg][name];
            Debug.Log($"[Areas] Modifying CreatureSpawner \"{name}\" in \"{cs.transform.position}\" in area \"{area.id}\" with config \"{area.cfg}\"");

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            cs.m_respawnTimeMinuts = csmod.respawn_time_minutes ?? cs.m_respawnTimeMinuts;
            cs.m_triggerDistance = csmod.trigger_distance ?? cs.m_triggerDistance;
            cs.m_triggerNoise = csmod.trigger_noise ?? cs.m_triggerNoise;
            cs.m_spawnAtDay = csmod.spawn_at_day ?? cs.m_spawnAtDay;
            cs.m_spawnAtNight = csmod.spawn_at_night ?? cs.m_spawnAtNight;
            cs.m_requireSpawnArea = csmod.require_spawn_area ?? cs.m_requireSpawnArea;
            cs.m_spawnInPlayerBase = csmod.spawn_in_player_base ?? cs.m_spawnInPlayerBase;
            cs.m_setPatrolSpawnPoint = csmod.set_patrol_spawn_point ?? cs.m_setPatrolSpawnPoint;

            CS_List.Add(cs.transform);

        }

        public static void Modify_SA(SpawnArea sa)
        {

            if (SA_List.Contains(sa.transform)) return;

            string name = sa.name.Replace("(Clone)", "");

            Area area = AreaHandler.GetArea(sa.transform.position);
            if (area == null) return;
            if (!Globals.CSMods.ContainsKey(area.cfg)) return;
            if (!Globals.CSMods[area.cfg].ContainsKey(name)) return;

            SAMods samod = Globals.SAMods[area.cfg][name];
            Debug.Log($"[Areas] Modifying SpawnArea: \"{name}\" in \"{sa.transform.position}\" in area \"{area.id}\" with config \"{area.cfg}\"");

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            sa.m_spawnIntervalSec = samod.spawn_interval_sec ?? sa.m_spawnIntervalSec;
            sa.m_triggerDistance = samod.trigger_distance ?? sa.m_triggerDistance;
            sa.m_setPatrolSpawnPoint = samod.set_patrol_spawn_point ?? sa.m_setPatrolSpawnPoint;
            sa.m_spawnRadius = samod.spawn_radius ?? sa.m_spawnRadius;
            sa.m_nearRadius = samod.near_radius ?? sa.m_nearRadius;
            sa.m_farRadius = samod.far_radius ?? sa.m_farRadius;
            sa.m_maxNear = samod.max_near ?? sa.m_maxNear;
            sa.m_maxTotal = samod.max_total ?? sa.m_maxTotal;
            sa.m_onGroundOnly = samod.on_ground_only ?? sa.m_onGroundOnly;


            // ----------------------------------------------------------------------------------------------------------------------------------- EXIT
            SA_List.Add(sa.transform);

        }

        public static void Spawners_ResetData()
        {

            SS_List.Clear();
            CS_List.Clear();
            SA_List.Clear();

        }

    }

}
