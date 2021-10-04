namespace Areas.Containers
{
    // Updated for patch 0.202.19
    public struct DataCharacter
    {
        public enum Faction
        {
            Players,
            AnimalsVeg,
            ForestMonsters,
            Undead,
            Demon,
            MountainMonsters,
            SeaMonsters,
            PlainsMonsters,
            Boss
        }

        public enum GroundTiltType
        {
            None,
            Pitch,
            Full,
            PitchRaycast,
            FullRaycast
        }

        public float? m_underWorldCheckTimer { get; set; }
        public bool? m_groundContact { get; set; }
        public float[] m_groundContactPoint { get; set; } // Vector3
        public float[] m_groundContactNormal { get; set; } // Vector3
        public string m_name { get; set; }
        public string m_group { get; set; }
        public string m_faction { get; set; }
        public bool? m_boss { get; set; }
        public string m_bossEvent { get; set; }
        public string m_defeatSetGlobalKey { get; set; }
        public float? m_crouchSpeed { get; set; }
        public float? m_walkSpeed { get; set; }
        public float? m_speed { get; set; }
        public float? m_turnSpeed { get; set; }
        public float? m_runSpeed { get; set; }
        public float? m_runTurnSpeed { get; set; }
        public float? m_flySlowSpeed { get; set; }
        public float? m_flyFastSpeed { get; set; }
        public float? m_flyTurnSpeed { get; set; }
        public float? m_acceleration { get; set; }
        public float? m_jumpForce { get; set; }
        public float? m_jumpForceForward { get; set; }
        public float? m_jumpForceTiredFactor { get; set; }
        public float? m_airControl { get; set; }
        public bool? m_canSwim { get; set; }
        public float? m_swimDepth { get; set; }
        public float? m_swimSpeed { get; set; }
        public float? m_swimTurnSpeed { get; set; }
        public float? m_swimAcceleration { get; set; }
        public string m_groundTilt { get; set; }
        public bool? m_flying { get; set; }
        public float? m_jumpStaminaUsage { get; set; }
        public bool? m_tolerateWater { get; set; }
        public bool? m_tolerateFire { get; set; }
        public bool? m_tolerateSmoke { get; set; }
        public bool? m_tolerateTar { get; set; }
        public float? m_health { get; set; }
        public bool? m_staggerWhenBlocked { get; set; }
        public float? m_staggerDamageFactor { get; set; }
        public float? m_staggerDamage { get; set; }
        public float? m_backstabTime { get; set; }
        public float[] m_moveDir { get; set; } // Vector3
        public float[] m_lookDir { get; set; } // Vector3
        public float[] m_lookYaw { get; set; } // Quaternion
        public bool? m_run { get; set; }
        public bool? m_walk { get; set; }
        public bool? m_attack { get; set; }
        public bool? m_attackHold { get; set; }
        public bool? m_secondaryAttack { get; set; }
        public bool? m_secondaryAttackHold { get; set; }
        public bool? m_blocking { get; set; }
        public float? m_jumpTimer { get; set; }
        public float? m_lastAutoJumpTime { get; set; }
        public float? m_lastGroundTouch { get; set; }
        public float[] m_lastGroundNormal { get; set; } // Vector3
        public float[] m_lastGroundPoint { get; set; } // Vector3
        public float[] m_lastAttachPos { get; set; } // Vector3
        public float? m_maxAirAltitude { get; set; }
        public float? m_waterLevel { get; set; }
        public float? m_tarLevel { get; set; }
        public float? m_swimTimer { get; set; }
        public float? m_noiseRange { get; set; }
        public float? m_syncNoiseTimer { get; set; }
        public bool? m_tamed { get; set; }
        public float? m_lastTamedCheck { get; set; }
        public int? m_level { get; set; }
        public float[] m_currentVel { get; set; } // Vector3
        public float? m_currentTurnVel { get; set; }
        public float? m_currentTurnVelChange { get; set; }
        public float[] m_groundTiltNormal { get; set; } // Vector3
        public float[] m_pushForce { get; set; } // Vector3
        public float[] m_rootMotion { get; set; } // Vector3
        public float? m_slippage { get; set; }
        public bool? m_wallRunning { get; set; }
        public bool? m_sliding { get; set; }
        public bool? m_running { get; set; }
        public bool? m_walking { get; set; }
        public float[] m_originalLocalRef { get; set; }
        public bool? m_lodVisibletrue { get; set; }
        public float? m_smokeCheckTimer { get; set; }
    }
}
