using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Areas.Containers;
using HarmonyLib;
using System.Reflection;

namespace Areas
{

    public static class CritterHandler
    {

        public static MethodInfo SetCritter_Info = AccessTools.Method(typeof(CritterHandler), nameof(CritterHandler.SetCritterHolder), new Type[] { typeof(GameObject) });
        public static void SetCritterHolder(GameObject critter) { CritterHolder = critter; }
        public static GameObject CritterHolder = null;

        public static void Modify_CT()
        {

            if (CritterHolder == null) return;
            Character critter = CritterHolder.GetComponent<Character>();
            if (critter == null) { CritterHolder = null; return; }

            string name = critter.name.Replace("(Clone)", "");

            Area area = AreaHandler.GetArea(critter.transform.position);
            if (area == null) { CritterHolder = null; return; }
            if (!Globals.CTMods.ContainsKey(area.cfg)) { CritterHolder = null; return; }
            if (!Globals.CTMods[area.cfg].ContainsKey(name)) { CritterHolder = null; return; }

            Debug.Log($"[Areas] Modifying Critter \"{name}\" in \"{critter.transform.position}\" in area \"{area.id}\" with config \"{area.cfg}\"");

            CTMods ctmods = Globals.CTMods[area.cfg][name];


            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            int level_fixed = ctmods.level_fixed ?? 0;
            int level_min = ctmods.level_min ?? 1;
            int level_max = ctmods.level_max ?? 3;
            int level_lvlUpChance = ctmods.level_lvlUpChance ?? 15;

            if (level_fixed > 0)
                critter.SetLevel(level_fixed);
            else if (level_min >= level_max)
                critter.SetLevel(level_max > 0 ? level_max : 1);
            else
            {
                int newLvl = level_min;
                int upChance = Mathf.Clamp(level_lvlUpChance, 0, 100);
                while (newLvl < level_max && UnityEngine.Random.Range(0f, 100f) <= upChance) newLvl++;
                critter.SetLevel(newLvl > 0 ? newLvl : 1);
            }

            critter.m_crouchSpeed = ctmods.crouch_speed ?? critter.m_crouchSpeed;
            critter.m_walkSpeed = ctmods.walk_speed ?? critter.m_walkSpeed;
            critter.m_speed = ctmods.speed ?? critter.m_speed;
            critter.m_turnSpeed = ctmods.turn_speed ?? critter.m_turnSpeed;
            critter.m_runSpeed = ctmods.run_speed ?? critter.m_runSpeed;
            critter.m_runTurnSpeed = ctmods.run_turn_speed ?? critter.m_runTurnSpeed;
            critter.m_flySlowSpeed = ctmods.fly_slow_speed ?? critter.m_flySlowSpeed;
            critter.m_flyFastSpeed = ctmods.fly_fast_speed ?? critter.m_flyFastSpeed;
            critter.m_flyTurnSpeed = ctmods.fly_turn_speed ?? critter.m_flyTurnSpeed;
            critter.m_acceleration = ctmods.acceleration ?? critter.m_acceleration;
            critter.m_jumpForce = ctmods.jump_force ?? critter.m_jumpForce;
            critter.m_jumpForceForward = ctmods.jump_force_forward ?? critter.m_jumpForceForward;
            critter.m_jumpForceTiredFactor = ctmods.jump_force_tired_factor ?? critter.m_jumpForceTiredFactor;
            critter.m_airControl = ctmods.air_control ?? critter.m_airControl;
            critter.m_canSwim = ctmods.can_swim ?? critter.m_canSwim;
            critter.m_swimDepth = ctmods.swim_depth ?? critter.m_swimDepth;
            critter.m_swimSpeed = ctmods.swim_speed ?? critter.m_swimSpeed;
            critter.m_swimTurnSpeed = ctmods.swim_turn_speed ?? critter.m_swimTurnSpeed;
            critter.m_swimAcceleration = ctmods.swim_acceleration ?? critter.m_swimAcceleration;
            critter.m_flying = ctmods.flying ?? critter.m_flying;
            critter.m_jumpStaminaUsage = ctmods.jump_stamina_usage ?? critter.m_jumpStaminaUsage;

            critter.m_tolerateWater = ctmods.tolerate_water ?? critter.m_tolerateWater;
            critter.m_tolerateFire = ctmods.tolerate_fire ?? critter.m_tolerateFire;
            critter.m_tolerateSmoke = ctmods.tolerate_smoke ?? critter.m_tolerateSmoke;
            critter.m_health = ctmods.health ?? critter.m_health;
            critter.m_staggerWhenBlocked = ctmods.stagger_when_blocked ?? critter.m_staggerWhenBlocked;
            critter.m_staggerDamageFactor = ctmods.stagger_damage_factor ?? critter.m_staggerDamageFactor;

            MonsterAI ai = critter.GetComponent<MonsterAI>();
            if (ai != null)
            {
                ai.m_alertRange = ctmods.alert_range ?? ai.m_alertRange;
                ai.m_fleeIfHurtWhenTargetCantBeReached = ctmods.flee_if_hurt_when_target_cant_be_reached ?? ai.m_fleeIfHurtWhenTargetCantBeReached;
                ai.m_fleeIfNotAlerted = ctmods.flee_if_not_alerted ?? ai.m_fleeIfNotAlerted;
                ai.m_fleeIfLowHealth = ctmods.flee_if_low_health ?? ai.m_fleeIfLowHealth;
                ai.m_circulateWhileCharging = ctmods.circulate_while_charging ?? ai.m_circulateWhileCharging;
                ai.m_circulateWhileChargingFlying = ctmods.circulate_while_charging_flying ?? ai.m_circulateWhileChargingFlying;
                ai.m_enableHuntPlayer = ctmods.enable_hunt_player ?? ai.m_enableHuntPlayer;
                ai.m_attackPlayerObjects = ctmods.attack_player_objects ?? ai.m_attackPlayerObjects;
                ai.m_attackPlayerObjectsWhenAlerted = ctmods.attack_player_objects_when_alerted ?? ai.m_attackPlayerObjectsWhenAlerted;
                ai.m_interceptTimeMax = ctmods.intercept_time_max ?? ai.m_interceptTimeMax;
                ai.m_interceptTimeMin = ctmods.intercept_time_min ?? ai.m_interceptTimeMin;
                ai.m_maxChaseDistance = ctmods.max_chase_distance ?? ai.m_maxChaseDistance;
                ai.m_minAttackInterval = ctmods.min_attack_interval ?? ai.m_minAttackInterval;
                ai.m_circleTargetInterval = ctmods.circle_target_interval ?? ai.m_circleTargetInterval;
                ai.m_circleTargetDuration = ctmods.circle_target_duration ?? ai.m_circleTargetDuration;
                ai.m_circleTargetDistance = ctmods.circle_target_distance ?? ai.m_circleTargetDistance;
                ai.m_sleeping = ctmods.sleeping ?? ai.m_sleeping;
                ai.m_noiseWakeup = ctmods.noise_wakeup ?? ai.m_noiseWakeup;
                ai.m_noiseRangeScale = ctmods.noise_range_scale ?? ai.m_noiseRangeScale;
                ai.m_wakeupRange = ctmods.wakeup_range ?? ai.m_wakeupRange;
                ai.m_avoidLand = ctmods.avoid_land ?? ai.m_avoidLand;
                ai.m_consumeRange = ctmods.consume_range ?? ai.m_consumeRange;
                ai.m_consumeSearchRange = ctmods.consume_search_range ?? ai.m_consumeSearchRange;
                ai.m_consumeSearchInterval = ctmods.consume_search_interval ?? ai.m_consumeSearchInterval;
                ai.m_consumeHeal = ctmods.consume_heal ?? ai.m_consumeHeal;
            }


            // ----------------------------------------------------------------------------------------------------------------------------------- EMPTY HOLDER
            CritterHolder = null;

        }

    }

}
