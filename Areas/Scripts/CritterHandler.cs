using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Areas.TJson;
using Areas.Containers;

namespace Areas
{

    public static class CritterHandler
    {

        public static void ModifyCritter(SpawnSystem system, SpawnSystem.SpawnData data, ref Character character, CritterCfg cfg)
        {

            // ----------------------------------------------------------------------------------------------------------------------------------- LEVEL
            if (cfg.level_fixed > 0)
                character.SetLevel(cfg.level_fixed > 0 ? cfg.level_fixed : 1);
            else if (cfg.level_min >= cfg.level_max)
                character.SetLevel(cfg.level_max > 0 ? cfg.level_max : 1);
            else
            {
                int newLvl = cfg.level_min;
                while (newLvl < cfg.level_max && Random.Range(0f, 100f) <= cfg.level_lvlUpChance) newLvl++;
                character.SetLevel(newLvl > 0 ? newLvl : 1);
            }

        }

    }

}
