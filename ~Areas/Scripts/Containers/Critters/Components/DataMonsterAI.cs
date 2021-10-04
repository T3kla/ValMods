namespace Areas.Containers
{
    // Updated for patch 0.202.19
    public class DataMonsterAI
    {
        public float? m_lastDespawnInDayCheck { get; set; }
        public float? m_lastEventCreatureCheck { get; set; }
        public float? m_alertRange { get; set; }
        public bool? m_fleeIfHurtWhenTargetCantBeReached { get; set; }
        public bool? m_fleeIfNotAlerted { get; set; }
        public float? m_fleeIfLowHealth { get; set; }
        public bool? m_circulateWhileCharging { get; set; }
        public bool? m_circulateWhileChargingFlying { get; set; }
        public bool? m_enableHuntPlayer { get; set; }
        public bool? m_attackPlayerObjects { get; set; }
        public float? m_interceptTimeMax { get; set; }
        public float? m_interceptTimeMin { get; set; }
        public float? m_maxChaseDistance { get; set; }
        public float? m_minAttackInterval { get; set; }
        public float? m_circleTargetInterval { get; set; }
        public float? m_circleTargetDuration { get; set; }
        public float? m_circleTargetDistance { get; set; }
        public bool? m_sleeping { get; set; }
        public bool? m_noiseWakeup { get; set; }
        public float? m_noiseRangeScale { get; set; }
        public float? m_wakeupRange { get; set; }
        public bool? m_avoidLand { get; set; }
        public float? m_consumeRange { get; set; }
        public float? m_consumeSearchRange { get; set; }
        public float? m_consumeSearchInterval { get; set; }
        public float? m_consumeSearchTimer { get; set; }
        public string m_aiStatus { get; set; }
        public bool? m_despawnInDay { get; set; }
        public bool? m_eventCreature { get; set; }
        public float[] m_lastKnownTargetPos { get; set; } // Vector3
        public bool? m_beenAtLastPos { get; set; }
        public float? m_timeSinceAttacking { get; set; }
        public float? m_timeSinceSensedTargetCreature { get; set; }
        public float? m_updateTargetTimer { get; set; }
        public float? m_updateWeaponTimer { get; set; }
        public float? m_lastAttackTime { get; set; }
        public float? m_interceptTime { get; set; }
        public float? m_pauseTimer { get; set; }
        public float? m_sleepTimer { get; set; }
        public float? m_unableToAttackTargetTimer { get; set; }
    }

}
