using BepInEx.Configuration;
using UnityEngine;

namespace ColorfulSigns
{
    public static class Configs
    {
        public static ConfigEntry<bool> Enable;

        public static ConfigEntry<string> DefaultTextColor;
        public static ConfigEntry<bool> UseLibrary;

        public static ConfigEntry<int> MaxFontSize;

        public static ConfigEntry<bool> LoggerEnable;

        public static Color Lit = new Color(0.96f, 0.33f, 0.23f, 1f);

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            Enable = Plugin.Config.Bind("1. General", "Enable", true,
                new ConfigDescription("Enables or disables mod.", null,
                new ConfigurationManagerAttributes { Order = 0, EntryColor = Lit }));
            DefaultTextColor = Plugin.Config.Bind("1. General", "Default Color", "#ededed",
                new ConfigDescription("Changes the default text color of signs.", null,
                new ConfigurationManagerAttributes { Order = 1, DefaultValue = "#ededed" }));
            UseLibrary = Plugin.Config.Bind("1. General", "Library", true,
                new ConfigDescription("Read and use color_library.json", null,
                new ConfigurationManagerAttributes { Order = 2 }));
            MaxFontSize = Plugin.Config.Bind("1. General", "Max Font Size", 8,
                new ConfigDescription("Set max size for the sign font.", null,
                new ConfigurationManagerAttributes { Order = 3, DefaultValue = 8 }));

            LoggerEnable = Plugin.Config.Bind("2. Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { Order = 4, EntryColor = Lit }));

            Plugin.Config.SaveOnConfigSet = true;
        }
    }
}