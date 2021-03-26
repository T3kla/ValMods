using System.Collections.Generic;
using UnityEngine;

namespace Areas.Containers
{

    public class Area
    {
        public string id;
        public string cfg_id;
        public string display_name;

        public Vector2 center;
        public int inner_radious;
        public int outter_radious;

        public AreaCfg cfg;
    }

    public class AreaCfg
    {
        public string id;
        public int layer;

        public CritterCfg general;
        public List<CritterCfg> specific;
    }

    public class CritterCfg
    {
        // ----------------------------------------------------------------------------------------------------------------------------------- GENERAL
        public string name;

        // ----------------------------------------------------------------------------------------------------------------------------------- LEVEL
        public int level_max;
        public int level_min;
        public int level_lvlUpChance;
        public int level_fixed;
    }


}
