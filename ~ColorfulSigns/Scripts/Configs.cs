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

        public static ConfigEntry<bool> EnableOutline;
        public static ConfigEntry<string> OutlineColor;
        public static ConfigEntry<float> OutlineSize;

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

            EnableOutline = Plugin.Config.Bind("2. Outline", "Enable", true,
                new ConfigDescription("Enable the outlining of text in signs.", null,
                new ConfigurationManagerAttributes { Order = 4, EntryColor = Lit }));
            OutlineColor = Plugin.Config.Bind("2. Outline", "Color", "#272830",
                new ConfigDescription("Changes the default outline color of signs.", null,
                new ConfigurationManagerAttributes { Order = 5, DefaultValue = "#272830" }));
            OutlineSize = Plugin.Config.Bind("2. Outline", "Size", 0.15f,
                new ConfigDescription("Set size of the outline effect.",
                new AcceptableValueRange<float>(0f, 0.2f),
                new ConfigurationManagerAttributes { Order = 6, DefaultValue = 0.15f }));

            LoggerEnable = Plugin.Config.Bind("3. Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { Order = 7, EntryColor = Lit }));

            Plugin.Config.SaveOnConfigSet = true;
            Plugin.Config.SettingChanged += (a, b) => ColorfulSigns.UpdateColorLib();
        }
    }
}