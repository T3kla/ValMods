using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Areas.Containers;

namespace Areas
{

    public class Globals
    {

        public class Path
        {

            public static string Assembly;
            public static string Areas { get { return $@"{Assembly}\areas.json"; } }
            public static string Critters { get { return $@"{Assembly}\critters.json"; } }
            public static string SS_Data { get { return $@"{Assembly}\ss_data.json"; } } // SpawnSystem
            public static string CS_Data { get { return $@"{Assembly}\cs_data.json"; } } // CreatureSystem
            public static string SA_Data { get { return $@"{Assembly}\sa_data.json"; } } // SpawnArea

        }

        public static List<Area> Areas = new List<Area>();
        public static JObject Critters;
        public static JObject SS_Data;

    }

}
