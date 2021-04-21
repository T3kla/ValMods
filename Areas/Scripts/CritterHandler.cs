using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Areas.Containers;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Areas
{

    public static class CritterHandler
    {

        public static Dictionary<(string, string), Material> CT_MatDic = new Dictionary<(string, string), Material>();
        public static HashSet<Transform> CheckedCritters = new HashSet<Transform>();

        public static MethodInfo CT_SetHolderInfo = AccessTools.Method(typeof(CritterHandler), nameof(CritterHandler.CT_SetHolder), new Type[] { typeof(GameObject) });
        public static void CT_SetHolder(GameObject critter) { CT_Holder = critter; }
        public static GameObject CT_Holder = null;

        public static Dictionary<string, bool> KilledBosses = new Dictionary<string, bool>();

        public static void Modify_CT()
        {

            if (CT_Holder == null) return;
            Character critter = CT_Holder.GetComponent<Character>();
            if (critter == null) { CT_Holder = null; return; }

            string name = critter.GetCleanName();

            Area area = AreaHandler.GetArea(critter.transform.position);
            if (area == null) { CT_Holder = null; return; }
            if (!Globals.CTMods.ContainsKey(area.cfg)) { CT_Holder = null; return; }
            if (!Globals.CTMods[area.cfg].ContainsKey(name)) { CT_Holder = null; return; }

            CTMods mods = Globals.CTMods[area.cfg][name];

            // ----------------------------------------------------------------------------------------------------------------------------------- MODS
            if (mods.scale_by_boss != null) if (mods.scale_by_boss.Count > 0) RefreshKilledBosses();

            critter.m_crouchSpeed = mods.crouch_speed.HasValue ? mods.crouch_speed.Value : critter.m_crouchSpeed;
            critter.m_walkSpeed = mods.walk_speed.HasValue ? mods.walk_speed.Value : critter.m_walkSpeed;
            critter.m_speed = mods.speed.HasValue ? mods.speed.Value : critter.m_speed;
            critter.m_turnSpeed = mods.turn_speed.HasValue ? mods.turn_speed.Value : critter.m_turnSpeed;
            critter.m_runSpeed = mods.run_speed.HasValue ? mods.run_speed.Value : critter.m_runSpeed;
            critter.m_runTurnSpeed = mods.run_turn_speed.HasValue ? mods.run_turn_speed.Value : critter.m_runTurnSpeed;
            critter.m_flySlowSpeed = mods.fly_slow_speed.HasValue ? mods.fly_slow_speed.Value : critter.m_flySlowSpeed;
            critter.m_flyFastSpeed = mods.fly_fast_speed.HasValue ? mods.fly_fast_speed.Value : critter.m_flyFastSpeed;
            critter.m_flyTurnSpeed = mods.fly_turn_speed.HasValue ? mods.fly_turn_speed.Value : critter.m_flyTurnSpeed;
            critter.m_acceleration = mods.acceleration.HasValue ? mods.acceleration.Value : critter.m_acceleration;
            critter.m_jumpForce = mods.jump_force.HasValue ? mods.jump_force.Value : critter.m_jumpForce;
            critter.m_jumpForceForward = mods.jump_force_forward.HasValue ? mods.jump_force_forward.Value : critter.m_jumpForceForward;
            critter.m_jumpForceTiredFactor = mods.jump_force_tired_factor.HasValue ? mods.jump_force_tired_factor.Value : critter.m_jumpForceTiredFactor;
            critter.m_airControl = mods.air_control.HasValue ? mods.air_control.Value : critter.m_airControl;
            critter.m_canSwim = mods.can_swim.HasValue ? mods.can_swim.Value : critter.m_canSwim;
            critter.m_swimDepth = mods.swim_depth.HasValue ? mods.swim_depth.Value : critter.m_swimDepth;
            critter.m_swimSpeed = mods.swim_speed.HasValue ? mods.swim_speed.Value : critter.m_swimSpeed;
            critter.m_swimTurnSpeed = mods.swim_turn_speed.HasValue ? mods.swim_turn_speed.Value : critter.m_swimTurnSpeed;
            critter.m_swimAcceleration = mods.swim_acceleration.HasValue ? mods.swim_acceleration.Value : critter.m_swimAcceleration;
            critter.m_flying = mods.flying.HasValue ? mods.flying.Value : critter.m_flying;
            critter.m_jumpStaminaUsage = mods.jump_stamina_usage.HasValue ? mods.jump_stamina_usage.Value : critter.m_jumpStaminaUsage;
            critter.m_tolerateWater = mods.tolerate_water.HasValue ? mods.tolerate_water.Value : critter.m_tolerateWater;
            critter.m_tolerateFire = mods.tolerate_fire.HasValue ? mods.tolerate_fire.Value : critter.m_tolerateFire;
            critter.m_tolerateSmoke = mods.tolerate_smoke.HasValue ? mods.tolerate_smoke.Value : critter.m_tolerateSmoke;
            critter.m_health = mods.health.HasValue ? mods.health.Value : critter.m_health;
            critter.m_staggerWhenBlocked = mods.stagger_when_blocked.HasValue ? mods.stagger_when_blocked.Value : critter.m_staggerWhenBlocked;
            critter.m_staggerDamageFactor = mods.stagger_damage_factor.HasValue ? mods.stagger_damage_factor.Value : critter.m_staggerDamageFactor;

            MonsterAI ai = critter.GetComponent<MonsterAI>();
            if (ai != null)
            {
                // Base AI
                ai.m_viewRange = mods.view_range.HasValue ? mods.view_range.Value : ai.m_viewRange;
                ai.m_viewAngle = mods.view_angle.HasValue ? mods.view_angle.Value : ai.m_viewAngle;
                ai.m_hearRange = mods.hear_range.HasValue ? mods.hear_range.Value : ai.m_hearRange;
                ai.m_idleSoundInterval = mods.idle_sound_interval.HasValue ? mods.idle_sound_interval.Value : ai.m_idleSoundInterval;
                ai.m_idleSoundChance = mods.idle_sound_chance.HasValue ? mods.idle_sound_chance.Value : ai.m_idleSoundChance;
                ai.m_moveMinAngle = mods.move_min_angle.HasValue ? mods.move_min_angle.Value : ai.m_moveMinAngle;
                ai.m_smoothMovement = mods.smooth_movement.HasValue ? mods.smooth_movement.Value : ai.m_smoothMovement;
                ai.m_serpentMovement = mods.serpent_movement.HasValue ? mods.serpent_movement.Value : ai.m_serpentMovement;
                ai.m_serpentTurnRadius = mods.serpent_turn_radius.HasValue ? mods.serpent_turn_radius.Value : ai.m_serpentTurnRadius;
                ai.m_jumpInterval = mods.jump_interval.HasValue ? mods.jump_interval.Value : ai.m_jumpInterval;
                ai.m_randomCircleInterval = mods.random_circle_interval.HasValue ? mods.random_circle_interval.Value : ai.m_randomCircleInterval;
                ai.m_randomMoveInterval = mods.random_move_interval.HasValue ? mods.random_move_interval.Value : ai.m_randomMoveInterval;
                ai.m_randomMoveRange = mods.random_move_range.HasValue ? mods.random_move_range.Value : ai.m_randomMoveRange;
                ai.m_randomFly = mods.random_fly.HasValue ? mods.random_fly.Value : ai.m_randomFly;
                ai.m_chanceToTakeoff = mods.chance_to_takeoff.HasValue ? mods.chance_to_takeoff.Value : ai.m_chanceToTakeoff;
                ai.m_chanceToLand = mods.chance_to_land.HasValue ? mods.chance_to_land.Value : ai.m_chanceToLand;
                ai.m_groundDuration = mods.ground_duration.HasValue ? mods.ground_duration.Value : ai.m_groundDuration;
                ai.m_airDuration = mods.air_duration.HasValue ? mods.air_duration.Value : ai.m_airDuration;
                ai.m_maxLandAltitude = mods.max_land_altitude.HasValue ? mods.max_land_altitude.Value : ai.m_maxLandAltitude;
                ai.m_flyAltitudeMin = mods.fly_altitude_min.HasValue ? mods.fly_altitude_min.Value : ai.m_flyAltitudeMin;
                ai.m_flyAltitudeMax = mods.fly_altitude_max.HasValue ? mods.fly_altitude_max.Value : ai.m_flyAltitudeMax;
                ai.m_takeoffTime = mods.takeoff_time.HasValue ? mods.takeoff_time.Value : ai.m_takeoffTime;
                ai.m_avoidFire = mods.avoid_fire.HasValue ? mods.avoid_fire.Value : ai.m_avoidFire;
                ai.m_afraidOfFire = mods.afraid_of_fire.HasValue ? mods.afraid_of_fire.Value : ai.m_afraidOfFire;
                ai.m_avoidWater = mods.avoid_water.HasValue ? mods.avoid_water.Value : ai.m_avoidWater;
                ai.m_spawnMessage = mods.spawn_message ?? ai.m_spawnMessage;
                ai.m_deathMessage = mods.death_message ?? ai.m_deathMessage;
                if (!string.IsNullOrEmpty(mods.path_agent_type))
                    switch (mods.path_agent_type)
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
                ai.m_alertRange = mods.alert_range.HasValue ? mods.alert_range.Value : ai.m_alertRange;
                ai.m_fleeIfHurtWhenTargetCantBeReached = mods.flee_if_hurt_when_target_cant_be_reached.HasValue ? mods.flee_if_hurt_when_target_cant_be_reached.Value : ai.m_fleeIfHurtWhenTargetCantBeReached;
                ai.m_fleeIfNotAlerted = mods.flee_if_not_alerted.HasValue ? mods.flee_if_not_alerted.Value : ai.m_fleeIfNotAlerted;
                ai.m_fleeIfLowHealth = mods.flee_if_low_health.HasValue ? mods.flee_if_low_health.Value : ai.m_fleeIfLowHealth;
                ai.m_circulateWhileCharging = mods.circulate_while_charging.HasValue ? mods.circulate_while_charging.Value : ai.m_circulateWhileCharging;
                ai.m_circulateWhileChargingFlying = mods.circulate_while_charging_flying.HasValue ? mods.circulate_while_charging_flying.Value : ai.m_circulateWhileChargingFlying;
                ai.m_enableHuntPlayer = mods.enable_hunt_player.HasValue ? mods.enable_hunt_player.Value : ai.m_enableHuntPlayer;
                ai.m_attackPlayerObjects = mods.attack_player_objects.HasValue ? mods.attack_player_objects.Value : ai.m_attackPlayerObjects;
                ai.m_attackPlayerObjectsWhenAlerted = mods.attack_player_objects_when_alerted.HasValue ? mods.attack_player_objects_when_alerted.Value : ai.m_attackPlayerObjectsWhenAlerted;
                ai.m_interceptTimeMax = mods.intercept_time_max.HasValue ? mods.intercept_time_max.Value : ai.m_interceptTimeMax;
                ai.m_interceptTimeMin = mods.intercept_time_min.HasValue ? mods.intercept_time_min.Value : ai.m_interceptTimeMin;
                ai.m_maxChaseDistance = mods.max_chase_distance.HasValue ? mods.max_chase_distance.Value : ai.m_maxChaseDistance;
                ai.m_minAttackInterval = mods.min_attack_interval.HasValue ? mods.min_attack_interval.Value : ai.m_minAttackInterval;
                ai.m_circleTargetInterval = mods.circle_target_interval.HasValue ? mods.circle_target_interval.Value : ai.m_circleTargetInterval;
                ai.m_circleTargetDuration = mods.circle_target_duration.HasValue ? mods.circle_target_duration.Value : ai.m_circleTargetDuration;
                ai.m_circleTargetDistance = mods.circle_target_distance.HasValue ? mods.circle_target_distance.Value : ai.m_circleTargetDistance;
                ai.m_sleeping = mods.sleeping.HasValue ? mods.sleeping.Value : ai.m_sleeping;
                ai.m_noiseWakeup = mods.noise_wakeup.HasValue ? mods.noise_wakeup.Value : ai.m_noiseWakeup;
                ai.m_noiseRangeScale = mods.noise_range_scale.HasValue ? mods.noise_range_scale.Value : ai.m_noiseRangeScale;
                ai.m_wakeupRange = mods.wakeup_range.HasValue ? mods.wakeup_range.Value : ai.m_wakeupRange;
                ai.m_avoidLand = mods.avoid_land.HasValue ? mods.avoid_land.Value : ai.m_avoidLand;
                ai.m_consumeRange = mods.consume_range.HasValue ? mods.consume_range.Value : ai.m_consumeRange;
                ai.m_consumeSearchRange = mods.consume_search_range.HasValue ? mods.consume_search_range.Value : ai.m_consumeSearchRange;
                ai.m_consumeSearchInterval = mods.consume_search_interval.HasValue ? mods.consume_search_interval.Value : ai.m_consumeSearchInterval;
                ai.m_consumeHeal = mods.consume_heal.HasValue ? mods.consume_heal.Value : ai.m_consumeHeal;
            }

            Assign_CT_Health(critter, mods);
            Assign_CT_Damage(critter, mods);

            if (mods.evolutions?.Count > 0)
                Assign_CT_Evolutions(critter, mods);

            if (mods.size.HasValue)
                Assign_CT_Size(critter, mods.size.Value);

            if (mods.color != null)
            {
                Assign_CT_Color(name, critter, mods.color);
                ZNetView znView = critter.GetComponent<ZNetView>();
                if (znView != null) { znView.GetZDO().Set("Areas CritterPaint", mods.color); }
            }

            if (mods.level_fixed.HasValue)
                Assign_CT_Level(critter, mods, "fixed");
            else if (mods.level_max.HasValue)
                Assign_CT_Level(critter, mods, "minmax");


            // ----------------------------------------------------------------------------------------------------------------------------------- EXIT
            Main.GLog.LogInfo($"Modifying Critter \"Lv.{critter.GetLevel()} {name}\" in area \"{area.name}\" with config \"{area.cfg}\"");

            CheckedCritters.Add(critter.transform);
            CT_Holder = null;

        }

        private static void Assign_CT_Health(Character critter, CTMods mods)
        {

            float multi = mods.health_multi.HasValue ? mods.health_multi.Value : 1;
            multi *= ByDay(mods, "health_multi", true);
            multi *= ByBoss(mods, "health_multi", true);

            critter.m_health *= multi;

        }

        private static void Assign_CT_Damage(Character critter, CTMods mods)
        {

            ZNetView netView = critter.GetComponent<ZNetView>();
            if (netView == null) return;

            float multi = mods.damage_multi.HasValue ? mods.damage_multi.Value : 1;
            multi *= ByDay(mods, "damage_multi", true);
            multi *= ByBoss(mods, "damage_multi", true);

            netView.GetZDO()?.Set("Areas CritterDmgMultiplier", multi);

        }

        private static void Assign_CT_Evolutions(Character critter, CTMods mods)
        {

            ZNetView netView = critter.GetComponent<ZNetView>();
            if (netView == null) return;

            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, mods.evolutions);

            netView.GetZDO()?.Set($"Areas EvolutionSetter", mStream.ToArray());

            mStream.Close();

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

        private static void Assign_CT_Level(Character critter, CTMods mods, string route)
        {

            if (route == "fixed")
            {

                int lvlFixed = mods.level_fixed.Value;

                lvlFixed += Mathf.FloorToInt(ByDay(mods, "level_fixed"));
                lvlFixed += Mathf.FloorToInt(ByBoss(mods, "level_fixed"));

                critter.SetLevel(Mathf.Clamp(lvlFixed, 0, 100));

            }
            else if (route == "minmax")
            {

                int lvlMin = mods.level_min.HasValue ? mods.level_min.Value : 1;
                int lvlMax = mods.level_max.HasValue ? mods.level_max.Value : 3;
                float lvlCha = mods.level_chance.HasValue ? mods.level_chance.Value : 15;

                lvlMin += Mathf.FloorToInt(ByDay(mods, "level_min"));
                lvlMax += Mathf.FloorToInt(ByDay(mods, "level_max"));
                lvlCha += Mathf.FloorToInt(ByDay(mods, "level_lvlUpChance"));

                lvlMin += Mathf.FloorToInt(ByBoss(mods, "level_min"));
                lvlMax += Mathf.FloorToInt(ByBoss(mods, "level_max"));
                lvlCha += Mathf.FloorToInt(ByBoss(mods, "level_lvlUpChance"));

                lvlCha = Mathf.Clamp(lvlCha, 0, 100);

                while (lvlMin < lvlMax && UnityEngine.Random.Range(0f, 100f) <= lvlCha) lvlMin++;
                critter.SetLevel(Mathf.Clamp(lvlMin, 1, 100));

            }

        }

        private static float ByDay(CTMods mods, string selector, bool multiply = false)
        {

            float result = multiply ? 1f : 0f;

            CTMods.ByDay byDay;
            if (mods.scale_by_day != null)
                if (mods.scale_by_day.TryGetValue(selector, out byDay))
                    if (!multiply)
                        result += Mathf.FloorToInt(EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds()) / byDay.days) * byDay.value;
                    else
                        result *= Mathf.Clamp(Mathf.FloorToInt(EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds()) / byDay.days) * byDay.value, 1, 100);

            return result;

        }

        private static float ByBoss(CTMods mods, string selector, bool multiply = false)
        {

            float result = multiply ? 1f : 0f;

            if (mods.scale_by_boss == null) return result;
            if (!mods.scale_by_boss.ContainsKey(selector)) return result;

            var resultList = new List<float>();
            var perBoss = mods.scale_by_boss[selector];
            foreach (var item in perBoss)
                switch (item.Key)
                {
                    case "eikthyr": if (KilledBosses["eikthyr"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
                    case "the_elder": if (KilledBosses["gdking"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
                    case "bonemass": if (KilledBosses["bonemass"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
                    case "moder": if (KilledBosses["dragon"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
                    case "yagluth": if (KilledBosses["goblinking"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
                    default: break;
                }

            resultList.ForEach(a => result = multiply ? result * a : result + a);

            return result;

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

            Main.GLog.LogInfo($"CT_MatDic generated with count: \"{CT_MatDic.Count}\"");

        }

        private static void RefreshKilledBosses()
        {

            KilledBosses = new Dictionary<string, bool>
            {
                {"eikthyr",ZoneSystem.instance.GetGlobalKey("defeated_eikthyr")},
                {"gdking",ZoneSystem.instance.GetGlobalKey("defeated_gdking")},
                {"bonemass",ZoneSystem.instance.GetGlobalKey("defeated_bonemass")},
                {"dragon",ZoneSystem.instance.GetGlobalKey("defeated_dragon")},
                {"goblinking",ZoneSystem.instance.GetGlobalKey("defeated_goblinking")},
            };

        }

        public static void ResetData()
        {

            KilledBosses = new Dictionary<string, bool>();
            CT_Holder = null;
            CT_MatDic.Clear();
            CheckedCritters.Clear();

        }

    }

}
