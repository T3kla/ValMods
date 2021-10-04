using System.Collections.Generic;

namespace Areas.Containers
{
    // Updated for patch 0.202.19
    public class DataSS
    {
        public enum Biome
        {
            None,
            Meadows,
            Swamp,
            Mountain = 4,
            BlackForest = 8,
            Plains = 16,
            AshLands = 32,
            DeepNorth = 64,
            Ocean = 256,
            Mistlands = 512,
            BiomesMax
        }

        public enum BiomeArea
        {
            Edge = 1,
            Median,
            Everything
        }

        public string m_name;
        public bool? m_enabled;
        public string m_biome;
        public string m_biomeArea;
        public int? m_maxSpawned;
        public float? m_spawnInterval;
        public float? m_spawnChance;
        public float? m_spawnDistance;
        public float? m_spawnRadiusMin;
        public float? m_spawnRadiusMax;
        public string m_requiredGlobalKey;
        public List<string> m_requiredEnvironments;
        public int? m_groupSizeMin;
        public int? m_groupSizeMax;
        public float? m_groupRadius;
        public bool? m_spawnAtNight;
        public bool? m_spawnAtDay;
        public float? m_minAltitude;
        public float? m_maxAltitude;
        public float? m_minTilt;
        public float? m_maxTilt;
        public bool? m_inForest;
        public bool? m_outsideForest;
        public float? m_minOceanDepth;
        public float? m_maxOceanDepth;
        public bool? m_huntPlayer;
        public float? m_groundOffset;
        public int? m_maxLevel;
        public int? m_minLevel;
        public float? m_levelUpMinCenterDistance;
        public bool? m_foldout;
    }
}
