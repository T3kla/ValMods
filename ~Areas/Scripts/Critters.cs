// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using Areas.Containers;
// using HarmonyLib;
// using Jotunn.Managers;
// using UnityEngine;

// namespace Areas
// {
//     public static class Critters
//     {
//         public static MethodInfo CT_SetHolderInfo = AccessTools.Method(typeof(Critters), nameof(Critters.CT_SetHolder), new Type[] { typeof(GameObject) });
//         public static void CT_SetHolder(GameObject critter) { CT_Holder = critter; }
//         public static GameObject CT_Holder = null;

//         public static Dictionary<string, bool> KilledBosses = new();

//         public static void ProcessCapturedCritter<T>(T spawner)
//         {
//             string name = "";

//             Character critter = CT_Holder.GetComponent<Character>();
//             CT_Holder = null;

//             if (critter == null) return;

//             CreatureSpawner cs = spawner is CreatureSpawner ? spawner as CreatureSpawner : null;
//             if (cs != null)
//             {
//                 var ctName = cs.GetCtName();
//                 if (!string.IsNullOrEmpty(ctName) && Global.CurrentData.VAMods.ContainsKey(ctName))
//                 {
//                     name = ctName;
//                     critter.SetVariant(ctName);
//                     critter.name = $"{ctName}(Clone)";
//                     critter.m_name = $"$enemy_{ctName}";
//                 }
//             }

//             name = string.IsNullOrEmpty(name) ? critter.gameObject.GetCleanName() : name;
//             CTData data = Areas.GetCTDataFromPos(name, critter.transform.position.ToXZ(), out _, out var cfg);

//             if (data == null)
//             {
//                 Console.instance.Print($"Couldn't find data for critter \"{name}\"");
//                 Critters.Assign_CT_Level_Vanilla(critter);
//                 return;
//             }

//             critter.SetCfg(cfg);
//             Modify(critter, data, name, cfg);
//         }

//         public static void ProcessSpawnCommand(Character critter, string ctName, string givenCfg)
//         {
//             string name = "";

//             if (critter == null) return;
//             if (givenCfg == "vanilla") { Critters.Assign_CT_Level_Vanilla(critter); return; }

//             if (Global.CurrentData.VAMods.ContainsKey(ctName))
//             {
//                 name = ctName;
//                 critter.SetVariant(ctName);
//                 critter.name = $"{name}(Clone)";
//                 critter.m_name = $"$enemy_{name}";
//             }

//             name = string.IsNullOrEmpty(name) ? critter.gameObject.GetCleanName() : name;
//             string config = givenCfg;
//             CTData data;

//             if (givenCfg == null || !Global.CurrentData.CTMods.ContainsKey(givenCfg))
//                 data = Areas.GetCTDataFromPos(name, critter.transform.position.ToXZ(), out _, out config);
//             else
//                 data = Areas.GetCTDataFromCfg(name, config);

//             if (data == null)
//             {
//                 Console.instance.Print($"Couldn't find data for critter \"{name}\"");
//                 Critters.Assign_CT_Level_Vanilla(critter);
//                 return;
//             }

//             critter.SetCfg(config);
//             Modify(critter, data, name, config);
//         }

//         public static void ProcessAwakenCritter(Character critter)
//         {
//             var variant = critter.GetVariant();
//             if (Global.CurrentData.VAMods.ContainsKey(variant))
//             {
//                 critter.name = $"{variant}(Clone)";
//                 critter.m_name = $"$enemy_{variant}";
//             }

//             var cfg = critter.GetCfg();
//             if (string.IsNullOrEmpty(cfg)) return;

//             var data = critter.GetCtData();
//             if (data == null) return;

//             Modify(critter, data, critter.gameObject.GetCleanName(), cfg, true);
//         }

