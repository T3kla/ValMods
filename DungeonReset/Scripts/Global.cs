using BepInEx.Configuration;

namespace DungeonReset
{
    public static class Global
    {
        public static class Config
        {
            public static ConfigEntry<bool> DungeonResetEnable;
            public static ConfigEntry<float> DungeonResetInterval;
            public static ConfigEntry<string> DungeonResetAllowedThemes;
            public static ConfigEntry<bool> DungeonResetPlayerProtection;
            public static ConfigEntry<float> DungeonResetPlayerProtectionInterval;

            public static ConfigEntry<bool> LoggerEnable;
        }
    }
}
