using Areas.Containers;
using BepInEx.Configuration;
using UnityEngine;

namespace Areas
{

    public static class Globals
    {

        public static class Path
        {
            public static string Assembly;
            public static string Areas { get { return $@"{Assembly}\areas.yaml"; } }
            public static string CTData { get { return $@"{Assembly}\critters.yaml"; } }
            public static string VAData { get { return $@"{Assembly}\variants.yaml"; } } // SpawnArea
            public static string SSData { get { return $@"{Assembly}\ss_data.yaml"; } } // SpawnSystem
            public static string CSData { get { return $@"{Assembly}\cs_data.yaml"; } } // CreatureSystem
            public static string SAData { get { return $@"{Assembly}\sa_data.yaml"; } } // SpawnArea
            public static string AssetBundle { get { return $@"{Assembly}\areasbundle"; } }
        }

        public static class Config
        {
            public static ConfigEntry<KeyCode> AGUIKeybinding;
            public static ConfigEntry<string> AGUIDefPosition;
            public static ConfigEntry<string> AGUIDefSize;

            public static ConfigEntry<bool> LootEnable;
            public static ConfigEntry<int> LootFix;

            public static ConfigEntry<bool> DungeonRegenEnable;
            public static ConfigEntry<int> DungeonRegenCooldown;
            public static ConfigEntry<string> DungeonRegenAllowedThemes;
            public static ConfigEntry<bool> DungeonRegenPlayerProtection;

            public static ConfigEntry<bool> LoggerEnable;
        }

        public static RawData RawLocalData = new RawData();
        public static RawData RawRemoteData = new RawData();

        public static Data CurrentData = new Data();

    }

}
