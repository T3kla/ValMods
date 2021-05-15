using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using HarmonyLib;
using Areas.Containers;

namespace Areas
{

    public static class CritterHandler
    {

        public static HashSet<Transform> CheckedCritters = new HashSet<Transform>();

        public static MethodInfo CT_SetHolderInfo = AccessTools.Method(typeof(CritterHandler), nameof(CritterHandler.CT_SetHolder), new Type[] { typeof(GameObject) });
        public static void CT_SetHolder(GameObject critter) { CT_Holder = critter; }
        public static GameObject CT_Holder = null;

        public static Dictionary<string, bool> KilledBosses = new Dictionary<string, bool>();

        public static void ProcessCapturedCritter()
        {

            Character critter = CT_Holder.GetComponent<Character>();
            CT_Holder = null;

            if (critter == null) return;
            CheckedCritters.Add(critter.transform);

            string name = critter.GetCleanName();
            CTData data = AreaHandler.GetCTDataFromPos(name, critter.transform.position.ToXZ(), out _, out var cfg);

            if (data == null) return;

            critter.SetCTDataStr(cfg, name);
            Modify(critter, data, name, cfg);

        }

        public static void ProcessSpawnCommand(Character critter = null, string cfg = null)
        {

            if (critter == null) return;
            CheckedCritters.Add(critter.transform);

            if (cfg == "vanilla") { CritterHandler.Assign_CT_Level_Vanilla(critter); return; }

            string name = critter.GetCleanName();
            string config = cfg;
            CTData data = null;

            if (cfg == null || !Globals.CurrentData.CTMods.ContainsKey(cfg))
                data = AreaHandler.GetCTDataFromPos(name, critter.transform.position.ToXZ(), out _, out config);
            else
                data = AreaHandler.GetCTDataFromCfg(name, config);

            if (data == null) return;

            critter.SetCTDataStr(config, name);
            Modify(critter, data, name, config);

        }

        public static void ProcessAwakenCritter(Character critter)
        {

            if (critter == null) return;
            CheckedCritters.Add(critter.transform);

            if (!critter.GetCTDataStr(out var dataStr)) return;
            CTData data = critter.GetCTData();
            if (data == null) return;

            Modify(critter, data, dataStr.name, dataStr.cfg);

        }

        private static void Modify(Character critter, CTData data, string critterName, string cfg)
        {
            Main.GLog.LogInfo($"Modifying Critter \"Lv.{critter.GetLevel()} {critterName}\" with config \"{cfg}\"");
            if (data.custom?.scale_by_boss?.Count > 0) RefreshKilledBosses();
            Patch_Character(critter, data.character);
            Patch_BaseAI(critter.GetComponent<BaseAI>(), data.base_ai);
            Patch_MonsterAI(critter.GetComponent<MonsterAI>(), data.monster_ai);
            Patch_Custom(critter, data.custom);
        }