//         private static void Modify(Character critter, CTData data, string ctName, string cfg, bool keepLevel = false)
//         {
//             if (data.custom?.scale_by_boss?.Count > 0) RefreshKilledBosses();
//             var prefab = PrefabManager.Instance.GetPrefab(Variants.FindOriginal(ctName) ?? ctName);
//             Patch_Character(critter, data.character);
//             Patch_BaseAI(critter.GetComponent<BaseAI>(), data.base_ai);
//             Patch_MonsterAI(critter.GetComponent<MonsterAI>(), data.monster_ai);
//             Patch_Custom(prefab, critter, data, keepLevel);
//             Main.Log.LogInfo($"Modified Critter \"Lv.{critter.GetLevel()} {ctName}\" with config \"{cfg}\"\n");
//         }

//         public static void Patch_Character(Character critter, CTCharacterData data)
//         {
//             if (critter == null || data == null) return;
//             critter.m_crouchSpeed = data.crouch_speed ?? critter.m_crouchSpeed;
//             critter.m_walkSpeed = data.walk_speed ?? critter.m_walkSpeed;
//             critter.m_speed = data.speed ?? critter.m_speed;
//             critter.m_turnSpeed = data.turn_speed ?? critter.m_turnSpeed;
//             critter.m_runSpeed = data.run_speed ?? critter.m_runSpeed;
//             critter.m_runTurnSpeed = data.run_turn_speed ?? critter.m_runTurnSpeed;
//             critter.m_flySlowSpeed = data.fly_slow_speed ?? critter.m_flySlowSpeed;
//             critter.m_flyFastSpeed = data.fly_fast_speed ?? critter.m_flyFastSpeed;
//             critter.m_flyTurnSpeed = data.fly_turn_speed ?? critter.m_flyTurnSpeed;
//             critter.m_acceleration = data.acceleration ?? critter.m_acceleration;
//             critter.m_jumpForce = data.jump_force ?? critter.m_jumpForce;
//             critter.m_jumpForceForward = data.jump_force_forward ?? critter.m_jumpForceForward;
//             critter.m_jumpForceTiredFactor = data.jump_force_tired_factor ?? critter.m_jumpForceTiredFactor;
//             critter.m_airControl = data.air_control ?? critter.m_airControl;
//             critter.m_canSwim = data.can_swim ?? critter.m_canSwim;
//             critter.m_swimDepth = data.swim_depth ?? critter.m_swimDepth;
//             critter.m_swimSpeed = data.swim_speed ?? critter.m_swimSpeed;
//             critter.m_swimTurnSpeed = data.swim_turn_speed ?? critter.m_swimTurnSpeed;
//             critter.m_swimAcceleration = data.swim_acceleration ?? critter.m_swimAcceleration;
//             critter.m_flying = data.flying ?? critter.m_flying;
//             critter.m_jumpStaminaUsage = data.jump_stamina_usage ?? critter.m_jumpStaminaUsage;
//             critter.m_tolerateWater = data.tolerate_water ?? critter.m_tolerateWater;
//             critter.m_tolerateFire = data.tolerate_fire ?? critter.m_tolerateFire;
//             critter.m_tolerateSmoke = data.tolerate_smoke ?? critter.m_tolerateSmoke;
//             critter.m_health = data.health ?? critter.m_health;
//             critter.m_staggerWhenBlocked = data.stagger_when_blocked ?? critter.m_staggerWhenBlocked;
//             critter.m_staggerDamageFactor = data.stagger_damage_factor ?? critter.m_staggerDamageFactor;
//         }

