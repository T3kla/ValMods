using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Areas.Containers;
using HarmonyLib;
using System.Reflection;
using System.Linq;

namespace Areas
{

    public static class CritterHandler
    {

        public static Dictionary<(string, string), Material> CT_MatDic = new Dictionary<(string, string), Material>();
        public static List<Transform> CheckedCritters = new List<Transform>();

        public static MethodInfo SetCritter_Info = AccessTools.Method(typeof(CritterHandler), nameof(CritterHandler.CT_SetHolder), new Type[] { typeof(GameObject) });
        public static void CT_SetHolder(GameObject critter) { CritterHolder = critter; }
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

            Main.Log.LogInfo($"Modifying Critter \"{name}\" in \"{critter.transform.position}\" in area \"{area.id}\" with config \"{area.cfg}\"");

            CTMods ctmod = Globals.CTMods[area.cfg][name];


            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            if (ctmod.level_fixed.HasValue)
                Assign_CT_Level(critter, ctmod, "fixed");
            else if (ctmod.level_min.HasValue && ctmod.level_max.HasValue)
                Assign_CT_Level(critter, ctmod, "minmax");

            if (ctmod.size.HasValue)
                Assign_CT_Size(critter, ctmod.size.Value);

            if (ctmod.color != null)
            {
                Assign_CT_Color(name, critter, ctmod.color);
                ZNetView znView = critter.GetComponent<ZNetView>();
                if (znView != null) { znView.GetZDO().Set("Critter Paint", ctmod.color); }
            }

            critter.m_crouchSpeed = ctmod.crouch_speed.HasValue ? ctmod.crouch_speed.Value : critter.m_crouchSpeed;
            critter.m_walkSpeed = ctmod.walk_speed.HasValue ? ctmod.walk_speed.Value : critter.m_walkSpeed;
            critter.m_speed = ctmod.speed.HasValue ? ctmod.speed.Value : critter.m_speed;
            critter.m_turnSpeed = ctmod.turn_speed.HasValue ? ctmod.turn_speed.Value : critter.m_turnSpeed;
            critter.m_runSpeed = ctmod.run_speed.HasValue ? ctmod.run_speed.Value : critter.m_runSpeed;
            critter.m_runTurnSpeed = ctmod.run_turn_speed.HasValue ? ctmod.run_turn_speed.Value : critter.m_runTurnSpeed;
            critter.m_flySlowSpeed = ctmod.fly_slow_speed.HasValue ? ctmod.fly_slow_speed.Value : critter.m_flySlowSpeed;
            critter.m_flyFastSpeed = ctmod.fly_fast_speed.HasValue ? ctmod.fly_fast_speed.Value : critter.m_flyFastSpeed;
            critter.m_flyTurnSpeed = ctmod.fly_turn_speed.HasValue ? ctmod.fly_turn_speed.Value : critter.m_flyTurnSpeed;
            critter.m_acceleration = ctmod.acceleration.HasValue ? ctmod.acceleration.Value : critter.m_acceleration;
            critter.m_jumpForce = ctmod.jump_force.HasValue ? ctmod.jump_force.Value : critter.m_jumpForce;
            critter.m_jumpForceForward = ctmod.jump_force_forward.HasValue ? ctmod.jump_force_forward.Value : critter.m_jumpForceForward;
            critter.m_jumpForceTiredFactor = ctmod.jump_force_tired_factor.HasValue ? ctmod.jump_force_tired_factor.Value : critter.m_jumpForceTiredFactor;
            critter.m_airControl = ctmod.air_control.HasValue ? ctmod.air_control.Value : critter.m_airControl;
            critter.m_canSwim = ctmod.can_swim.HasValue ? ctmod.can_swim.Value : critter.m_canSwim;
            critter.m_swimDepth = ctmod.swim_depth.HasValue ? ctmod.swim_depth.Value : critter.m_swimDepth;
            critter.m_swimSpeed = ctmod.swim_speed.HasValue ? ctmod.swim_speed.Value : critter.m_swimSpeed;
            critter.m_swimTurnSpeed = ctmod.swim_turn_speed.HasValue ? ctmod.swim_turn_speed.Value : critter.m_swimTurnSpeed;
            critter.m_swimAcceleration = ctmod.swim_acceleration.HasValue ? ctmod.swim_acceleration.Value : critter.m_swimAcceleration;
            critter.m_flying = ctmod.flying.HasValue ? ctmod.flying.Value : critter.m_flying;
            critter.m_jumpStaminaUsage = ctmod.jump_stamina_usage.HasValue ? ctmod.jump_stamina_usage.Value : critter.m_jumpStaminaUsage;
            critter.m_tolerateWater = ctmod.tolerate_water.HasValue ? ctmod.tolerate_water.Value : critter.m_tolerateWater;
            critter.m_tolerateFire = ctmod.tolerate_fire.HasValue ? ctmod.tolerate_fire.Value : critter.m_tolerateFire;
            critter.m_tolerateSmoke = ctmod.tolerate_smoke.HasValue ? ctmod.tolerate_smoke.Value : critter.m_tolerateSmoke;
            critter.m_health = ctmod.health.HasValue ? ctmod.health.Value : critter.m_health;
            critter.m_staggerWhenBlocked = ctmod.stagger_when_blocked.HasValue ? ctmod.stagger_when_blocked.Value : critter.m_staggerWhenBlocked;
            critter.m_staggerDamageFactor = ctmod.stagger_damage_factor.HasValue ? ctmod.stagger_damage_factor.Value : critter.m_staggerDamageFactor;

