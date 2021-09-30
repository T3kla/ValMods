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

        internal readonly Harmony Harmony;
        internal readonly Assembly Assembly;
        internal readonly string Folder;

        public Main()
        {
            Log = new ManualLogSource(NAME);
            Harmony = new Harmony(GUID);
            Assembly = Assembly.GetExecutingAssembly();
            Folder = Path.GetDirectoryName(Assembly.Location);
            Instance = this;
        }

        private void Awake()
        {

            Configs.Awake(this);

            LoadDataFromDisk();

            OnDataLoaded += Variants.OnDataLoaded;
            OnDataReset += Critters.OnDataReset;
            OnDataReset += Variants.OnDataReset;

            // Areas.GUI.Awake();
            Spawners.Awake();
            // CommandHandler.Awake();

            Harmony.PatchAll(Assembly);
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

            Main.Log.LogInfo($"Instance is loading {source} Data\n");
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
            Main.Log.LogInfo($"Instance is resetting {source} Data\n");

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