//         public static void Patch_BaseAI(BaseAI ai, CTBaseAIData data)
//         {
//             if (ai == null || data == null) return;
//             ai.m_viewRange = data.view_range ?? ai.m_viewRange;
//             ai.m_viewAngle = data.view_angle ?? ai.m_viewAngle;
//             ai.m_hearRange = data.hear_range ?? ai.m_hearRange;
//             ai.m_idleSoundInterval = data.idle_sound_interval ?? ai.m_idleSoundInterval;
//             ai.m_idleSoundChance = data.idle_sound_chance ?? ai.m_idleSoundChance;
//             ai.m_moveMinAngle = data.move_min_angle ?? ai.m_moveMinAngle;
//             ai.m_smoothMovement = data.smooth_movement ?? ai.m_smoothMovement;
//             ai.m_serpentMovement = data.serpent_movement ?? ai.m_serpentMovement;
//             ai.m_serpentTurnRadius = data.serpent_turn_radius ?? ai.m_serpentTurnRadius;
//             ai.m_jumpInterval = data.jump_interval ?? ai.m_jumpInterval;
//             ai.m_randomCircleInterval = data.random_circle_interval ?? ai.m_randomCircleInterval;
//             ai.m_randomMoveInterval = data.random_move_interval ?? ai.m_randomMoveInterval;
//             ai.m_randomMoveRange = data.random_move_range ?? ai.m_randomMoveRange;
//             ai.m_randomFly = data.random_fly ?? ai.m_randomFly;
//             ai.m_chanceToTakeoff = data.chance_to_takeoff ?? ai.m_chanceToTakeoff;
//             ai.m_chanceToLand = data.chance_to_land ?? ai.m_chanceToLand;
//             ai.m_groundDuration = data.ground_duration ?? ai.m_groundDuration;
//             ai.m_airDuration = data.air_duration ?? ai.m_airDuration;
//             ai.m_maxLandAltitude = data.max_land_altitude ?? ai.m_maxLandAltitude;
//             ai.m_flyAltitudeMin = data.fly_altitude_min ?? ai.m_flyAltitudeMin;
//             ai.m_flyAltitudeMax = data.fly_altitude_max ?? ai.m_flyAltitudeMax;
//             ai.m_takeoffTime = data.takeoff_time ?? ai.m_takeoffTime;
//             ai.m_avoidFire = data.avoid_fire ?? ai.m_avoidFire;
//             ai.m_afraidOfFire = data.afraid_of_fire ?? ai.m_afraidOfFire;
//             ai.m_avoidWater = data.avoid_water ?? ai.m_avoidWater;
//             ai.m_spawnMessage = data.spawn_message ?? ai.m_spawnMessage;
//             ai.m_deathMessage = data.death_message ?? ai.m_deathMessage;
//             if (!string.IsNullOrEmpty(data.path_agent_type))
//                 switch (data.path_agent_type)
//                 {
//                     case "Humanoid": ai.m_pathAgentType = Pathfinding.AgentType.Humanoid; break;
//                     case "TrollSize": ai.m_pathAgentType = Pathfinding.AgentType.TrollSize; break;
//                     case "HugeSize": ai.m_pathAgentType = Pathfinding.AgentType.HugeSize; break;
//                     case "HorseSize": ai.m_pathAgentType = Pathfinding.AgentType.HorseSize; break;
//                     case "HumanoidNoSwim": ai.m_pathAgentType = Pathfinding.AgentType.HumanoidNoSwim; break;
//                     case "HumanoidAvoidWater": ai.m_pathAgentType = Pathfinding.AgentType.HumanoidAvoidWater; break;
//                     case "Fish": ai.m_pathAgentType = Pathfinding.AgentType.Fish; break;
//                     case "Wolf": ai.m_pathAgentType = Pathfinding.AgentType.Wolf; break;
//                     case "BigFish": ai.m_pathAgentType = Pathfinding.AgentType.BigFish; break;
//                     case "GoblinBruteSize": ai.m_pathAgentType = Pathfinding.AgentType.GoblinBruteSize; break;
//                     case "HumanoidBigNoSwim": ai.m_pathAgentType = Pathfinding.AgentType.HumanoidBigNoSwim; break;
//                     default: break;
//                 }
//         }

