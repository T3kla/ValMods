using System.Collections.Generic;
using ModConfigEnforcer;
using Areas.Containers;

namespace Areas
{

    public static class Globals
    {

        public static class Path
        {

            public static string Assembly;
            public static string Areas { get { return $@"{Assembly}\areas.yaml"; } }
            public static string CTData { get { return $@"{Assembly}\critters.yaml"; } }
            public static string SSData { get { return $@"{Assembly}\ss_data.yaml"; } } // SpawnSystem
            public static string CSData { get { return $@"{Assembly}\cs_data.yaml"; } } // CreatureSystem
            public static string SAData { get { return $@"{Assembly}\sa_data.yaml"; } } // SpawnArea

        }

        public static class LocalRaw
        {

            public static string Areas = "";
            public static string CTData = "";
            public static string SSData = "";
            public static string CSData = "";
            public static string SAData = "";

        }

        public static class RemoteRaw
        {

            public static string Areas = "";
            public static string CTData = "";
            public static string SSData = "";
            public static string CSData = "";
            public static string SAData = "";

        }

        public static class Config
        {

            public static ConfigVariable<int> Loot;
            public static ConfigVariable<bool> DungeonRegenEnable;
            public static ConfigVariable<long> DungeonRegenCooldown;
            public static ConfigVariable<string> DungeonRegenAllowedThemes;
            public static ConfigVariable<bool> DungeonRegenPlayerProtection;
            public static ConfigVariable<bool> Debug;

        }

        public static Dictionary<string, Area> Areas = new Dictionary<string, Area>();
        public static Dictionary<string, Dictionary<string, CTMods>> CTMods = new Dictionary<string, Dictionary<string, CTMods>>();
        public static Dictionary<string, Dictionary<int, SSMods>> SSMods = new Dictionary<string, Dictionary<int, SSMods>>();
        public static Dictionary<string, Dictionary<string, CSMods>> CSMods = new Dictionary<string, Dictionary<string, CSMods>>();
        public static Dictionary<string, Dictionary<string, SAMods>> SAMods = new Dictionary<string, Dictionary<string, SAMods>>();

    }

}