        public static void Patch_Character(Character critter, CTCharacterData data)
        {
            if (critter == null || data == null) return;
            critter.m_crouchSpeed = data.crouch_speed.HasValue ? data.crouch_speed.Value : critter.m_crouchSpeed;
            critter.m_walkSpeed = data.walk_speed.HasValue ? data.walk_speed.Value : critter.m_walkSpeed;
            critter.m_speed = data.speed.HasValue ? data.speed.Value : critter.m_speed;
            critter.m_turnSpeed = data.turn_speed.HasValue ? data.turn_speed.Value : critter.m_turnSpeed;
            critter.m_runSpeed = data.run_speed.HasValue ? data.run_speed.Value : critter.m_runSpeed;
            critter.m_runTurnSpeed = data.run_turn_speed.HasValue ? data.run_turn_speed.Value : critter.m_runTurnSpeed;
            critter.m_flySlowSpeed = data.fly_slow_speed.HasValue ? data.fly_slow_speed.Value : critter.m_flySlowSpeed;
            critter.m_flyFastSpeed = data.fly_fast_speed.HasValue ? data.fly_fast_speed.Value : critter.m_flyFastSpeed;
            critter.m_flyTurnSpeed = data.fly_turn_speed.HasValue ? data.fly_turn_speed.Value : critter.m_flyTurnSpeed;
            critter.m_acceleration = data.acceleration.HasValue ? data.acceleration.Value : critter.m_acceleration;
            critter.m_jumpForce = data.jump_force.HasValue ? data.jump_force.Value : critter.m_jumpForce;
            critter.m_jumpForceForward = data.jump_force_forward.HasValue ? data.jump_force_forward.Value : critter.m_jumpForceForward;
            critter.m_jumpForceTiredFactor = data.jump_force_tired_factor.HasValue ? data.jump_force_tired_factor.Value : critter.m_jumpForceTiredFactor;
            critter.m_airControl = data.air_control.HasValue ? data.air_control.Value : critter.m_airControl;
            critter.m_canSwim = data.can_swim.HasValue ? data.can_swim.Value : critter.m_canSwim;
            critter.m_swimDepth = data.swim_depth.HasValue ? data.swim_depth.Value : critter.m_swimDepth;
            critter.m_swimSpeed = data.swim_speed.HasValue ? data.swim_speed.Value : critter.m_swimSpeed;
            critter.m_swimTurnSpeed = data.swim_turn_speed.HasValue ? data.swim_turn_speed.Value : critter.m_swimTurnSpeed;
            critter.m_swimAcceleration = data.swim_acceleration.HasValue ? data.swim_acceleration.Value : critter.m_swimAcceleration;
            critter.m_flying = data.flying.HasValue ? data.flying.Value : critter.m_flying;
            critter.m_jumpStaminaUsage = data.jump_stamina_usage.HasValue ? data.jump_stamina_usage.Value : critter.m_jumpStaminaUsage;
            critter.m_tolerateWater = data.tolerate_water.HasValue ? data.tolerate_water.Value : critter.m_tolerateWater;
            critter.m_tolerateFire = data.tolerate_fire.HasValue ? data.tolerate_fire.Value : critter.m_tolerateFire;
            critter.m_tolerateSmoke = data.tolerate_smoke.HasValue ? data.tolerate_smoke.Value : critter.m_tolerateSmoke;
            critter.m_health = data.health.HasValue ? data.health.Value : critter.m_health;
            critter.m_staggerWhenBlocked = data.stagger_when_blocked.HasValue ? data.stagger_when_blocked.Value : critter.m_staggerWhenBlocked;
            critter.m_staggerDamageFactor = data.stagger_damage_factor.HasValue ? data.stagger_damage_factor.Value : critter.m_staggerDamageFactor;
        }

        public static void Patch_BaseAI(BaseAI ai, CTBaseAIData data)
        {
            if (ai == null || data == null) return;
            ai.m_viewRange = data.view_range.HasValue ? data.view_range.Value : ai.m_viewRange;
            ai.m_viewAngle = data.view_angle.HasValue ? data.view_angle.Value : ai.m_viewAngle;
            ai.m_hearRange = data.hear_range.HasValue ? data.hear_range.Value : ai.m_hearRange;
            ai.m_idleSoundInterval = data.idle_sound_interval.HasValue ? data.idle_sound_interval.Value : ai.m_idleSoundInterval;
            ai.m_idleSoundChance = data.idle_sound_chance.HasValue ? data.idle_sound_chance.Value : ai.m_idleSoundChance;
            ai.m_moveMinAngle = data.move_min_angle.HasValue ? data.move_min_angle.Value : ai.m_moveMinAngle;
            ai.m_smoothMovement = data.smooth_movement.HasValue ? data.smooth_movement.Value : ai.m_smoothMovement;
            ai.m_serpentMovement = data.serpent_movement.HasValue ? data.serpent_movement.Value : ai.m_serpentMovement;
            ai.m_serpentTurnRadius = data.serpent_turn_radius.HasValue ? data.serpent_turn_radius.Value : ai.m_serpentTurnRadius;
            ai.m_jumpInterval = data.jump_interval.HasValue ? data.jump_interval.Value : ai.m_jumpInterval;
            ai.m_randomCircleInterval = data.random_circle_interval.HasValue ? data.random_circle_interval.Value : ai.m_randomCircleInterval;
            ai.m_randomMoveInterval = data.random_move_interval.HasValue ? data.random_move_interval.Value : ai.m_randomMoveInterval;
            ai.m_randomMoveRange = data.random_move_range.HasValue ? data.random_move_range.Value : ai.m_randomMoveRange;
            ai.m_randomFly = data.random_fly.HasValue ? data.random_fly.Value : ai.m_randomFly;
            ai.m_chanceToTakeoff = data.chance_to_takeoff.HasValue ? data.chance_to_takeoff.Value : ai.m_chanceToTakeoff;
            ai.m_chanceToLand = data.chance_to_land.HasValue ? data.chance_to_land.Value : ai.m_chanceToLand;
            ai.m_groundDuration = data.ground_duration.HasValue ? data.ground_duration.Value : ai.m_groundDuration;
            ai.m_airDuration = data.air_duration.HasValue ? data.air_duration.Value : ai.m_airDuration;
            ai.m_maxLandAltitude = data.max_land_altitude.HasValue ? data.max_land_altitude.Value : ai.m_maxLandAltitude;
            ai.m_flyAltitudeMin = data.fly_altitude_min.HasValue ? data.fly_altitude_min.Value : ai.m_flyAltitudeMin;
            ai.m_flyAltitudeMax = data.fly_altitude_max.HasValue ? data.fly_altitude_max.Value : ai.m_flyAltitudeMax;
            ai.m_takeoffTime = data.takeoff_time.HasValue ? data.takeoff_time.Value : ai.m_takeoffTime;
            ai.m_avoidFire = data.avoid_fire.HasValue ? data.avoid_fire.Value : ai.m_avoidFire;
            ai.m_afraidOfFire = data.afraid_of_fire.HasValue ? data.afraid_of_fire.Value : ai.m_afraidOfFire;
            ai.m_avoidWater = data.avoid_water.HasValue ? data.avoid_water.Value : ai.m_avoidWater;
            ai.m_spawnMessage = data.spawn_message ?? ai.m_spawnMessage;
            ai.m_deathMessage = data.death_message ?? ai.m_deathMessage;
            if (!string.IsNullOrEmpty(data.path_agent_type))
                switch (data.path_agent_type)
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
        }

