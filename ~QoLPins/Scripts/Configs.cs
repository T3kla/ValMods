using BepInEx.Configuration;

namespace QoLPins
{
    public static class Configs
    {
        public static ConfigEntry<bool> Enable;

        public static ConfigEntry<bool> DontPinWhenInvIsEmpty;
        public static ConfigEntry<bool> RemoveAtRetrieval;

        public static ConfigEntry<bool> EnableColors;
        public static ConfigEntry<string> ColorFireplace;
        public static ConfigEntry<string> ColorHouse;
        public static ConfigEntry<string> ColorHammer;
        public static ConfigEntry<string> ColorBall;
        public static ConfigEntry<string> ColorPortal;

        public static ConfigEntry<bool> LoggerEnable;

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            Enable = Plugin.Config.Bind("General", "Enable", true,
                "Enables or disables mod.");

            DontPinWhenInvIsEmpty = Plugin.Config.Bind("QoL", "Generate with empty inventory", true,
                "Death Pin will won't be generated if your inventory was empty.");
            RemoveAtRetrieval = Plugin.Config.Bind("QoL", "Remove at retrieval", true,
                "Death Pin will dissapear automatically when Tombstone is retrieved.");

            EnableColors = Plugin.Config.Bind("Colors", "Always generate Death Pin", false,
                "Enable the coloring of pins.");
            ColorFireplace = Plugin.Config.Bind("General", "Always generate Death Pin", "#cc9d37", // Orange
                "Color for the first icon, the fireplace.");
            ColorHouse = Plugin.Config.Bind("General", "Always generate Death Pin", "#35b5cc", // Cyan
                "Color for the second icon, the house.");
            ColorHammer = Plugin.Config.Bind("General", "Always generate Death Pin", "#4b4c66", // Dusk
                "Color for the third icon, the hammer.");
            ColorBall = Plugin.Config.Bind("General", "Always generate Death Pin", "#9142b3", // Lime
                "Color for the third icon, the ball.");
            ColorPortal = Plugin.Config.Bind("General", "Always generate Death Pin", "#a86840", // Brown
                "Color for the third icon, the portal.");

            LoggerEnable = Plugin.Config.Bind("Logger", "Enable", true,
                "Enables or disables debugging logs.");
        }
    }
}
