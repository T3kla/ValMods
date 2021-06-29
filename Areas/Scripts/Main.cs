using System.IO;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using Areas.Containers;
using Areas.TYaml;
using Jotunn.Utils;

namespace Areas
{

    public delegate void DVoid();
    public enum EDS { Local, Remote, Current }

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class Main : BaseUnityPlugin
    {

        public static DVoid OnDataLoaded;
        public static DVoid OnDataReset;

        private const string NAME = "Areas";
        private const string GUID = "Tekla_" + NAME;
        private const string VERSION = "1.1.0";

        public static Main Instance;
        public static ManualLogSource GLog;

        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        public Main()
        {

            GLog = new ManualLogSource(NAME + ".General");
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);

        }

        private void Awake()
        {

            Instance = this;
            Globals.Path.Assembly = Path.GetDirectoryName(assembly.Location);
            LoadDataFromDisk();

            OnDataLoaded += SpawnerHandler.OnDataLoaded;
            OnDataLoaded += VariantsHandler.OnDataLoaded;

            OnDataReset += CritterHandler.OnDataReset;
            OnDataReset += SpawnerHandler.OnDataReset;
            OnDataReset += VariantsHandler.OnDataReset;

            Configs();

            CommandHandler.Awake();

            if (Globals.Config.LoggerEnable.Value)
                BepInEx.Logging.Logger.Sources.Add(GLog);

            harmony.PatchAll(assembly);

        }

        public void Configs()
        {

            Config.SaveOnConfigSet = true;

            Globals.Config.LootEnable = Config.Bind("Loot Fix", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.",
                null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Globals.Config.LootFix = Config.Bind("Loot Fix", "value", 10,
                new ConfigDescription("Number of levels it takes to get the next vanilla level reward. Only for Lv.3+. For example \"5\" will result in: [Lv.5 monster = Lv.4 reward] [Lv.10 monster = Lv.5 reward] [Lv.15 monster = Lv.6 reward]",
                new AcceptableValueRange<int>(1, 50),
                new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Globals.Config.DungeonRegenEnable = Config.Bind(
                "Dungeon Regen", "Enable", false,
                new ConfigDescription("Enables or disables Dungeon Regeneration.",
                null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Globals.Config.DungeonRegenCooldown = Config.Bind(
                "Dungeon Regen", "Cooldown", 60,
                new ConfigDescription("Set the amount of minutes it takes each dungeon to try to regenerate.",
                null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Globals.Config.DungeonRegenAllowedThemes = Config.Bind(
                "Dungeon Regen", "Themes", "Crypt, SunkenCrypt, Cave, ForestCrypt",
                new ConfigDescription("Set allowed dungeon themes to regen. Possible themes are: Crypt, SunkenCrypt, Cave, ForestCrypt",
                null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Globals.Config.DungeonRegenPlayerProtection = Config.Bind(
                "Dungeon Regen", "Player Protection", true,
                new ConfigDescription("If enabled, Dungeons won't regen while players are inside.",
                null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));


            Globals.Config.LoggerEnable = Config.Bind(
                "Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.",
                null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));

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

            Main.GLog.LogInfo($"Instance is loading {source.ToString()} Data");
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
            Main.GLog.LogInfo($"Instance is resetting {source.ToString()} Data");

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
