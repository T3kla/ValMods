// using System;
// using System.Collections.Generic;
// using System.Reflection;
// using System.Reflection.Emit;
// using Areas.NetCode;
// using HarmonyLib;
// using Jotunn;
// using Jotunn.Managers;
// using UnityEngine;

// namespace Areas.Patches
// {
//     [HarmonyPatch]
//     public static class Patches
//     {
//         // ---------------------------------------------------------------------------------------------------------- LANGUAGE CHANGE
//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(Localization), nameof(Localization.SetupLanguage))]
//         public static void Localization_SetupLanguage_Post(Localization __instance, ref string language)
//         {
//             Variants.OnLanguageChanged(language);
//         }

//         // ---------------------------------------------------------------------------------------------------------- PLAYER
//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(Player), nameof(Player.OnSpawned))]
//         public static void Player_OnSpawned(Player __instance)
//         {
//             Main.Log.LogInfo($"ZoneLookup try Start because Player.OnSpawned\n");
//             Areas.ZoneLookup_Start();
//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(Player), nameof(Player.OnRespawn))]
//         public static void Player_OnRespawn(Player __instance)
//         {
//             Main.Log.LogInfo($"ZoneLookup try Start because Player.OnRespawn\n");
//             Areas.ZoneLookup_Start();
//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(Player), nameof(Player.OnDeath))]
//         public static void Player_OnDeath(Player __instance)
//         {
//             Main.Log.LogInfo($"ZoneLookup try Stop because Player.OnDeath\n");
//             Areas.ZoneLookup_Stop();
//         }

//         // ---------------------------------------------------------------------------------------------------------- RCP STUFF
//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(Game), nameof(Game.Start))]
//         // Register RPC calls as soon as possible
//         public static void Game_Start_Post(Game __instance)
//         {
//             ZRoutedRpc.instance.Register<ZPackage>("Areas.SetDataValues", new Action<long, ZPackage>(RPC.SetDataValues));
//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(ZNet), nameof(ZNet.Awake))]
//         // When opening/entering a world, set if I should be using local or remote data
//         public static void ZNet_Awake_Post(ZNet __instance)
//         {
//             var type = __instance.GetInstanceType();

//             Main.Log.LogInfo($"Instance is \"{type}\"\n");

//             switch (type)
//             {
//                 case ZNetExtension.ZNetInstanceType.Local:
//                 case ZNetExtension.ZNetInstanceType.Server:
//                     Main.LoadData(EDS.Local);
//                     break;

//                 case ZNetExtension.ZNetInstanceType.Client:
//                     break;

//                 default: break;
//             }
//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(ZNet), nameof(ZNet.OnDestroy))]
//         public static void ZNet_OnDestroy_Post(ZNet __instance)
//         {
//             Main.ResetData(EDS.Current);
//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(ZNet), nameof(ZNet.RPC_CharacterID))]
//         // When receiving a client, send data
//         public static void ZNet_RPCCharacterID_Post(ZNet __instance, ZRpc rpc, ZDOID characterID)
//         {
//             if (__instance.GetInstanceType() == ZNetExtension.ZNetInstanceType.Client) return;

//             long client = __instance.GetPeer(rpc).m_uid;

//             Main.Log.LogInfo($"Instance is sending Data to client \"{client}\"\n");
//             RPC.SendDataToClient(client);
//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.OnNewPeer))]
//         // When receiving a client, send data
//         public static void ZoneSystem_OnNewPeer_Post(ZoneSystem __instance, ref long peerID)
//         {
//             if (ZNet.instance.GetInstanceType() == ZNetExtension.ZNetInstanceType.Client) return; // Send when Server or Local

//             Main.Log.LogInfo($"Instance is sending Data to client \"{peerID}\"\n");
//             RPC.SendDataToClient(peerID);
//         }


//         // ---------------------------------------------------------------------------------------------------------- CRITTER REGISTER
//         [HarmonyPrefix]
//         [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
//         public static void Character_Awake_Pre(Character __instance)
//         {

//             if (__instance == null) return;
//             if (__instance.IsPlayer()) return;

//             ZNetView znv = __instance.m_nview ?? __instance.GetComponent<ZNetView>();
//             if (znv == null) return;

//             var hp = znv.GetZDO()?.GetFloat("health", __instance.m_health) ?? __instance.m_health;
//             var max = znv.GetZDO()?.GetFloat("max_health", __instance.m_health) ?? __instance.m_health;

//             znv.GetZDO()?.Set("Areas Health Percentage", hp / max);

//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
//         public static void Character_Awake_Post(Character __instance)
//         {

//             if (__instance is null) return;
//             if (__instance.IsPlayer()) return;

//             Critters.ProcessAwakenCritter(__instance);

//         }


