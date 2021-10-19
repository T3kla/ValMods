using BepInEx.Configuration;
using UnityEngine;

namespace QoLPins
{
    public static class Configs
    {
        public static ConfigEntry<bool> Enable { get; private set; }

        public static ConfigEntry<bool> DontPinWhenInvIsEmpty { get; private set; }
        public static ConfigEntry<bool> RemoveAtRetrieval { get; private set; }
        public static ConfigEntry<bool> AutoPinCopper { get; private set; }

        public static ConfigEntry<bool> EnableColors { get; private set; }
        public static ConfigEntry<string> ColorDeath { get; private set; }
        public static ConfigEntry<string> ColorBed { get; private set; }
        public static ConfigEntry<string> ColorFireplace { get; private set; }
        public static ConfigEntry<string> ColorHouse { get; private set; }
        public static ConfigEntry<string> ColorHammer { get; private set; }
        public static ConfigEntry<string> ColorBall { get; private set; }
        public static ConfigEntry<string> ColorCave { get; private set; }
        public static ConfigEntry<string> ColorBoss { get; private set; }
        public static ConfigEntry<string> ColorPlayer { get; private set; }
        public static ConfigEntry<string> ColorShout { get; private set; }
        public static ConfigEntry<string> ColorRandomEvent { get; private set; }
        public static ConfigEntry<string> ColorPing { get; private set; }
        public static ConfigEntry<string> ColorEventArea { get; private set; }

        public static ConfigEntry<bool> EnableAutoPin { get; private set; }
        public static ConfigEntry<string> AutoTin { get; private set; }
        public static ConfigEntry<string> AutoCopper { get; private set; }
        public static ConfigEntry<string> AutoSilver { get; private set; }
        public static ConfigEntry<string> AutoDungeon { get; private set; }

        public static ConfigEntry<bool> LoggerEnable { get; private set; }

        public static Color Lit = new Color(0.96f, 0.33f, 0.23f, 1f);

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            #region 1. General
            Enable = Plugin.Config.Bind("1. General", "Enable", true,
                new ConfigDescription("Enables or disables mod.", null,
                new ConfigurationManagerAttributes { Order = 0, EntryColor = Lit }));

            DontPinWhenInvIsEmpty = Plugin.Config.Bind("1. General", "Generate with empty inventory", true,
                new ConfigDescription("Death Pin will won't be generated if your inventory was empty.", null,
                new ConfigurationManagerAttributes { Order = 1 }));
            RemoveAtRetrieval = Plugin.Config.Bind("1. General", "Remove at retrieval", true,
                new ConfigDescription("Death Pin will dissapear automatically when Tombstone is retrieved.", null,
                new ConfigurationManagerAttributes { Order = 2 }));
            #endregion

            #region 2. Colors
            EnableColors = Plugin.Config.Bind("2. Colors", "Enable", true,
                new ConfigDescription("Enables or disables pin colorization", null,
                new ConfigurationManagerAttributes { Order = 3, EntryColor = Lit }));

            ColorDeath = Plugin.Config.Bind("2. Colors", "Death Color", "#d43d3d", // Red
                new ConfigDescription("Color for the death pin.", null,
                new ConfigurationManagerAttributes { Order = 4, DefaultValue = "#d43d3d" }));
            ColorBed = Plugin.Config.Bind("2. Colors", "Bed Color", "#35b5cc", // Cyan
                new ConfigDescription("Color for the bed pin.", null,
                new ConfigurationManagerAttributes { Order = 5, DefaultValue = "#35b5cc" }));

            ColorFireplace = Plugin.Config.Bind("2. Colors", "Fireplace Color", "#d6b340", // Orange
                new ConfigDescription("Color for the first freely placeable pin: Fireplace.", null,
                new ConfigurationManagerAttributes { Order = 6, DefaultValue = "#d6b340" }));
            ColorHouse = Plugin.Config.Bind("2. Colors", "House Color", "#35b5cc", // Cyan
                new ConfigDescription("Color for the second freely placeable pin: House.", null,
                new ConfigurationManagerAttributes { Order = 7, DefaultValue = "#35b5cc" }));
            ColorHammer = Plugin.Config.Bind("2. Colors", "Hammer Color", "#737373", // Light Dusk
                new ConfigDescription("Color for the third freely placeable pin: Hammer.", null,
                new ConfigurationManagerAttributes { Order = 8, DefaultValue = "#737373" }));
            ColorBall = Plugin.Config.Bind("2. Colors", "Ball Color", "#c95151", // Red
                new ConfigDescription("Color for the fourth freely placeable pin: Ball.", null,
                new ConfigurationManagerAttributes { Order = 9, DefaultValue = "#c95151" }));
            ColorCave = Plugin.Config.Bind("2. Colors", "Cave Color", "#a86840", // Brown
                new ConfigDescription("Color for the fifth freely placeable pin: Cave.", null,
                new ConfigurationManagerAttributes { Order = 10, DefaultValue = "#a86840" }));

