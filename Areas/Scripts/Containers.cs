using UnityEngine;

namespace DifficultyAreas.Containers
{

    public class Area
    {
        public string id;
        public string cfg_id;
        public string display_name;

        public Vector2 center;
        public int inner_radious;
        public int outter_radious;

        // public string[] lvlBracket;
        // public AreaConfig overrideConfig;

        public AreaCfg cfg;
    }

    public class AreaCfg
    {
        public string id;
        public int layer;

        // public float hp_multiplier;
    }


}