            MonsterAI ai = critter.GetComponent<MonsterAI>();
            if (ai != null)
            {
                // Base AI
                ai.m_viewRange = ctmod.view_range.HasValue ? ctmod.view_range.Value : ai.m_viewRange;
                ai.m_viewAngle = ctmod.view_angle.HasValue ? ctmod.view_angle.Value : ai.m_viewAngle;
                ai.m_hearRange = ctmod.hear_range.HasValue ? ctmod.hear_range.Value : ai.m_hearRange;
                ai.m_idleSoundInterval = ctmod.idle_sound_interval.HasValue ? ctmod.idle_sound_interval.Value : ai.m_idleSoundInterval;
                ai.m_idleSoundChance = ctmod.idle_sound_chance.HasValue ? ctmod.idle_sound_chance.Value : ai.m_idleSoundChance;
                ai.m_moveMinAngle = ctmod.move_min_angle.HasValue ? ctmod.move_min_angle.Value : ai.m_moveMinAngle;
                ai.m_smoothMovement = ctmod.smooth_movement.HasValue ? ctmod.smooth_movement.Value : ai.m_smoothMovement;
                ai.m_serpentMovement = ctmod.serpent_movement.HasValue ? ctmod.serpent_movement.Value : ai.m_serpentMovement;
                ai.m_serpentTurnRadius = ctmod.serpent_turn_radius.HasValue ? ctmod.serpent_turn_radius.Value : ai.m_serpentTurnRadius;
                ai.m_jumpInterval = ctmod.jump_interval.HasValue ? ctmod.jump_interval.Value : ai.m_jumpInterval;
                ai.m_randomCircleInterval = ctmod.random_circle_interval.HasValue ? ctmod.random_circle_interval.Value : ai.m_randomCircleInterval;
                ai.m_randomMoveInterval = ctmod.random_move_interval.HasValue ? ctmod.random_move_interval.Value : ai.m_randomMoveInterval;
                ai.m_randomMoveRange = ctmod.random_move_range.HasValue ? ctmod.random_move_range.Value : ai.m_randomMoveRange;
                ai.m_randomFly = ctmod.random_fly.HasValue ? ctmod.random_fly.Value : ai.m_randomFly;
                ai.m_chanceToTakeoff = ctmod.chance_to_takeoff.HasValue ? ctmod.chance_to_takeoff.Value : ai.m_chanceToTakeoff;
                ai.m_chanceToLand = ctmod.chance_to_land.HasValue ? ctmod.chance_to_land.Value : ai.m_chanceToLand;
                ai.m_groundDuration = ctmod.ground_duration.HasValue ? ctmod.ground_duration.Value : ai.m_groundDuration;
                ai.m_airDuration = ctmod.air_duration.HasValue ? ctmod.air_duration.Value : ai.m_airDuration;
                ai.m_maxLandAltitude = ctmod.max_land_altitude.HasValue ? ctmod.max_land_altitude.Value : ai.m_maxLandAltitude;
                ai.m_flyAltitudeMin = ctmod.fly_altitude_min.HasValue ? ctmod.fly_altitude_min.Value : ai.m_flyAltitudeMin;
                ai.m_flyAltitudeMax = ctmod.fly_altitude_max.HasValue ? ctmod.fly_altitude_max.Value : ai.m_flyAltitudeMax;
                ai.m_takeoffTime = ctmod.takeoff_time.HasValue ? ctmod.takeoff_time.Value : ai.m_takeoffTime;
                ai.m_avoidFire = ctmod.avoid_fire.HasValue ? ctmod.avoid_fire.Value : ai.m_avoidFire;
                ai.m_afraidOfFire = ctmod.afraid_of_fire.HasValue ? ctmod.afraid_of_fire.Value : ai.m_afraidOfFire;
                ai.m_avoidWater = ctmod.avoid_water.HasValue ? ctmod.avoid_water.Value : ai.m_avoidWater;
                ai.m_spawnMessage = ctmod.spawn_message ?? ai.m_spawnMessage;
                ai.m_deathMessage = ctmod.death_message ?? ai.m_deathMessage;
                if (!string.IsNullOrEmpty(ctmod.path_agent_type))
                    switch (ctmod.path_agent_type)
                    {
                        case "Humanoid": ai.m_pathAgentType = Pathfinding.AgentType.Humanoid; break;
                        case "TrollSize": ai.m_pathAgentType = Pathfinding.AgentType.TrollSize; break;
                        case "HugeSize": ai.m_pathAgentType = Pathfinding.AgentType.HugeSize; break;
                        case "HorseSize": ai.m_pathAgentType = Pathfinding.AgentType.HorseSize; break;
                        case "HumanoidNoSwim": ai.m_pathAgentType = Pathfinding.AgentType.HumanoidNoSwim; break;
                        case "HumanoidAvoidWater": ai.m_pathAgentType = Pathfinding.AgentType.HumanoidAvoidWater; break;
                        case "Fish": ai.m_pathAgentType = Pathfinding.AgentType.Fish; break;
                        case "Wolf": ai.m_pathAgentType = Pathfinding.AgentType.Wolf; break;
                        case "BigFish": ai.m_pathAgentType = Pathfinding.AgentType.BigFish; break;
                        case "GoblinBruteSize": ai.m_pathAgentType = Pathfinding.AgentType.GoblinBruteSize; break;
                        case "HumanoidBigNoSwim": ai.m_pathAgentType = Pathfinding.AgentType.HumanoidBigNoSwim; break;
                        default: break;
                    }

                // Monster AI
                ai.m_alertRange = ctmod.alert_range.HasValue ? ctmod.alert_range.Value : ai.m_alertRange;
                ai.m_fleeIfHurtWhenTargetCantBeReached = ctmod.flee_if_hurt_when_target_cant_be_reached.HasValue ? ctmod.flee_if_hurt_when_target_cant_be_reached.Value : ai.m_fleeIfHurtWhenTargetCantBeReached;
                ai.m_fleeIfNotAlerted = ctmod.flee_if_not_alerted.HasValue ? ctmod.flee_if_not_alerted.Value : ai.m_fleeIfNotAlerted;
                ai.m_fleeIfLowHealth = ctmod.flee_if_low_health.HasValue ? ctmod.flee_if_low_health.Value : ai.m_fleeIfLowHealth;
                ai.m_circulateWhileCharging = ctmod.circulate_while_charging.HasValue ? ctmod.circulate_while_charging.Value : ai.m_circulateWhileCharging;
                ai.m_circulateWhileChargingFlying = ctmod.circulate_while_charging_flying.HasValue ? ctmod.circulate_while_charging_flying.Value : ai.m_circulateWhileChargingFlying;
                ai.m_enableHuntPlayer = ctmod.enable_hunt_player.HasValue ? ctmod.enable_hunt_player.Value : ai.m_enableHuntPlayer;
                ai.m_attackPlayerObjects = ctmod.attack_player_objects.HasValue ? ctmod.attack_player_objects.Value : ai.m_attackPlayerObjects;
                ai.m_attackPlayerObjectsWhenAlerted = ctmod.attack_player_objects_when_alerted.HasValue ? ctmod.attack_player_objects_when_alerted.Value : ai.m_attackPlayerObjectsWhenAlerted;
                ai.m_interceptTimeMax = ctmod.intercept_time_max.HasValue ? ctmod.intercept_time_max.Value : ai.m_interceptTimeMax;
                ai.m_interceptTimeMin = ctmod.intercept_time_min.HasValue ? ctmod.intercept_time_min.Value : ai.m_interceptTimeMin;
                ai.m_maxChaseDistance = ctmod.max_chase_distance.HasValue ? ctmod.max_chase_distance.Value : ai.m_maxChaseDistance;
                ai.m_minAttackInterval = ctmod.min_attack_interval.HasValue ? ctmod.min_attack_interval.Value : ai.m_minAttackInterval;
                ai.m_circleTargetInterval = ctmod.circle_target_interval.HasValue ? ctmod.circle_target_interval.Value : ai.m_circleTargetInterval;
                ai.m_circleTargetDuration = ctmod.circle_target_duration.HasValue ? ctmod.circle_target_duration.Value : ai.m_circleTargetDuration;
                ai.m_circleTargetDistance = ctmod.circle_target_distance.HasValue ? ctmod.circle_target_distance.Value : ai.m_circleTargetDistance;
                ai.m_sleeping = ctmod.sleeping.HasValue ? ctmod.sleeping.Value : ai.m_sleeping;
                ai.m_noiseWakeup = ctmod.noise_wakeup.HasValue ? ctmod.noise_wakeup.Value : ai.m_noiseWakeup;
                ai.m_noiseRangeScale = ctmod.noise_range_scale.HasValue ? ctmod.noise_range_scale.Value : ai.m_noiseRangeScale;
                ai.m_wakeupRange = ctmod.wakeup_range.HasValue ? ctmod.wakeup_range.Value : ai.m_wakeupRange;
                ai.m_avoidLand = ctmod.avoid_land.HasValue ? ctmod.avoid_land.Value : ai.m_avoidLand;
                ai.m_consumeRange = ctmod.consume_range.HasValue ? ctmod.consume_range.Value : ai.m_consumeRange;
                ai.m_consumeSearchRange = ctmod.consume_search_range.HasValue ? ctmod.consume_search_range.Value : ai.m_consumeSearchRange;
                ai.m_consumeSearchInterval = ctmod.consume_search_interval.HasValue ? ctmod.consume_search_interval.Value : ai.m_consumeSearchInterval;
                ai.m_consumeHeal = ctmod.consume_heal.HasValue ? ctmod.consume_heal.Value : ai.m_consumeHeal;
            }


