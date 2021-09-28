using System;
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
    public enum EDS { Local, Remote, Current }

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class Main : BaseUnityPlugin
    {
        public const string NAME = "Areas";
        public const string GUID = "Tekla_" + NAME;
        public const string VERSION = "1.1.3";

        public static Action OnDataLoaded;
        public static Action OnDataReset;

        public static Main Instance;
        public static ManualLogSource Log;

        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        public Main()
        {
            Log = new ManualLogSource(NAME);
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            Global.Path.ModFolder = Path.GetDirectoryName(assembly.Location);
        }

        private void Awake()
        {
            Instance = this;

            Configs();
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

            Global.Config.GUI_TogglePanel = Config.Bind("GUI", "Panel Toggle", KeyCode.PageUp,
                new ConfigDescription("Set the key binding to open and close Areas GUI.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            Global.Config.GUI_ToggleMouse = Config.Bind("GUI", "Mouse Toggle", KeyCode.PageDown,
                new ConfigDescription("Set the key binding to show and hide mouse.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            Global.Config.GUI_DefaultPosition = Config.Bind("GUI", "Default Position", "0:0",
                new ConfigDescription("Default position at which Areas GUI will appear.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            Global.Config.GUI_DefaultSize = Config.Bind("GUI", "Default Size", "1600:800",
                new ConfigDescription("Default size at which Areas GUI will appear.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));

            Global.Config.LootEnable = Config.Bind("Loot Fix", "Enable", true,
                new ConfigDescription("Enables or disables loot fixing.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Global.Config.LootFix = Config.Bind("Loot Fix", "Value", 10,
                new ConfigDescription("Number of levels it takes to get the next vanilla level reward. Only for Lv.3+. For example \"5\" will result in: [Lv.5 monster = Lv.4 reward] [Lv.10 monster = Lv.5 reward] [Lv.15 monster = Lv.6 reward]",
                new AcceptableValueRange<int>(1, 50),
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Global.Config.LoggerEnable = Config.Bind("Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            if (Global.Config.LoggerEnable.Value) BepInEx.Logging.Logger.Sources.Add(Log);
        }

        private void LoadDataFromDisk()
        {
            if (File.Exists(Global.Path.Areas)) Global.RawLocalData.Areas = File.ReadAllText(Global.Path.Areas);
            if (File.Exists(Global.Path.CTData)) Global.RawLocalData.CTData = File.ReadAllText(Global.Path.CTData);
            if (File.Exists(Global.Path.VAData)) Global.RawLocalData.VAData = File.ReadAllText(Global.Path.VAData);
            if (File.Exists(Global.Path.SSData)) Global.RawLocalData.SSData = File.ReadAllText(Global.Path.SSData);
            if (File.Exists(Global.Path.CSData)) Global.RawLocalData.CSData = File.ReadAllText(Global.Path.CSData);
            if (File.Exists(Global.Path.SAData)) Global.RawLocalData.SAData = File.ReadAllText(Global.Path.SAData);
        }

        public static void LoadData(EDS source)
        {
            RawData data = source == EDS.Remote ? Global.RawRemoteData : Global.RawLocalData;

            Main.Log.LogInfo($"Instance is loading {source} Data");
            Global.CurrentData.Areas = Serialization.Deserialize<Dictionary<string, Area>>(data.Areas);
            Global.CurrentData.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTData>>>(data.CTData);
            Global.CurrentData.VAMods = Serialization.Deserialize<Dictionary<string, VAData>>(data.VAData);
            Global.CurrentData.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSData>>>(data.SSData);
            Global.CurrentData.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSData>>>(data.CSData);
            Global.CurrentData.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAData>>>(data.SAData);

            if (OnDataLoaded != null) OnDataLoaded.Invoke();
        }

        public static void ResetData(EDS source)
        {
            Main.Log.LogInfo($"Instance is resetting {source} Data");

            switch (source)
            {
                case EDS.Local:
                    Global.RawLocalData = new RawData();
                    break;
                case EDS.Remote:
                    Global.RawRemoteData = new RawData();
                    break;
                case EDS.Current:
                    Global.CurrentData = new Data();
                    if (OnDataReset != null) OnDataReset.Invoke();
                    break;
                default:
                    break;
            }
        }
    }
}
