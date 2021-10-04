using BepInEx.Configuration;
using UnityEngine;

namespace Areas
{
    public static class Configs
    {
        public static ConfigEntry<KeyCode> GUI_TogglePanel;
        public static ConfigEntry<KeyCode> GUI_ToggleMouse;
        public static ConfigEntry<string> GUI_DefaultPosition;
        public static ConfigEntry<string> GUI_DefaultSize;

        public static ConfigEntry<bool> LootEnable;
        public static ConfigEntry<int> LootFix;

        public static ConfigEntry<bool> LoggerEnable;

        public static void Awake(BepInEx.BaseUnityPlugin Plugin)
        {
            Plugin.Config.SaveOnConfigSet = true;

            GUI_TogglePanel = Plugin.Config.Bind("GUI", "Panel Toggle", KeyCode.PageUp,
                new ConfigDescription("Set the key binding to open and close Areas GUI.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            GUI_ToggleMouse = Plugin.Config.Bind("GUI", "Mouse Toggle", KeyCode.PageDown,
                new ConfigDescription("Set the key binding to show and hide mouse.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            GUI_DefaultPosition = Plugin.Config.Bind("GUI", "Default Position", "0:0",
                new ConfigDescription("Default position at which Areas GUI will appear.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            GUI_DefaultSize = Plugin.Config.Bind("GUI", "Default Size", "1600:800",
                new ConfigDescription("Default size at which Areas GUI will appear.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));

            LootEnable = Plugin.Config.Bind("Loot Fix", "Enable", true,
                new ConfigDescription("Enables or disables loot fixing.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            LootFix = Plugin.Config.Bind("Loot Fix", "Value", 10,
                new ConfigDescription("Number of levels it takes to get the next vanilla level reward. Only for Lv.3+. For example \"5\" will result in: [Lv.5 monster = Lv.4 reward] [Lv.10 monster = Lv.5 reward] [Lv.15 monster = Lv.6 reward]",
                new AcceptableValueRange<int>(1, 50),
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            LoggerEnable = Plugin.Config.Bind("Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            if (LoggerEnable.Value) BepInEx.Logging.Logger.Sources.Add(Main.Log);
        }
    }
}