//         public static void Patch_MonsterAI(MonsterAI ai, CTMonsterAIData data)
//         {
//             if (ai == null || data == null) return;
//             ai.m_alertRange = data.alert_range ?? ai.m_alertRange;
//             ai.m_fleeIfHurtWhenTargetCantBeReached = data.flee_if_hurt_when_target_cant_be_reached ?? ai.m_fleeIfHurtWhenTargetCantBeReached;
//             ai.m_fleeIfNotAlerted = data.flee_if_not_alerted ?? ai.m_fleeIfNotAlerted;
//             ai.m_fleeIfLowHealth = data.flee_if_low_health ?? ai.m_fleeIfLowHealth;
//             ai.m_circulateWhileCharging = data.circulate_while_charging ?? ai.m_circulateWhileCharging;
//             ai.m_circulateWhileChargingFlying = data.circulate_while_charging_flying ?? ai.m_circulateWhileChargingFlying;
//             ai.m_enableHuntPlayer = data.enable_hunt_player ?? ai.m_enableHuntPlayer;
//             ai.m_attackPlayerObjects = data.attack_player_objects ?? ai.m_attackPlayerObjects;
//             ai.m_interceptTimeMax = data.intercept_time_max ?? ai.m_interceptTimeMax;
//             ai.m_interceptTimeMin = data.intercept_time_min ?? ai.m_interceptTimeMin;
//             ai.m_maxChaseDistance = data.max_chase_distance ?? ai.m_maxChaseDistance;
//             ai.m_minAttackInterval = data.min_attack_interval ?? ai.m_minAttackInterval;
//             ai.m_circleTargetInterval = data.circle_target_interval ?? ai.m_circleTargetInterval;
//             ai.m_circleTargetDuration = data.circle_target_duration ?? ai.m_circleTargetDuration;
//             ai.m_circleTargetDistance = data.circle_target_distance ?? ai.m_circleTargetDistance;
//             ai.m_sleeping = data.sleeping ?? ai.m_sleeping;
//             ai.m_noiseWakeup = data.noise_wakeup ?? ai.m_noiseWakeup;
//             ai.m_noiseRangeScale = data.noise_range_scale ?? ai.m_noiseRangeScale;
//             ai.m_wakeupRange = data.wakeup_range ?? ai.m_wakeupRange;
//             ai.m_avoidLand = data.avoid_land ?? ai.m_avoidLand;
//             ai.m_consumeRange = data.consume_range ?? ai.m_consumeRange;
//             ai.m_consumeSearchRange = data.consume_search_range ?? ai.m_consumeSearchRange;
//             ai.m_consumeSearchInterval = data.consume_search_interval ?? ai.m_consumeSearchInterval;
//         }

//         public static void Patch_Custom(GameObject prefab, Character critter, CTData data, bool keepLevel = false)
//         {
//             if (critter == null || data == null || data.custom == null) return;
//             Assign_CT_Damage(critter, data.custom);
//             Assign_CT_Level(critter, data.custom, keepLevel);
//             Assign_CT_Health(critter, data.custom);

//             if (data.custom.evolution == null) return;
//             Apply_CT_Evolution(critter, data.custom.evolution, prefab);
//         }

//         private static void Assign_CT_Damage(Character critter, CTCustomData data)
//         {
//             float multi = data.damage_multi ?? 1;
//             multi *= ByDay(data, "damage_multi", true);
//             multi *= ByBoss(data, "damage_multi", true);

//             critter.SetDamageMulti(multi);
//         }

//         public static void Assign_CT_Level(Character critter, CTCustomData data, bool keepLevel = false)
//         {
//             if (keepLevel)
//             {
//                 critter.SetLevel(critter.GetLevel());
//                 return;
//             }
//             else if (data.level_fixed.HasValue)
//             {
//                 int lvlFixed = data.level_fixed.Value;

//                 lvlFixed += Mathf.FloorToInt(ByDay(data, "level_fixed"));
//                 lvlFixed += Mathf.FloorToInt(ByBoss(data, "level_fixed"));

//                 critter.SetLevel(Mathf.Clamp(lvlFixed, 0, 100));
//             }
//             else
//             {
//                 int lvlMin = data.level_min ?? 1;
//                 int lvlMax = data.level_max ?? 3;
//                 float lvlCha = data.level_chance ?? 15;

//                 lvlMin += Mathf.FloorToInt(ByDay(data, "level_min"));
//                 lvlMax += Mathf.FloorToInt(ByDay(data, "level_max"));
//                 lvlCha += Mathf.FloorToInt(ByDay(data, "level_lvlUpChance"));

//                 lvlMin += Mathf.FloorToInt(ByBoss(data, "level_min"));
//                 lvlMax += Mathf.FloorToInt(ByBoss(data, "level_max"));
//                 lvlCha += Mathf.FloorToInt(ByBoss(data, "level_lvlUpChance"));

