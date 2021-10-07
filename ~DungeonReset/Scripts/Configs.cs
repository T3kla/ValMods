using BepInEx.Configuration;

namespace DungeonReset
{
    public static class Configs
    {
        public static ConfigEntry<bool> Enable;
        public static ConfigEntry<float> Interval;
        public static ConfigEntry<string> AllowedThemes;
        public static ConfigEntry<bool> PlayerProtection;
        public static ConfigEntry<float> PlayerProtectionInterval;

        public static ConfigEntry<bool> CommandsEnable;

        public static ConfigEntry<bool> LoggerEnable;

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            Enable = Plugin.Config.Bind("Dungeon Reset", "Enable", true,
                new ConfigDescription("Enables or disables dungeon regeneration.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Interval = Plugin.Config.Bind("Dungeon Reset", "Interval", 82800f,
                new ConfigDescription("Set the amount of seconds it takes each dungeon to try to regenerate.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            AllowedThemes = Plugin.Config.Bind("Dungeon Reset", "Themes", "Crypt, SunkenCrypt, Cave, ForestCrypt",
                new ConfigDescription("Set allowed dungeon themes to reset. Possible themes are: Crypt, SunkenCrypt, Cave, ForestCrypt, GoblinCamp, MeadowsVillage, MeadowsFarm", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PlayerProtection = Plugin.Config.Bind("Dungeon Reset", "Player Protection", true,
                new ConfigDescription("If enabled, dungeons won't reset while players are inside.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            PlayerProtectionInterval = Plugin.Config.Bind("Dungeon Reset", "Player Protection Interval", 600f,
                new ConfigDescription("Time it takes to retry a reset on a dungeon that wasn't reset due to Player Protection.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            CommandsEnable = Plugin.Config.Bind("Commands", "Enable", true,
                new ConfigDescription("Enables or disables commands.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));

            LoggerEnable = Plugin.Config.Bind("Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));

            Plugin.Config.SaveOnConfigSet = true;
        }
    }
}
