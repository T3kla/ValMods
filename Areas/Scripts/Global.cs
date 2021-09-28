using Areas.Containers;
using BepInEx.Configuration;
using UnityEngine;

namespace Areas
{
    public static class Global
    {
        public static class Path
        {
            public static string ModFolder;
            public static string Areas => $@"{ModFolder}\areas.yaml";
            public static string CTData => $@"{ModFolder}\critters.yaml";
            public static string VAData => $@"{ModFolder}\variants.yaml";  // SpawnArea
            public static string SSData => $@"{ModFolder}\ss_data.yaml";  // SpawnSystem
            public static string CSData => $@"{ModFolder}\cs_data.yaml";  // CreatureSystem
            public static string SAData => $@"{ModFolder}\sa_data.yaml"; // SpawnArea
            public static string Assets => $@"{ModFolder}\areasbundle";
        }

        public static class Config
        {
            public static ConfigEntry<KeyCode> GUI_TogglePanel;
            public static ConfigEntry<KeyCode> GUI_ToggleMouse;
            public static ConfigEntry<string> GUI_DefaultPosition;
            public static ConfigEntry<string> GUI_DefaultSize;

            public static ConfigEntry<bool> LootEnable;
            public static ConfigEntry<int> LootFix;

            public static ConfigEntry<bool> LoggerEnable;
        }

        public static RawData RawLocalData = new();
        public static RawData RawRemoteData = new();

        public static Data CurrentData = new();
    }
}
