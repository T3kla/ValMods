using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Areas.Containers;

namespace Areas
{

    public static class CritterHandler
    {

        public static void Modify(Character critter)
        {

            string name = critter.name.Replace("(Clone)", "");
            Debug.Log($"[Areas] Spawning: \"{name}\"");

            // Humanoid humanoid = critter.GetComponent<Humanoid>();
            // if (humanoid == null) return;

            Area area = AreaHandler.GetArea(critter.transform.position);
            if (area == null) return;

            JToken cfg = Globals.Critters.GetValue(area.cfg_id)?.Value<JToken>(name); // CAMBIAR ESTO PA K FILTREE
            if (cfg == null) return;

            critter.name = "modified";

            // ----------------------------------------------------------------------------------------------------------------------------------- LEVEL
            int level_fixed = cfg.Value<int?>("level_fixed") ?? 0;
            int level_min = cfg.Value<int?>("level_min") ?? 1;
            int level_max = cfg.Value<int?>("level_max") ?? 3;
            int level_lvlUpChance = cfg.Value<int?>("level_lvlUpChance") ?? 15;

            if (level_fixed > 0)
                critter.SetLevel(level_min > 0 ? level_max : 1);
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


        }

    }

}
