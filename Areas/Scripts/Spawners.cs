using System.Collections.Generic;
using Areas.Containers;
using Jotunn.Managers;
using UnityEngine;
using SpawnData = SpawnSystem.SpawnData;

namespace Areas
{

    public static class Spawners
    {

        private static bool SS_DataDicFlag = false;
        private static readonly Dictionary<string, List<SpawnData>> SS_DataDic = new();

        public static void OnDataLoaded() => SS_DataDicFlag = true;
        public static void OnDataReset() => SS_DataDic.Clear();

        public static void Awake()
        {
            Main.OnDataLoaded += OnDataLoaded;
            Main.OnDataReset += OnDataReset;
        }

        public static void ProcessCapturedSS(SpawnSystem ss)
        {
            if (SS_DataDicFlag) GenerateSSDataDic(ss);

            foreach (var area in Areas.GetAreas(ss.transform.position.ToXZ()))
                if (SS_DataDic.ContainsKey(area.cfg))
                {
                    ss.m_spawners = SS_DataDic[area.cfg];
                    Main.Log.LogInfo($"Modifying SpawnSystem \"{ss.gameObject.GetCleanNamePos()}\" in area \"{area.name}\"\n");
                    return;
                }
        }

        public static void ProcessCapturedCS(CreatureSpawner cs)
        {
            string name = "";
            string area = "";
            string cfg = "";

            string ctName = cs.GetCtName();
            CSData data = cs.GetData();

            if (!string.IsNullOrEmpty(ctName))
            {
                if (data == null) { ZNetScene.instance.Destroy(cs.gameObject); return; }
                GameObject critter = PrefabManager.Instance.GetPrefab(Variants.FindOriginal(ctName) ?? ctName);
                if (critter == null) { ZNetScene.instance.Destroy(cs.gameObject); return; }
                cs.m_creaturePrefab = critter;
            }

            data ??= Areas.GetCSDataFromPos(cs.gameObject.GetCleanName(), cs.transform.position.ToXZ(), out area, out cfg);

            if (data == null) return;

            Main.Log.LogInfo($"Modifying {(string.IsNullOrEmpty(ctName) ? "custom" : "")} CreatureSpawner \"{name}\" in area \"{area}\" with config \"{cfg}\"\n");

            ApplyCSData(cs, data);
        }

        public static void ApplyCSData(in CreatureSpawner cs, in CSData data)
        {
            cs.m_respawnTimeMinuts = data.respawn_time_minutes ?? cs.m_respawnTimeMinuts;
            cs.m_triggerDistance = data.trigger_distance ?? cs.m_triggerDistance;
            cs.m_triggerNoise = data.trigger_noise ?? cs.m_triggerNoise;
            cs.m_spawnAtDay = data.spawn_at_day ?? cs.m_spawnAtDay;
            cs.m_spawnAtNight = data.spawn_at_night ?? cs.m_spawnAtNight;
            cs.m_requireSpawnArea = data.require_spawn_area ?? cs.m_requireSpawnArea;
            cs.m_spawnInPlayerBase = data.spawn_in_player_base ?? cs.m_spawnInPlayerBase;
            cs.m_setPatrolSpawnPoint = data.set_patrol_spawn_point ?? cs.m_setPatrolSpawnPoint;
        }

        public static void ProcessCapturedSA(SpawnArea sa)
        {
            string name = sa.gameObject.GetCleanName();

            SAData data = Areas.GetSADataFromPos(name, sa.transform.position.ToXZ(), out var area, out var cfg);
            if (data == null) return;

            Main.Log.LogInfo($"Modifying SpawnArea \"{name}\" in area \"{area}\" with config \"{cfg}\"\n");

            sa.m_spawnIntervalSec = data.spawn_interval_sec ?? sa.m_spawnIntervalSec;
            sa.m_triggerDistance = data.trigger_distance ?? sa.m_triggerDistance;
            sa.m_setPatrolSpawnPoint = data.set_patrol_spawn_point ?? sa.m_setPatrolSpawnPoint;
            sa.m_spawnRadius = data.spawn_radius ?? sa.m_spawnRadius;
            sa.m_nearRadius = data.near_radius ?? sa.m_nearRadius;
            sa.m_farRadius = data.far_radius ?? sa.m_farRadius;
            sa.m_maxNear = data.max_near ?? sa.m_maxNear;
            sa.m_maxTotal = data.max_total ?? sa.m_maxTotal;
            sa.m_onGroundOnly = data.on_ground_only ?? sa.m_onGroundOnly;
        }

        public static void GenerateSSDataDic(SpawnSystem ss)
        {
            static List<SpawnData> ModifySSList(ref List<SpawnData> list, Dictionary<int, SSData> mods)
            {
                foreach (var mod in mods)
                {
                    if (mod.Key < list.Count) continue;
                    SpawnData spawnData = list[mod.Key];
                    spawnData.m_groupSizeMin = mod.Value.group_size_min ?? spawnData.m_groupSizeMin;
                    spawnData.m_groupSizeMax = mod.Value.group_size_max ?? spawnData.m_groupSizeMax;
                    spawnData.m_groupRadius = mod.Value.group_radius ?? spawnData.m_groupRadius;
                    spawnData.m_spawnAtNight = mod.Value.spawn_at_night ?? spawnData.m_spawnAtNight;
                    spawnData.m_spawnAtDay = mod.Value.spawn_at_day ?? spawnData.m_spawnAtDay;
                    spawnData.m_minAltitude = mod.Value.min_altitude ?? spawnData.m_minAltitude;
                    spawnData.m_maxAltitude = mod.Value.group_size_min ?? spawnData.m_maxAltitude;
                }
                return list;
            }

            if (ss == null) return;

            foreach (var cfg in Global.CurrentData.SSMods)
            {
                var newList = new List<SpawnData>(ss.m_spawners);
                ModifySSList(ref newList, cfg.Value);
                SS_DataDic.Add(cfg.Key, newList);
            }

            SS_DataDicFlag = false;

            Main.Log.LogInfo($"SS_DataDic generated with count: \"{SS_DataDic.Count}\"\n");
        }

    }

}