        public static void Patch_MonsterAI(MonsterAI ai, CTMonsterAIData data)
        {
            if (ai == null || data == null) return;
            ai.m_alertRange = data.alert_range.HasValue ? data.alert_range.Value : ai.m_alertRange;
            ai.m_fleeIfHurtWhenTargetCantBeReached = data.flee_if_hurt_when_target_cant_be_reached.HasValue ? data.flee_if_hurt_when_target_cant_be_reached.Value : ai.m_fleeIfHurtWhenTargetCantBeReached;
            ai.m_fleeIfNotAlerted = data.flee_if_not_alerted.HasValue ? data.flee_if_not_alerted.Value : ai.m_fleeIfNotAlerted;
            ai.m_fleeIfLowHealth = data.flee_if_low_health.HasValue ? data.flee_if_low_health.Value : ai.m_fleeIfLowHealth;
            ai.m_circulateWhileCharging = data.circulate_while_charging.HasValue ? data.circulate_while_charging.Value : ai.m_circulateWhileCharging;
            ai.m_circulateWhileChargingFlying = data.circulate_while_charging_flying.HasValue ? data.circulate_while_charging_flying.Value : ai.m_circulateWhileChargingFlying;
            ai.m_enableHuntPlayer = data.enable_hunt_player.HasValue ? data.enable_hunt_player.Value : ai.m_enableHuntPlayer;
            ai.m_attackPlayerObjects = data.attack_player_objects.HasValue ? data.attack_player_objects.Value : ai.m_attackPlayerObjects;
            ai.m_attackPlayerObjectsWhenAlerted = data.attack_player_objects_when_alerted.HasValue ? data.attack_player_objects_when_alerted.Value : ai.m_attackPlayerObjectsWhenAlerted;
            ai.m_interceptTimeMax = data.intercept_time_max.HasValue ? data.intercept_time_max.Value : ai.m_interceptTimeMax;
            ai.m_interceptTimeMin = data.intercept_time_min.HasValue ? data.intercept_time_min.Value : ai.m_interceptTimeMin;
            ai.m_maxChaseDistance = data.max_chase_distance.HasValue ? data.max_chase_distance.Value : ai.m_maxChaseDistance;
            ai.m_minAttackInterval = data.min_attack_interval.HasValue ? data.min_attack_interval.Value : ai.m_minAttackInterval;
            ai.m_circleTargetInterval = data.circle_target_interval.HasValue ? data.circle_target_interval.Value : ai.m_circleTargetInterval;
            ai.m_circleTargetDuration = data.circle_target_duration.HasValue ? data.circle_target_duration.Value : ai.m_circleTargetDuration;
            ai.m_circleTargetDistance = data.circle_target_distance.HasValue ? data.circle_target_distance.Value : ai.m_circleTargetDistance;
            ai.m_sleeping = data.sleeping.HasValue ? data.sleeping.Value : ai.m_sleeping;
            ai.m_noiseWakeup = data.noise_wakeup.HasValue ? data.noise_wakeup.Value : ai.m_noiseWakeup;
            ai.m_noiseRangeScale = data.noise_range_scale.HasValue ? data.noise_range_scale.Value : ai.m_noiseRangeScale;
            ai.m_wakeupRange = data.wakeup_range.HasValue ? data.wakeup_range.Value : ai.m_wakeupRange;
            ai.m_avoidLand = data.avoid_land.HasValue ? data.avoid_land.Value : ai.m_avoidLand;
            ai.m_consumeRange = data.consume_range.HasValue ? data.consume_range.Value : ai.m_consumeRange;
            ai.m_consumeSearchRange = data.consume_search_range.HasValue ? data.consume_search_range.Value : ai.m_consumeSearchRange;
            ai.m_consumeSearchInterval = data.consume_search_interval.HasValue ? data.consume_search_interval.Value : ai.m_consumeSearchInterval;
            ai.m_consumeHeal = data.consume_heal.HasValue ? data.consume_heal.Value : ai.m_consumeHeal;
        }