//         // ---------------------------------------------------------------------------------------------------------- CRITTER DAMAGE
//         [HarmonyPrefix]
//         [HarmonyPatch(typeof(Character), nameof(Character.Damage))]
//         private static void Character_Damage_Pre(Character __instance, HitData hit)
//         {

//             float? multi = hit?.GetAttacker()?.GetDamageMulti();
//             if (multi.HasValue && multi.Value != 1) hit.ApplyModifier(multi.Value);

//         }


//         // ---------------------------------------------------------------------------------------------------------- CRITTER EVOLUTIONS
//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(LevelEffects), nameof(LevelEffects.SetupLevelVisualization))]
//         public static void LevelEffects_SetupLevelVisualization_Post(LevelEffects __instance, ref int level)
//         {

//             Critters.Apply_CT_Evolution(__instance.m_character);

//         }


//         // ---------------------------------------------------------------------------------------------------------- CRITTER LOOT
//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(CharacterDrop), nameof(CharacterDrop.GenerateDropList))]
//         public static void CharacterDrop_GenerateDropList_Post(CharacterDrop __instance, ref List<KeyValuePair<GameObject, int>> __result)
//         {

//             if (!Configs.LootEnable.Value) return;

//             List<KeyValuePair<GameObject, int>> list = new();

//             int lvlReward = 1;

//             if (__instance.m_character)
//             {
//                 int lvl = __instance.m_character.GetLevel();

//                 if (lvl <= 3)
//                     lvlReward = Mathf.FloorToInt(Mathf.Max(1, 2 * (lvl - 1)));
//                 else
//                 {
//                     int adjustLvl = Mathf.FloorToInt((1 / Configs.LootFix.Value) * lvl + 3);
//                     lvlReward = Mathf.FloorToInt(Mathf.Max(1, 2 * (adjustLvl - 1)));
//                 }

//                 Main.Log.LogInfo($"Calculating DropList for \"Lv.{lvl} {__instance.m_character.gameObject.GetCleanName()}\"\n");
//             }

//             foreach (CharacterDrop.Drop drop in __instance.m_drops)
//             {
//                 if (drop.m_prefab == null) continue;

//                 float dropChance = drop.m_chance;

//                 if (drop.m_levelMultiplier) dropChance *= (float)lvlReward;

//                 if (UnityEngine.Random.value <= dropChance)
//                 {
//                     int amount = UnityEngine.Random.Range(drop.m_amountMin, drop.m_amountMax);
//                     if (drop.m_levelMultiplier) amount *= lvlReward;
//                     if (drop.m_onePerPlayer) amount = ZNet.instance.GetNrOfPlayers();
//                     if (amount > 0) list.Add(new KeyValuePair<GameObject, int>(drop.m_prefab, amount));
//                 }
//             }

//             __result = list;

//         }


//         // ---------------------------------------------------------------------------------------------------------- CRITTER CAPTURE
//         [HarmonyTranspiler]
//         [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Spawn))]
//         private static IEnumerable<CodeInstruction> SpawnSystem_Spawn_Transpiler(IEnumerable<CodeInstruction> instructions)
//         {

//             List<CodeInstruction> ins = new(instructions);
//             List<CodeInstruction> codes = new();

//             for (var i = 0; i < ins.Count; i++)
//             {
//                 MethodInfo method = ins[i].operand as MethodInfo;
//                 string str = method?.Name;

//                 if (str == "Instantiate")
//                 {
//                     // i   = call to instantiate
//                     codes.Add(ins[i]);
//                     // i+1 = stloc.0 storages gameobject in stloc.0
//                     codes.Add(ins[i + 1]);
//                     // i+2 = ldloc.0 loads from stloc.0 which should be the gameobject
//                     codes.Add(new CodeInstruction(OpCodes.Ldloc_0));
//                     // i+3 = call the modify critter info where the first arg is ldloc.0
//                     codes.Add(new CodeInstruction(OpCodes.Call, Critters.CT_SetHolderInfo));

//                     // this jumps i+1 because i'm already adding it
//                     i++;
//                     // this jumps i because i'm already adding it
//                     continue;
//                 }

//                 codes.Add(ins[i]);

//             }

//             return codes;

//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Spawn))]
//         public static void SpawnSystem_Spawn_Post(SpawnSystem __instance) { Critters.ProcessCapturedCritter(__instance); }

//         [HarmonyTranspiler]
//         [HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Spawn))]
//         private static IEnumerable<CodeInstruction> CreatureSpawner_Spawn_Transpiler(IEnumerable<CodeInstruction> instructions)
//         {

//             List<CodeInstruction> ins = new(instructions);
//             List<CodeInstruction> codes = new();

//             for (var i = 0; i < ins.Count; i++)
//             {
//                 MethodInfo method = ins[i].operand as MethodInfo;
//                 string str = method?.Name;

