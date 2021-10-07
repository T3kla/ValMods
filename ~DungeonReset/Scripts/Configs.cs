using BepInEx.Configuration;
using UnityEngine;

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

        public static Color Lit = new Color(0.96f, 0.33f, 0.23f, 1f);

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            Enable = Plugin.Config.Bind("1. General", "Enable", true,
                new ConfigDescription("Enables or disables dungeon regeneration.", null,
                new ConfigurationManagerAttributes { Order = 0, IsAdminOnly = true, EntryColor = Lit }));
            Interval = Plugin.Config.Bind("1. General", "Interval", 82800f,
                new ConfigDescription("Set the amount of seconds it takes each dungeon to try to regenerate.", null,
                new ConfigurationManagerAttributes { Order = 1, IsAdminOnly = true }));
            AllowedThemes = Plugin.Config.Bind("1. General", "Themes", "Crypt, SunkenCrypt, Cave, ForestCrypt",
                new ConfigDescription("Set allowed dungeon themes to reset. Possible themes are: Crypt, SunkenCrypt, Cave, ForestCrypt, GoblinCamp, MeadowsVillage, MeadowsFarm", null,
                new ConfigurationManagerAttributes { Order = 2, IsAdminOnly = true }));
            PlayerProtection = Plugin.Config.Bind("1. General", "Player Protection", true,
                new ConfigDescription("If enabled, dungeons won't reset while players are inside.", null,
                new ConfigurationManagerAttributes { Order = 3, IsAdminOnly = true }));
            PlayerProtectionInterval = Plugin.Config.Bind("1. General", "Player Protection Interval", 600f,
                new ConfigDescription("Time it takes to retry a reset on a dungeon that wasn't reset due to Player Protection.", null,
                new ConfigurationManagerAttributes { Order = 4, IsAdminOnly = true }));

            CommandsEnable = Plugin.Config.Bind("2. Commands", "Enable", true,
                new ConfigDescription("Enables or disables commands.", null,
                new ConfigurationManagerAttributes { Order = 5, IsAdminOnly = false, EntryColor = Lit }));

            LoggerEnable = Plugin.Config.Bind("3. Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { Order = 6, IsAdminOnly = false, EntryColor = Lit }));

            Plugin.Config.SaveOnConfigSet = true;
        }
    }
}
