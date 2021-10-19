using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Areas.Containers;
using Areas.TYaml;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Jotunn.Utils;
using static Areas.Containers.RawData;

namespace Areas
{
    public enum EDS { Local, Remote, Current }

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]
        public const string NAME = "DungeonReset";
        public const string AUTHOR = "Tekla";
        public const string GUID = AUTHOR + "_" + NAME;
        public const string VERSION = "5.4.1600";
        #endregion

        internal static Main Instance { get; private set; }
        internal static ManualLogSource Log = new ManualLogSource(NAME);
        internal static Harmony Harmony = new Harmony(GUID);
        internal static Assembly Assembly = Assembly.GetExecutingAssembly();
        internal static string Folder = Path.GetDirectoryName(Assembly.Location);

        public static Action OnDataLoaded;
        public static Action OnDataReset;

        public Main() => Instance = this;

        private void Awake()
        {
            Configs.Awake(this);

            LoadDataFromDisk();

            // OnDataLoaded += Variants.OnDataLoaded;
            // OnDataReset += Critters.OnDataReset;
            // OnDataReset += Variants.OnDataReset;

            // Areas.GUI.Awake();
            // Spawners.Awake();
            // CommandHandler.Awake();

            Harmony.PatchAll(Assembly);
        }

        private static void LoadDataFromDisk()
            => RawData.Loc = new(
                areas: File.Exists(Global.Path.Areas) ? File.ReadAllText(Global.Path.Areas) : "",
                cTData: File.Exists(Global.Path.CTData) ? File.ReadAllText(Global.Path.CTData) : "",
                vAData: File.Exists(Global.Path.VAData) ? File.ReadAllText(Global.Path.VAData) : "",
                sSData: File.Exists(Global.Path.SSData) ? File.ReadAllText(Global.Path.SSData) : "",
                cSData: File.Exists(Global.Path.CSData) ? File.ReadAllText(Global.Path.CSData) : "",
                sAData: File.Exists(Global.Path.SAData) ? File.ReadAllText(Global.Path.SAData) : ""
            );

        public static void LoadData(EDS type)
        {
            ref readonly var raw = ref RawData.Get(type);

            Main.Log.LogInfo($"Instance is loading {type} Data\n");
            Global.CurrentData.Areas = Serialization.Deserialize<Dictionary<string, Area>>(raw.Areas);
            Global.CurrentData.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, DataCritter>>>(raw.CTData);
            Global.CurrentData.VAMods = Serialization.Deserialize<Dictionary<string, DataVariant>>(raw.VAData);
            Global.CurrentData.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, DataSS>>>(raw.SSData);
            Global.CurrentData.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSData>>>(raw.CSData);
            Global.CurrentData.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAData>>>(raw.SAData);

            if (OnDataLoaded != null) OnDataLoaded.Invoke();
        }

        // public static void ResetData(EDS source)
        // {
        //     Main.Log.LogInfo($"Instance is resetting {source} Data\n");

        //     switch (source)
        //     {
        //         case EDS.Local:
        //             Global.RawLocalData = new RawData();
        //             break;
        //         case EDS.Remote:
        //             Global.RawRemoteData = new RawData();
        //             break;
        //         case EDS.Current:
        //             Global.CurrentData = new Data();
        //             if (OnDataReset != null) OnDataReset.Invoke();
        //             break;
        //         default:
        //             break;
        //     }
        // }
    }
}