            // ----------------------------------------------------------------------------------------------------------------------------------- EMPTY HOLDER
            CheckedCritters.Add(critter.transform);
            CritterHolder = null;

        }

        private static void Assign_CT_Level(Character critter, CTMods mods, string route)
        {

            int ByBoss(Dictionary<string, bool> boss, Dictionary<string, int> perBoss)
            {
                if (perBoss == null) return 0;
                int result = 0;
                foreach (var item in perBoss)
                    switch (item.Key)
                    {
                        case "Eikthyr": result += boss["eikthyr"] ? Mathf.Clamp(item.Value, 0, 100) : 0; break;
                        case "The Elder": result += boss["gdking"] ? Mathf.Clamp(item.Value, 0, 100) : 0; break;
                        case "Bonemass": result += boss["bonemass"] ? Mathf.Clamp(item.Value, 0, 100) : 0; break;
                        case "Moder": result += boss["dragon"] ? Mathf.Clamp(item.Value, 0, 100) : 0; break;
                        case "Yagluth": result += boss["goblinking"] ? Mathf.Clamp(item.Value, 0, 100) : 0; break;
                        default: break;
                    }
                return result;
            }

            int days = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

            Dictionary<string, bool> bosses = new Dictionary<string, bool>
            {
                {"eikthyr",ZoneSystem.instance.GetGlobalKey("defeated_eikthyr")},
                {"gdking",ZoneSystem.instance.GetGlobalKey("defeated_gdking")},
                {"bonemass",ZoneSystem.instance.GetGlobalKey("defeated_bonemass")},
                {"dragon",ZoneSystem.instance.GetGlobalKey("defeated_dragon")},
                {"goblinking",ZoneSystem.instance.GetGlobalKey("defeated_goblinking")},
            };

            // Fixed level route
            if (route == "fixed")
            {

                int lvl = (int)mods.level_fixed;
                lvl += mods.level_fixed_AddByDay.HasValue ? Mathf.FloorToInt(days / (float)mods.level_fixed_AddByDay) : 0;
                lvl += mods.level_fixed_AddByBoss != null ? ByBoss(bosses, mods.level_fixed_AddByBoss) : 0;
                critter.SetLevel(Mathf.Clamp(lvl, 0, 100));

            }
            // MinMax level route
            else if (route == "minmax")
            {

                int lvlMin = (int)mods.level_min;
                int lvlMax = (int)mods.level_max;
                int lvlCha = (int)mods.level_lvlUpChance;

                lvlMin += mods.level_min_AddByDay.HasValue ? Mathf.FloorToInt(days / (float)mods.level_min_AddByDay) : 0;
                lvlMax += mods.level_max_AddByDay.HasValue ? Mathf.FloorToInt(days / (float)mods.level_max_AddByDay) : 0;
                lvlCha += mods.level_lvlUpChance_AddByDay.HasValue ? Mathf.FloorToInt(days / (float)mods.level_lvlUpChance_AddByDay) : 0;

                lvlMin += mods.level_min_AddByBoss != null ? ByBoss(bosses, mods.level_min_AddByBoss) : 0;
                lvlMax += mods.level_max_AddByBoss != null ? ByBoss(bosses, mods.level_max_AddByBoss) : 0;
                lvlCha += mods.level_lvlUpChance_AddByBoss != null ? ByBoss(bosses, mods.level_lvlUpChance_AddByBoss) : 0;

                lvlCha = Mathf.Clamp(lvlCha, 0, 100);

                while (lvlMin < lvlMax && UnityEngine.Random.Range(0f, 100f) <= lvlCha) lvlMin++;
                critter.SetLevel(Mathf.Clamp(lvlMin, 1, 100));

            }

        }

