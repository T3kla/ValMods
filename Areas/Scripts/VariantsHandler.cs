using System.Collections.Generic;
using UnityEngine;
using Jotunn.Managers;
using Jotunn.Configs;

namespace Areas
{

    public static class VariantsHandler
    {

        public static Dictionary<string, GameObject> Variants = new Dictionary<string, GameObject>();
        public static HashSet<string> Tokens = new HashSet<string>();

        public static void OnDataLoaded()
        {
            GenerateVariants();
        }

        public static void OnDataReset()
        {
            ClearVariants();
        }

        public static GameObject GetVariant(string name)
        {

            return Variants.TryGetValue(name, out var value) ? value : null;

        }

        private static void GenerateVariants()
        {

            if (Globals.CurrentData.VAMods == null) return;
            if (Globals.CurrentData.VAMods.Count < 1) return;

            foreach (var variant in Globals.CurrentData.VAMods)
            {
                GameObject newCritter = PrefabManager.Instance.CreateClonedPrefab(variant.Key, variant.Value.original);
                if (newCritter == null) { Main.GLog.LogInfo($"Couldn't find original critter for variant \"{variant.Key}\""); continue; }
                newCritter.name = variant.Key;

                Character character = newCritter.GetComponent<Character>();
                CritterHandler.Patch_Character(newCritter.GetComponent<Character>(), variant.Value.character);
                CritterHandler.Patch_BaseAI(newCritter.GetComponent<BaseAI>(), variant.Value.base_ai);
                CritterHandler.Patch_MonsterAI(newCritter.GetComponent<MonsterAI>(), variant.Value.monster_ai);
                CritterHandler.Assign_CT_Evolutions(character, variant.Value.evolution);

                if (variant.Value.damage_multi.HasValue) character.SetDamageMulti(variant.Value.damage_multi.Value);

                if (variant.Value.localization != null)
                {
                    string tokenKey = $"$enemy_{variant.Key}";
                    character.m_name = tokenKey;
                    Tokens.Add(tokenKey);
                    variant.Value.localization?.ForEach(a => LocalizationManager.Instance.AddToken(tokenKey, a[1], a[0]));
                }

                PrefabManager.Instance.AddPrefab(newCritter);
            }

        }

        private static void ClearVariants()
        {

            Variants.Clear();
            Localization.m_instance.m_translations.RemoveAll(Tokens);

        }

    }

}
