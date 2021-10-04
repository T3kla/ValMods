namespace Areas.Containers
{
    // Updated for patch 0.202.19
    public struct DataBaseAI
    {
        public float? m_lastMoveToWaterUpdate { get; set; }
        public bool? m_haveWaterPosition { get; set; }
        public float[] m_moveToWaterPosition { get; set; } // Vector3
        public float? m_fleeTargetUpdateTime { get; set; }
        public float[] m_fleeTarget { get; set; } // Vector3
        public float? m_nearFireTime { get; set; }
        public float? aroundPointUpdateTime { get; set; }
        public float[] arroundPointTarget { get; set; } // Vector3
        public float[] m_lastMovementCheck { get; set; } // Vector3
        public float? m_lastMoveTime { get; set; }
        public float? m_viewRange { get; set; }
        public float? m_viewAngle { get; set; }
        public float? m_hearRange { get; set; }
        public float? m_idleSoundInterval { get; set; }
        public float? m_idleSoundChance { get; set; }
        public string m_pathAgentType { get; set; }
        public float? m_moveMinAngle { get; set; }
        public bool? m_smoothMovement { get; set; }
        public bool? m_serpentMovement { get; set; }
        public float? m_serpentTurnRadius { get; set; }
        public float? m_jumpInterval { get; set; }
        public float? m_randomCircleInterval { get; set; }
        public float? m_randomMoveInterval { get; set; }
        public float? m_randomMoveRange { get; set; }
        public bool? m_randomFly { get; set; }
        public float? m_chanceToTakeoff { get; set; }
        public float? m_chanceToLand { get; set; }
        public float? m_groundDuration { get; set; }
        public float? m_airDuration { get; set; }
        public float? m_maxLandAltitude { get; set; }
        public float? m_flyAltitudeMin { get; set; }
        public float? m_flyAltitudeMax { get; set; }
        public float? m_takeoffTime { get; set; }
        public bool? m_avoidFire { get; set; }
        public bool? m_afraidOfFire { get; set; }
        public bool? m_avoidWater { get; set; }
        public string m_spawnMessage { get; set; }
        public string m_deathMessage { get; set; }
        public bool? m_patrol { get; set; }
        public float[] m_patrolPoint { get; set; } // Vector3
        public float? m_patrolPointUpdateTime { get; set; }
        public float? m_updateTimer { get; set; }
        public int m_solidRayMask { get; set; }
        public int m_viewBlockMask { get; set; }
        public int m_monsterTargetRayMask { get; set; }
        public float[] m_randomMoveTarget { get; set; } // Vector3
        public float? m_randomMoveUpdateTimer { get; set; }
        public bool? m_reachedRandomMoveTarget { get; set; }
        public float? m_jumpTimer { get; set; }
        public float? m_randomFlyTimer { get; set; }
        public float? m_regenTimer { get; set; }
        public bool? m_alerted { get; set; }
        public bool? m_huntPlayer { get; set; }
        public float[] m_spawnPoint { get; set; } // Vector3
        public float? m_getOutOfCornerTimer { get; set; }
        public float? m_getOutOfCornerAngle { get; set; }
        public float[] m_lastPosition { get; set; } // Vector3
        public float? m_stuckTimer { get; set; }
        public float? m_timeSinceHurt { get; set; }
        public float[] m_lastFindPathTarget { get; set; } // Vector3
        public float? m_lastFindPathTime { get; set; }
        public bool? m_lastFindPathResult { get; set; }
    }
}
