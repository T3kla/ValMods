using BepInEx.Configuration;

namespace ColorfulSigns
{
    public static class Configs
    {
        public static ConfigEntry<string> DefaultColor;
        public static ConfigEntry<bool> EnableColorLibrary;

        public static ConfigEntry<int> MaxFontSize;

        public static ConfigEntry<bool> LoggerEnable;

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            DefaultColor = Plugin.Config.Bind("Color", "Default Color", "#ededed",
                "Changes the default color of signs. Use hexadecimals as hash followed by six digits.");
            EnableColorLibrary = Plugin.Config.Bind("Color", "Enable Color Library", true,
                "Enables or disables color library functionality.");

            MaxFontSize = Plugin.Config.Bind("Font", "Max Font Size", 8,
                "Stablish a max size for sign fonts. ");

            LoggerEnable = Plugin.Config.Bind("Logger", "Enable", true,
                "Enables or disables debugging logs.");
        }
    }
}