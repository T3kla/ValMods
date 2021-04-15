using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Areas.Containers;
using BepInEx.Configuration;
using ModConfigEnforcer;

namespace Areas
{

    public static class Globals
    {

        public static class Path
        {

            public static string Assembly;
            public static string Areas { get { return $@"{Assembly}\areas.json"; } }
            public static string CTData { get { return $@"{Assembly}\critters.json"; } }
            public static string SSData { get { return $@"{Assembly}\ss_data.json"; } } // SpawnSystem
            public static string CSData { get { return $@"{Assembly}\cs_data.json"; } } // CreatureSystem
            public static string SAData { get { return $@"{Assembly}\sa_data.json"; } } // SpawnArea

        }

        public static class LocalRaw
        {

            public static string Areas = "{}";
            public static string CTData = "{}";
            public static string SSData = "{}";
            public static string CSData = "{}";
            public static string SAData = "{}";

        }

        public static class RemoteRaw
        {

            public static string Areas = "{}";
            public static string CTData = "{}";
            public static string SSData = "{}";
            public static string CSData = "{}";
            public static string SAData = "{}";

        }

        public static class Config
        {

            public static ConfigVariable<bool> Debug;
            public static ConfigVariable<int> Loot;

        }

        public static Dictionary<string, Area> Areas = new Dictionary<string, Area>();
        public static Dictionary<string, Dictionary<string, CTMods>> CTMods = new Dictionary<string, Dictionary<string, CTMods>>();
        public static Dictionary<string, Dictionary<int, SSMods>> SSMods = new Dictionary<string, Dictionary<int, SSMods>>();
        public static Dictionary<string, Dictionary<string, CSMods>> CSMods = new Dictionary<string, Dictionary<string, CSMods>>();
        public static Dictionary<string, Dictionary<string, SAMods>> SAMods = new Dictionary<string, Dictionary<string, SAMods>>();

    }

}