            ColorBoss = Plugin.Config.Bind("2. Colors", "Boss Color", "#9c39ed", // Purple
                new ConfigDescription("Color for the boss pin.", null,
                new ConfigurationManagerAttributes { Order = 11, DefaultValue = "#9c39ed" }));
            ColorPlayer = Plugin.Config.Bind("2. Colors", "Player Color", "#ffffff", // Purple
                new ConfigDescription("Color for the player pin.", null,
                new ConfigurationManagerAttributes { Order = 12, DefaultValue = "#ffffff" }));
            ColorShout = Plugin.Config.Bind("2. Colors", "Shout Color", "#ffffff", // White
                new ConfigDescription("Color for the shout pin.", null,
                new ConfigurationManagerAttributes { Order = 13, DefaultValue = "#ffffff" }));
            ColorRandomEvent = Plugin.Config.Bind("2. Colors", "Random Event Color", "#ffffff", // White
                new ConfigDescription("Color for the random event pin.", null,
                new ConfigurationManagerAttributes { Order = 14, DefaultValue = "#ffffff" }));
            ColorPing = Plugin.Config.Bind("2. Colors", "Ping Color", "#ffffff", // White
                new ConfigDescription("Color for the ping pin.", null,
                new ConfigurationManagerAttributes { Order = 15, DefaultValue = "#ffffff" }));
            ColorEventArea = Plugin.Config.Bind("2. Colors", "Event Area Color", "#ffffff", // White
                new ConfigDescription("Color for the event area pin.", null,
                new ConfigurationManagerAttributes { Order = 16, DefaultValue = "#ffffff" }));
            #endregion

            #region 3. AutoPin
            EnableAutoPin = Plugin.Config.Bind("3. AutoPin", "Enable", true,
                new ConfigDescription("Enables or disables automatic pin placement.", null,
                new ConfigurationManagerAttributes { Order = 17, EntryColor = Lit }));

            AutoTin = Plugin.Config.Bind("3. AutoPin", "Autopin Tin ore", "Hammer:Tin",
                new ConfigDescription("Pin tin when hit with something. Type of pin at the left, pin name at the right.", null,
                new ConfigurationManagerAttributes { Order = 18, DefaultValue = "Hammer:Tin" }));
            AutoCopper = Plugin.Config.Bind("3. AutoPin", "Autopin Copper ore", "Hammer:Copper",
                new ConfigDescription("Pin copper when hit with something. Type of pin at the left, pin name at the right.", null,
                new ConfigurationManagerAttributes { Order = 19, DefaultValue = "Hammer:Copper" }));
            AutoSilver = Plugin.Config.Bind("3. AutoPin", "Autopin Silver ore", "Hammer:Silver",
                new ConfigDescription("Pin silver when hit with something. Type of pin at the left, pin name at the right.", null,
                new ConfigurationManagerAttributes { Order = 20, DefaultValue = "Hammer:Silver" }));
            AutoDungeon = Plugin.Config.Bind("3. AutoPin", "Autopin Dungeons", "Cave:",
                new ConfigDescription("Pin dungeons when interacting with their entrance. Leave right side unassigned for automatic naming.", null,
                new ConfigurationManagerAttributes { Order = 21, DefaultValue = "Cave:" }));
            #endregion

            #region 4. Logger
            LoggerEnable = Plugin.Config.Bind("4. Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { Order = 99, EntryColor = Lit }));
            #endregion

            Plugin.Config.SaveOnConfigSet = true;
            Plugin.Config.SettingChanged += (a, b) => PinColor.UpdateColorLib();
            Plugin.Config.SettingChanged += (a, b) => PinAuto.UpdateAutoPinData();
        }
    }
}
