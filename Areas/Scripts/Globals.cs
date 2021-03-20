using System.Collections.Generic;
using DifficultyAreas.Containers;

namespace DifficultyAreas
{

    public class Global
    {

        public class Path
        {
            public static string Assembly;
            public static string AreaMap { get { return $@"{Assembly}\area_map.json"; } }
            public static string AreaCfgs { get { return $@"{Assembly}\area_configs.json"; } }
        }

        public static List<Area> Areas = new List<Area>();
        public static List<AreaCfg> AreaCfgs = new List<AreaCfg>();

    }

}