        public static void Patch_Custom(Character critter, CTCustomData data)
        {

            if (critter == null || data == null) return;

            Assign_CT_Health(critter, data);
            Assign_CT_Damage(critter, data);
            Assign_CT_Level(critter, data);

        }

        private static void Assign_CT_Health(Character critter, CTCustomData data)
        {

            float multi = data.health_multi.HasValue ? data.health_multi.Value : 1;
            multi *= ByDay(data, "health_multi", true);
            multi *= ByBoss(data, "health_multi", true);

            critter.m_health *= multi;

        }

        private static void Assign_CT_Damage(Character critter, CTCustomData data)
        {

            float multi = data.damage_multi.HasValue ? data.damage_multi.Value : 1;
            multi *= ByDay(data, "damage_multi", true);
            multi *= ByBoss(data, "damage_multi", true);

        }

        public static void Apply_CT_Evolution(Character critter)
        {

            var evolution = critter?.GetCTData()?.custom?.evolution;
            if (evolution == null) return;
            int level = critter.GetLevel();
            Stage stage = null;
            int[] interval = null;

            foreach (var stg in evolution)
                if (level >= stg.Key[0] && level <= stg.Key[1]) { interval = stg.Key; stage = stg.Value; break; }
            if (stage == null) return;

            // Declarations
            Renderer renderer = new Renderer();
            LevelEffects levelEffects = critter.GetComponentInChildren<LevelEffects>();
            LevelEffects.LevelSetup levelSetup = null;
            if (stage.stage.HasValue && levelEffects != null)
                if (stage.stage == 2 || stage.stage == 3)
                    levelSetup = levelEffects.m_levelSetups[stage.stage.Value - 2];

            // Fill missing Stage parameters with vanilla LevelSetup if found
            if (levelSetup != null)
            {
                if (!stage.h.HasValue) { stage.h = levelSetup.m_hue; }
                if (!stage.s.HasValue) { stage.s = levelSetup.m_saturation; }
                if (!stage.v.HasValue) { stage.v = levelSetup.m_value; }
                if (!stage.scale.HasValue) { stage.scale = levelSetup.m_scale; }
            }

            // Apply Scale
            if (stage.scale.HasValue) critter.transform.localScale *= Mathf.Clamp(stage.scale.Value, 0.1f, 50f);

            // Apply Decoration Gameobjects if found
            GameObject baseDecor = levelEffects?.m_baseEnableObject;
            if (stage.stage.HasValue && stage.stage.Value > 1)
            {
                GameObject stageDecor = levelSetup?.m_enableObject;
                if (stageDecor != null) stageDecor.SetActive(true);
                if (baseDecor != null) baseDecor.SetActive(false);
            }
            else
            {
                if (baseDecor != null) baseDecor.SetActive(true);
            }

            // Find Renderer
            renderer = levelEffects?.m_mainRender ?? critter.GetComponentInChildren<SkinnedMeshRenderer>();
            if (renderer == null) return;

            // Apply/Create Material
            string key = $"{critter.GetCleanName()} [{interval[0]}-{interval[1]}]";
            Material mat;
            if (LevelEffects.m_materials.TryGetValue(key, out var value))
            {
                mat = new Material(value);
            }
            else
            {
                mat = new Material(renderer.sharedMaterials[0]);
                if (stage.h.HasValue) mat.SetFloat("_Hue", stage.h.Value);
                if (stage.s.HasValue) mat.SetFloat("_Saturation", stage.s.Value);
                if (stage.v.HasValue) mat.SetFloat("_Value", stage.v.Value);
                LevelEffects.m_materials.Add(key, mat);
            }
            renderer.sharedMaterials = new Material[] { mat };

        }

