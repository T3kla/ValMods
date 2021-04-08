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

            CTMods crmods = Globals.CTMods[area.cfg][name];


            // ----------------------------------------------------------------------------------------------------------------------------------- LEVEL
            int level_fixed = crmods.level_fixed ?? 0;
            int level_min = crmods.level_min ?? 1;
            int level_max = crmods.level_max ?? 3;
            int level_lvlUpChance = crmods.level_lvlUpChance ?? 15;

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

            // ----------------------------------------------------------------------------------------------------------------------------------- STUFF

            // ----------------------------------------------------------------------------------------------------------------------------------- EMPTY HOLDER
            CritterHolder = null;

        }

    }

}
