using System.Collections.Generic;

namespace Areas.Containers
{
    public static class Data
    {
        public static Dictionary<string, Area> Areas = new();

        //                      critter          component  fields
        public static Dictionary<string, Dictionary<string, object>> CTMods = new();

        //                      critter          component  fields
        public static Dictionary<string, Dictionary<string, object>> VAMods = new();

        //                         ring              index, fields
        public static Dictionary<string, Dictionary<string, object>> SSMods = new();

        //                         ring              index, fields
        public static Dictionary<string, Dictionary<string, object>> CSMods = new();

        //                         ring              index, fields
        public static Dictionary<string, Dictionary<string, object>> SAMods = new();
    }
}