//                 lvlCha = Mathf.Clamp(lvlCha, 0, 100);

//                 while (lvlMin < lvlMax && UnityEngine.Random.Range(0f, 100f) <= lvlCha) lvlMin++;
//                 critter.SetLevel(Mathf.Clamp(lvlMin, 1, 100));
//             }

//             critter.SetHealth(critter.GetMaxHealth());
//         }

//         private static void Assign_CT_Health(Character critter, CTCustomData data)
//         {
//             var percent = critter.GetCustomHealthPercentage() ?? 1f;
//             var level = critter.GetLevel();
//             // var difficultyHealthScale = Game.instance.GetDifficultyHealthScale(critter.transform.position);
//             var difficultyHealthScale = 1f;

//             var multi = data.health_multi ?? 1f;
//             multi *= ByDay(data, "health_multi", true);
//             multi *= ByBoss(data, "health_multi", true);

//             var newMax = critter.m_health * multi * difficultyHealthScale * (float)level;
//             var newCur = newMax * percent;

//             critter.SetMaxHealth(newMax);
//             critter.SetHealth(newCur);
//         }

//         public static void Assign_CT_Level_Vanilla(Character critter)
//         {
//             int lvlMin = 1;
//             int lvlMax = 3;
//             float lvlCha = 15;

//             while (lvlMin < lvlMax && UnityEngine.Random.Range(0f, 100f) <= lvlCha) lvlMin++;
//             critter.SetLevel(Mathf.Clamp(lvlMin, 1, 100));
//         }

//         public static void Apply_CT_Evolution(Character critter, Dictionary<int[], Stage> evoDic = null, GameObject prefab = null)
//         {
//             if (critter.name.Contains("(Evo)")) return;
//             critter.name += "(Evo)";

//             var evolution = evoDic ?? critter.GetCtData()?.custom?.evolution;
//             if (evolution == null) return;
//             int level = critter.GetLevel();
//             Stage stage = null;
//             int[] interval = null;

//             foreach (var stg in evolution)
//                 if (level >= stg.Key[0] && level <= stg.Key[1]) { interval = stg.Key; stage = stg.Value; break; }
//             if (stage == null) return;

//             // Declarations
//             LevelEffects levelEffects = critter.GetComponentInChildren<LevelEffects>();
//             LevelEffects.LevelSetup levelSetup = null;
//             if (stage.stage.HasValue && levelEffects != null)
//                 if (stage.stage == 2 || stage.stage == 3)
//                     levelSetup = levelEffects.m_levelSetups[stage.stage.Value - 2];

//             // Fill missing Stage parameters with vanilla LevelSetup if found
//             if (levelSetup != null)
//             {
//                 if (!stage.h.HasValue) { stage.h = levelSetup.m_hue; }
//                 if (!stage.s.HasValue) { stage.s = levelSetup.m_saturation; }
//                 if (!stage.v.HasValue) { stage.v = levelSetup.m_value; }
//                 if (!stage.scale.HasValue) { stage.scale = levelSetup.m_scale; }
//             }

//             // Apply Scale
//             if (stage.scale.HasValue)
//             {
//                 var ctName = critter.gameObject.GetCleanName();
//                 var pfb = prefab ?? PrefabManager.Instance.GetPrefab(Variants.FindOriginal(ctName) ?? ctName);
//                 critter.transform.localScale = pfb.transform.localScale * Mathf.Clamp(stage.scale.Value, 0.1f, 50f);
//             }

//             // Apply Decoration Gameobjects if found
//             GameObject baseDecor = levelEffects?.m_baseEnableObject;
//             if (stage.stage.HasValue && stage.stage.Value > 1)
//             {
//                 GameObject stageDecor = levelSetup?.m_enableObject;
//                 if (stageDecor != null) stageDecor.SetActive(true);
//                 if (baseDecor != null) baseDecor.SetActive(false);
//             }
//             else
//             {
//                 if (baseDecor != null) baseDecor.SetActive(true);
//             }