        private static void Assign_CT_Size(Character critter, float modifier)
        {

            critter.transform.localScale *= Mathf.Clamp(modifier, 0.1f, 50f);
            Physics.SyncTransforms();

        }

        public static void Assign_CT_Color(string name, Character critter, string hexColorStr)
        {

            Material newMat;
            if (!CT_MatDic.TryGetValue((name, hexColorStr), out newMat)) return;

            Renderer renderer = null;

            LevelEffects levelEffects = critter.GetComponentInChildren<LevelEffects>();
            if (levelEffects != null)
            {
                if (levelEffects.m_mainRender != null)
                    renderer = levelEffects.m_mainRender;
            }
            else
            {
                renderer = critter.GetComponentInChildren<SkinnedMeshRenderer>();
            }
            if (renderer == null) return;

            List<Material> newArray = new List<Material>(renderer.materials);
            newArray.Add(newMat);
            renderer.materials = newArray.ToArray();

        }

        public static void Generate_CTMatDic()
        {

            foreach (var cfg in Globals.CTMods)
                foreach (var critter in cfg.Value)
                {
                    string name = critter.Key;
                    if (string.IsNullOrEmpty(critter.Value.color)) continue;
                    Color color;
                    if (!ColorUtility.TryParseHtmlString(critter.Value.color, out color))
                    {
                        critter.Value.color = null;
                        continue;
                    }

                    Material newMat = new Material(Shader.Find("Standard"));
                    newMat.color = color;
                    newMat.SetFloat("_Smoothness", 0f);
                    newMat.ToFadeMode();

                    CT_MatDic.Add((name, critter.Value.color), newMat);
                }

            Main.Log.LogInfo($"CT_MatDic generated with count: \"{CT_MatDic.Count}\"");

        }

        public static void ResetData()
        {

            CritterHolder = null;
            CT_MatDic.Clear();
            CheckedCritters.Clear();

        }

    }

}