        public static void Assign_CT_Level(Character critter, CTCustomData data)
        {

            if (data.level_fixed.HasValue)
            {

                int lvlFixed = data.level_fixed.Value;

                lvlFixed += Mathf.FloorToInt(ByDay(data, "level_fixed"));
                lvlFixed += Mathf.FloorToInt(ByBoss(data, "level_fixed"));

                critter.SetLevel(Mathf.Clamp(lvlFixed, 0, 100));

            }
            else
            {

                int lvlMin = data.level_min.HasValue ? data.level_min.Value : 1;
                int lvlMax = data.level_max.HasValue ? data.level_max.Value : 3;
                float lvlCha = data.level_chance.HasValue ? data.level_chance.Value : 15;

                lvlMin += Mathf.FloorToInt(ByDay(data, "level_min"));
                lvlMax += Mathf.FloorToInt(ByDay(data, "level_max"));
                lvlCha += Mathf.FloorToInt(ByDay(data, "level_lvlUpChance"));

                lvlMin += Mathf.FloorToInt(ByBoss(data, "level_min"));
                lvlMax += Mathf.FloorToInt(ByBoss(data, "level_max"));
                lvlCha += Mathf.FloorToInt(ByBoss(data, "level_lvlUpChance"));

                lvlCha = Mathf.Clamp(lvlCha, 0, 100);

                while (lvlMin < lvlMax && UnityEngine.Random.Range(0f, 100f) <= lvlCha) lvlMin++;
                critter.SetLevel(Mathf.Clamp(lvlMin, 1, 100));

            }

            Check_Apply_CT_Evolution(critter);

        }

        public static void Assign_CT_Level_Vanilla(Character critter)
        {

            int lvlMin = 1;
            int lvlMax = 3;
            float lvlCha = 15;

            while (lvlMin < lvlMax && UnityEngine.Random.Range(0f, 100f) <= lvlCha) lvlMin++;
            critter.SetLevel(Mathf.Clamp(lvlMin, 1, 100));

            Check_Apply_CT_Evolution(critter);

        }

        /// <summary>
        ///     Checks if Evolution should be applied immediately or via LevelEffects awake
        /// </summary>
        public static void Check_Apply_CT_Evolution(Character critter)
        {

            // If it has LevelEffects, it's check will trigger evolution application
            LevelEffects levelEffects = critter.GetComponentInChildren<LevelEffects>();
            if (levelEffects == null)
                Apply_CT_Evolution(critter);

        }

        private static float ByDay(CTCustomData mods, string selector, bool multiply = false)
        {

            float result = multiply ? 1f : 0f;

            if (mods.scale_by_day != null)
                if (mods.scale_by_day.TryGetValue(selector, out var value))
                    if (!multiply)
                        result += Mathf.FloorToInt(EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds()) / value.days) * value.value;
                    else
                        result *= Mathf.Clamp(Mathf.FloorToInt(EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds()) / value.days) * value.value, 1, 100);

            return result;

        }

        private static float ByBoss(CTCustomData mods, string selector, bool multiply = false)
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

        public static void OnDataReset()
        {

            KilledBosses = new Dictionary<string, bool>();
            CT_Holder = null;
            CheckedCritters.Clear();

        }

        public static Character Spawn(GameObject prefab, Vector3 position, Quaternion rotation, bool patrol = false)
        {

            if (ZoneSystem.instance.FindFloor(position, out var height))
                position.y = height + 0.25f;

            GameObject newCritter = UnityEngine.Object.Instantiate(prefab, position, rotation);

            ZDO zDO = newCritter.GetComponent<ZNetView>()?.GetZDO();
            Character character = newCritter.GetComponent<Character>();
            BaseAI baseAI = newCritter.GetComponent<BaseAI>();

            if (baseAI != null && patrol)
                baseAI.SetPatrolPoint();

            zDO?.SetPGWVersion(zDO.GetPGWVersion());
            zDO?.Set("spawn_id", zDO.m_uid);
            zDO?.Set("alive_time", ZNet.instance.GetTime().Ticks);

            return character;

        }

    }

}