//             // Find Renderer
//             Renderer renderer = levelEffects?.m_mainRender ?? critter.GetComponentInChildren<SkinnedMeshRenderer>();
//             if (renderer == null) return;

//             // Apply/Create Material
//             string key = $"{critter.gameObject.GetCleanName()} [{interval[0]}-{interval[1]}]";
//             Material mat;
//             if (LevelEffects.m_materials.TryGetValue(key, out var value))
//             {
//                 mat = new Material(value);
//             }
//             else
//             {
//                 mat = new Material(renderer.sharedMaterials[0]);
//                 if (stage.h.HasValue) mat.SetFloat("_Hue", stage.h.Value);
//                 if (stage.s.HasValue) mat.SetFloat("_Saturation", stage.s.Value);
//                 if (stage.v.HasValue) mat.SetFloat("_Value", stage.v.Value);
//                 LevelEffects.m_materials.Add(key, mat);
//             }
//             renderer.sharedMaterials = new Material[] { mat };
//         }

//         private static float ByDay(CTCustomData mods, string selector, bool multiply = false)
//         {
//             float result = multiply ? 1f : 0f;

//             if (mods.scale_by_day != null)
//                 if (mods.scale_by_day.TryGetValue(selector, out var value))
//                     if (!multiply)
//                         result += Mathf.FloorToInt(EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds()) / value.days) * value.value;
//                     else
//                         result *= Mathf.Clamp(Mathf.FloorToInt(EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds()) / value.days) * value.value, 1, 100);

//             return result;
//         }

//         private static float ByBoss(CTCustomData mods, string selector, bool multiply = false)
//         {
//             float result = multiply ? 1f : 0f;

//             if (mods.scale_by_boss == null) return result;
//             if (!mods.scale_by_boss.ContainsKey(selector)) return result;

//             var resultList = new List<float>();
//             var perBoss = mods.scale_by_boss[selector];
//             foreach (var item in perBoss)
//                 switch (item.Key)
//                 {
//                     case "eikthyr": if (KilledBosses["eikthyr"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
//                     case "the_elder": if (KilledBosses["gdking"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
//                     case "bonemass": if (KilledBosses["bonemass"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
//                     case "moder": if (KilledBosses["dragon"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
//                     case "yagluth": if (KilledBosses["goblinking"]) resultList.Add(Mathf.Clamp(item.Value, 0, 100)); break;
//                     default: break;
//                 }


//             resultList.ForEach(a => result = multiply ? result * a : result + a);

//             return result;
//         }

//         private static void RefreshKilledBosses()
//         {
//             KilledBosses = new Dictionary<string, bool>
//             {
//                 {"eikthyr",ZoneSystem.instance.GetGlobalKey("defeated_eikthyr")},
//                 {"gdking",ZoneSystem.instance.GetGlobalKey("defeated_gdking")},
//                 {"bonemass",ZoneSystem.instance.GetGlobalKey("defeated_bonemass")},
//                 {"dragon",ZoneSystem.instance.GetGlobalKey("defeated_dragon")},
//                 {"goblinking",ZoneSystem.instance.GetGlobalKey("defeated_goblinking")},
//             };
//         }

//         public static void OnDataReset()
//         {
//             KilledBosses = new Dictionary<string, bool>();
//             CT_Holder = null;
//         }

//         public static Character Spawn(GameObject prefab, Vector3 position, Quaternion rotation, bool patrol = false)
//         {
//             if (ZoneSystem.instance.FindFloor(position, out var height))
//                 position.y = height + 0.25f;

//             GameObject newCritter = UnityEngine.Object.Instantiate(prefab, position, rotation);

//             ZDO zDO = newCritter.GetComponent<ZNetView>()?.GetZDO();
//             Character character = newCritter.GetComponent<Character>();
//             BaseAI baseAI = newCritter.GetComponent<BaseAI>();

//             if (baseAI != null && patrol)
//                 baseAI.SetPatrolPoint();

//             zDO?.SetPGWVersion(zDO.GetPGWVersion());

//             return character;
//         }

//     }

// }