//                 if (str == "Instantiate")
//                 {
//                     // i   = call to instantiate
//                     codes.Add(ins[i]);
//                     // i+1 = stloc.3 storages gameobject in stloc.3
//                     codes.Add(ins[i + 1]);
//                     // i+2 = ldloc.3 loads from stloc.3 which should be the gameobject
//                     codes.Add(new CodeInstruction(OpCodes.Ldloc_3));
//                     // i+3 = call the modify critter info where the first arg is ldloc.3
//                     codes.Add(new CodeInstruction(OpCodes.Call, Critters.CT_SetHolderInfo));

//                     // this jumps i+1 because i'm already adding it
//                     i++;
//                     // this jumps i because i'm already adding it
//                     continue;
//                 }

//                 codes.Add(ins[i]);

//             }

//             return codes;

//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Spawn))]
//         public static void CreatureSpawner_Spawn_Post(CreatureSpawner __instance) { Critters.ProcessCapturedCritter(__instance); }

//         [HarmonyTranspiler]
//         [HarmonyPatch(typeof(SpawnArea), nameof(SpawnArea.SpawnOne))]
//         private static IEnumerable<CodeInstruction> SpawnArea_SpawnOne_Transpiler(IEnumerable<CodeInstruction> instructions)
//         {

//             List<CodeInstruction> ins = new(instructions);
//             List<CodeInstruction> codes = new();

//             for (var i = 0; i < ins.Count; i++)
//             {
//                 MethodInfo method = ins[i].operand as MethodInfo;
//                 string str = method?.Name;

//                 if (str == "Instantiate")
//                 {
//                     // i   = call to instantiate
//                     codes.Add(ins[i]);
//                     // i+1 = stloc.s V_4 storages gameobject in stloc.s V_4
//                     codes.Add(ins[i + 1]);
//                     // i+2 = ldloc.s V_4 loads from stloc.s V_4 which should be the gameobject
//                     codes.Add(new CodeInstruction(OpCodes.Ldloc_S, 4));
//                     // i+3 = call the modify critter info where the first arg is ldloc.s V_4
//                     codes.Add(new CodeInstruction(OpCodes.Call, Critters.CT_SetHolderInfo));

//                     // this jumps i+1 because i'm already adding it
//                     i++;
//                     // this jumps i because i'm already adding it
//                     continue;
//                 }

//                 codes.Add(ins[i]);

//             }

//             return codes;

//         }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(SpawnArea), nameof(SpawnArea.SpawnOne))]
//         public static void SpawnArea_SpawnOne_Post(SpawnArea __instance) => Critters.ProcessCapturedCritter(__instance);


//         // ---------------------------------------------------------------------------------------------------------- RAGDOLL SETUP
//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(Ragdoll), nameof(Ragdoll.Setup))]
//         public static void Ragdoll_Setup_Post(Ragdoll __instance, CharacterDrop characterDrop)
//         {
//             if (characterDrop == null) return;
//             if (characterDrop.m_character == null) return;
//             if (characterDrop.m_character.IsPlayer()) return;

//             LevelEffects levelEffects = characterDrop.GetComponentInChildren<LevelEffects>();
//             var renderer = levelEffects?.m_mainRender ?? characterDrop.GetComponentInChildren<SkinnedMeshRenderer>();
//             if (renderer != null) __instance.m_mainModel.materials = new Material[] { renderer.material };

//             string ctName = characterDrop.m_character.gameObject.GetCleanName();
//             Transform prefab = PrefabManager.Instance.GetPrefab(Variants.FindOriginal(ctName) ?? ctName).transform;
//             if (prefab == null) return;

//             Vector3 rag = __instance.transform.localScale;
//             Vector3 act = characterDrop.m_character.transform.localScale;
//             Vector3 pref = prefab.localScale;

//             Vector3 ragBYact = new(rag.x * act.x, rag.y * act.y, rag.z * act.z);
//             Vector3 final = new(ragBYact.x / pref.x, ragBYact.y / pref.y, ragBYact.z / pref.z);

//             __instance.transform.localScale = final;

//         }


//         // ---------------------------------------------------------------------------------------------------------- MODIFY SPAWNERS
//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Awake))]
//         public static void SpawnSystem_Awake_Post(SpawnSystem __instance) { Spawners.ProcessCapturedSS(__instance); }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.Awake))]
//         public static void CreatureSpawner_Awake_Post(CreatureSpawner __instance) { Spawners.ProcessCapturedCS(__instance); }

//         [HarmonyPostfix]
//         [HarmonyPatch(typeof(SpawnArea), nameof(SpawnArea.Awake))]
//         public static void SpawnArea_Awake_Post(SpawnArea __instance) { Spawners.ProcessCapturedSA(__instance); }

//     }

// }
