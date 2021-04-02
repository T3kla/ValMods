using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Areas.Containers;

namespace Areas
{

    public static class SpawnerHandler
    {

        // something like this should be done so its faster to patch spawners that are in the same area
        // public static Dictionary<string, List<SpawnSystem.SpawnData>> SS_Dic = new Dictionary<string, List<SpawnSystem.SpawnData>>();

        public static void Modify_SS(SpawnSystem spawnSystem)
        {

            Area area = AreaHandler.GetArea(spawnSystem.transform.position);
            if (area == null) return;

            JObject cfg = Globals.SS_Data.Value<JObject>(area.cfg_id);
            if (cfg == null || !cfg.HasValues) return;

            spawnSystem.name = "modified";

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            foreach (var data in cfg)
            {
                SpawnSystem.SpawnData spawnData = spawnSystem.m_spawners.FirstOrDefault(a => a.m_prefab.name == data.Key);
                if (spawnData == null) continue;
                spawnData.m_groupSizeMin = data.Value.Value<int?>("group_size_min") ?? spawnData.m_groupSizeMin;
                spawnData.m_groupSizeMax = data.Value.Value<int?>("group_size_max") ?? spawnData.m_groupSizeMax;
                spawnData.m_groupRadius = data.Value.Value<int?>("group_radius") ?? spawnData.m_groupRadius;
                spawnData.m_spawnAtNight = data.Value.Value<bool?>("spawn_at_night") ?? spawnData.m_spawnAtNight;
                spawnData.m_spawnAtDay = data.Value.Value<bool?>("spawn_at_day") ?? spawnData.m_spawnAtDay;
                spawnData.m_minAltitude = data.Value.Value<int?>("min_altitude") ?? spawnData.m_minAltitude;
                spawnData.m_maxAltitude = data.Value.Value<int?>("group_size_min") ?? spawnData.m_maxAltitude;

            }

        }

    }

}
