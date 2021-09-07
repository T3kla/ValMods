using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Areas.Containers;
using Areas.TYaml;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using Jotunn.Utils;
using UnityEngine;

namespace Areas
{

    public delegate void DVoid();
    public enum EDS { Local, Remote, Current }

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class Main : BaseUnityPlugin
    {
        public const string NAME = "Areas";
        public const string GUID = "Tekla_" + NAME;
        public const string VERSION = "1.1.1";

        public static DVoid OnDataLoaded;
        public static DVoid OnDataReset;

        public static Main Instance;
        public static ManualLogSource GLog;

        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        public Main()
        {
            GLog = new ManualLogSource(NAME + ".G");
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
        }

        private void Awake()
        {
            Instance = this;

            Configs();

            Globals.Path.Assembly = Path.GetDirectoryName(assembly.Location);
            LoadDataFromDisk();

            OnDataLoaded += VariantsHandler.OnDataLoaded;
            OnDataReset += CritterHandler.OnDataReset;
            OnDataReset += VariantsHandler.OnDataReset;

            // Areas.GUI.Awake();
            SpawnerHandler.Awake();
            // CommandHandler.Awake();

            harmony.PatchAll(assembly);
        }

        private void Configs()
        {
            Config.SaveOnConfigSet = true;

            Globals.Config.GUI_TogglePanel = Config.Bind("GUI", "Panel Toggle", KeyCode.PageUp,
                new ConfigDescription("Set the key binding to open and close Areas GUI.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            Globals.Config.GUI_ToggleMouse = Config.Bind("GUI", "Mouse Toggle", KeyCode.PageDown,
                new ConfigDescription("Set the key binding to show and hide mouse.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            Globals.Config.GUI_DefaultPosition = Config.Bind("GUI", "Default Position", "0:0",
                new ConfigDescription("Default position at which Areas GUI will appear.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            Globals.Config.GUI_DefaultSize = Config.Bind("GUI", "Default Size", "1600:800",
                new ConfigDescription("Default size at which Areas GUI will appear.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));

            Globals.Config.LootEnable = Config.Bind("Loot Fix", "Enable", true,
                new ConfigDescription("Enables or disables loot fixing.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Globals.Config.LootFix = Config.Bind("Loot Fix", "Value", 10,
                new ConfigDescription("Number of levels it takes to get the next vanilla level reward. Only for Lv.3+. For example \"5\" will result in: [Lv.5 monster = Lv.4 reward] [Lv.10 monster = Lv.5 reward] [Lv.15 monster = Lv.6 reward]",
                new AcceptableValueRange<int>(1, 50),
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Globals.Config.DungeonRegenEnable = Config.Bind("Dungeon Regen", "Enable", false,
                new ConfigDescription("Enables or disables Dungeon Regeneration.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Globals.Config.DungeonRegenCooldown = Config.Bind("Dungeon Regen", "Cooldown", 60,
                new ConfigDescription("Set the amount of minutes it takes each dungeon to try to regenerate.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Globals.Config.DungeonRegenAllowedThemes = Config.Bind("Dungeon Regen", "Themes", "Crypt, SunkenCrypt, Cave, ForestCrypt",
                new ConfigDescription("Set allowed dungeon themes to regen. Possible themes are: Crypt, SunkenCrypt, Cave, ForestCrypt", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Globals.Config.DungeonRegenPlayerProtection = Config.Bind("Dungeon Regen", "Player Protection", true,
                new ConfigDescription("If enabled, Dungeons won't regen while players are inside.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Globals.Config.LoggerEnable = Config.Bind("Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));

            if (Globals.Config.LoggerEnable.Value) BepInEx.Logging.Logger.Sources.Add(GLog);
        }


        // ----------------------------------------------------------------------------------------------------------------------------------- LOCAL
        private void LoadDataFromDisk()
        {
            if (File.Exists(Globals.Path.Areas)) Globals.RawLocalData.Areas = File.ReadAllText(Globals.Path.Areas);
            if (File.Exists(Globals.Path.CTData)) Globals.RawLocalData.CTData = File.ReadAllText(Globals.Path.CTData);
            if (File.Exists(Globals.Path.VAData)) Globals.RawLocalData.VAData = File.ReadAllText(Globals.Path.VAData);
            if (File.Exists(Globals.Path.SSData)) Globals.RawLocalData.SSData = File.ReadAllText(Globals.Path.SSData);
            if (File.Exists(Globals.Path.CSData)) Globals.RawLocalData.CSData = File.ReadAllText(Globals.Path.CSData);
            if (File.Exists(Globals.Path.SAData)) Globals.RawLocalData.SAData = File.ReadAllText(Globals.Path.SAData);
        }

        public static void LoadData(EDS source)
        {
            RawData data = source == EDS.Remote ? Globals.RawRemoteData : Globals.RawLocalData;

            Main.GLog.LogInfo($"Instance is loading {source} Data");
            Globals.CurrentData.Areas = Serialization.Deserialize<Dictionary<string, Area>>(data.Areas);
            Globals.CurrentData.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTData>>>(data.CTData);
            Globals.CurrentData.VAMods = Serialization.Deserialize<Dictionary<string, VAData>>(data.VAData);
            Globals.CurrentData.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSData>>>(data.SSData);
            Globals.CurrentData.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSData>>>(data.CSData);
            Globals.CurrentData.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAData>>>(data.SAData);

            if (OnDataLoaded != null) OnDataLoaded.Invoke();
        }


        public static void ResetData(EDS source)
        {
            Main.GLog.LogInfo($"Instance is resetting {source} Data");

            switch (source)
            {
                case EDS.Local:
                    Globals.RawLocalData = new RawData();
                    break;
                case EDS.Remote:
                    Globals.RawRemoteData = new RawData();
                    break;
                case EDS.Current:
                    Globals.CurrentData = new Data();
                    if (OnDataReset != null) OnDataReset.Invoke();
                    break;
                default:
                    break;
            }
        }

    }

}
