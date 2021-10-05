using BepInEx.Configuration;

namespace QoLPins
{
    public static class Configs
    {
        public static ConfigEntry<bool> Enable;

        public static ConfigEntry<bool> DontPinWhenInvIsEmpty;
        public static ConfigEntry<bool> RemoveAtRetrieval;

        public static ConfigEntry<bool> EnableColors;

        public static ConfigEntry<string> ColorDeath;
        public static ConfigEntry<string> ColorBed;

        public static ConfigEntry<string> ColorFireplace;
        public static ConfigEntry<string> ColorHouse;
        public static ConfigEntry<string> ColorHammer;
        public static ConfigEntry<string> ColorBall;
        public static ConfigEntry<string> ColorPortal;

        public static ConfigEntry<string> ColorBoss;
        public static ConfigEntry<string> ColorPlayer;
        public static ConfigEntry<string> ColorShout;
        public static ConfigEntry<string> ColorRandomEvent;
        public static ConfigEntry<string> ColorPing;
        public static ConfigEntry<string> ColorEventArea;

        public static ConfigEntry<bool> LoggerEnable;

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            Enable = Plugin.Config.Bind("1. General", "Enable", true,
                "Enables or disables mod.");

            DontPinWhenInvIsEmpty = Plugin.Config.Bind("1. General", "Generate with empty inventory", true,
                "Death Pin will won't be generated if your inventory was empty.");
            RemoveAtRetrieval = Plugin.Config.Bind("1. General", "Remove at retrieval", true,
                "Death Pin will dissapear automatically when Tombstone is retrieved.");

            EnableColors = Plugin.Config.Bind("2. Colors", "Always generate Death Pin", true,
                "Enable the coloring of pins.");

            ColorDeath = Plugin.Config.Bind("2. Colors", "Death Color", "#d43d3d", // Red
                "Color for the death pin.");
            ColorBed = Plugin.Config.Bind("2. Colors", "Bed Color", "#35b5cc", // Cyan
                "Color for the bed pin.");

            ColorFireplace = Plugin.Config.Bind("2. Colors", "Fireplace Color", "#d6b340", // Orange
                "Color for the first freely placeable pin, the fireplace.");
            ColorHouse = Plugin.Config.Bind("2. Colors", "House Color", "#35b5cc", // Cyan
                "Color for the second freely placeable pin, the house.");
            ColorHammer = Plugin.Config.Bind("2. Colors", "Hammer Color", "#7d7fad", // Light Dusk
                "Color for the third freely placeable pin, the hammer.");
            ColorBall = Plugin.Config.Bind("2. Colors", "Ball Color", "#d43d3d", // Red
                "Color for the fourth freely placeable pin, the ball.");
            ColorPortal = Plugin.Config.Bind("2. Colors", "Portal Color", "#a86840", // Brown
                "Color for the fifth freely placeable pin, the portal.");

            ColorBoss = Plugin.Config.Bind("2. Colors", "Boss Color", "#872ad4", // Purple
                "Color for the boss pin.");
            ColorPlayer = Plugin.Config.Bind("2. Colors", "Player Color", "#ffffff", // White
                "Color for the player pin.");
            ColorShout = Plugin.Config.Bind("2. Colors", "Shout Color", "#ffffff", // White
                "Color for the shout pin.");
            ColorRandomEvent = Plugin.Config.Bind("2. Colors", "Random Event Color", "#ffffff", // White
                "Color for the random event pin.");
            ColorPing = Plugin.Config.Bind("2. Colors", "Ping Color", "#ffffff", // White
                "Color for the ping pin.");
            ColorEventArea = Plugin.Config.Bind("2. Colors", "Event Area Color", "#ffffff", // White
                "Color for the event area pin.");

            LoggerEnable = Plugin.Config.Bind("3. Logger", "Enable", true,
                "Enables or disables debugging logs.");
        }
    }
}
