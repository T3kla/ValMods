﻿using System.IO;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Areas.TJson;
using Areas.Containers;
using UnityEngine;

namespace Areas
{

    public delegate void DVoid();

    [BepInPlugin(GUID, NAME, VERSION)]
    public class Main : BaseUnityPlugin
    {

        public static DVoid OnDataLoaded;
        public static DVoid OnDataReset;

        private const string NAME = "Areas";
        private const string GUID = "Tekla_" + NAME;
        private const string VERSION = "1.0.0";

        public static Main Instance;
        public static ManualLogSource Log = new ManualLogSource(NAME);

        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        public Main()
        {

            Log = new ManualLogSource("MyLogSource");

            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);

        }

        private void Awake()
        {

            Instance = this;
            Globals.Path.Assembly = Path.GetDirectoryName(assembly.Location);
            Local_ReadFromDisk();

            OnDataLoaded += CritterHandler.Generate_CTMatDic;

            OnDataReset += CritterHandler.ResetData;
            OnDataReset += SpawnerHandler.ResetData;

            Configs();

            if (Globals.Config.Debug.Value)
                BepInEx.Logging.Logger.Sources.Add(Log);
            else
                BepInEx.Logging.Logger.Sources.Remove(Log);

            harmony.PatchAll(assembly);

        }

        public void Configs()
        {

            Globals.Config.Debug = Config.Bind(
                "General",
                "Debug Logs",
                false,
                "Enables or disables debug logs.");

        }


        // ----------------------------------------------------------------------------------------------------------------------------------- LOCAL
        private void Local_ReadFromDisk()
        {
            if (File.Exists(Globals.Path.Areas)) Globals.LocalRaw.Areas = File.ReadAllText(Globals.Path.Areas);
            if (File.Exists(Globals.Path.CTData)) Globals.LocalRaw.CTData = File.ReadAllText(Globals.Path.CTData);
            if (File.Exists(Globals.Path.SSData)) Globals.LocalRaw.SSData = File.ReadAllText(Globals.Path.SSData);
            if (File.Exists(Globals.Path.CSData)) Globals.LocalRaw.CSData = File.ReadAllText(Globals.Path.CSData);
            if (File.Exists(Globals.Path.SAData)) Globals.LocalRaw.SAData = File.ReadAllText(Globals.Path.SAData);
        }

        public static void Local_LoadData()
        {
            Main.Log.LogInfo($"Instance is loading Local Data");
            Globals.Areas = Serialization.Deserialize<Dictionary<string, Area>>(Globals.LocalRaw.Areas);
            Globals.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTMods>>>(Globals.LocalRaw.CTData);
            Globals.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSMods>>>(Globals.LocalRaw.SSData);
            Globals.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSMods>>>(Globals.LocalRaw.CSData);
            Globals.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAMods>>>(Globals.LocalRaw.SAData);

            if (OnDataLoaded != null) OnDataLoaded.Invoke();
        }

        public static void Local_ResetData()
        {
            Main.Log.LogInfo($"Instance is reseting Local Data");
            Globals.LocalRaw.Areas = "{}";
            Globals.LocalRaw.CTData = "{}";
            Globals.LocalRaw.SSData = "{}";
            Globals.LocalRaw.CSData = "{}";
            Globals.LocalRaw.SAData = "{}";
        }


        // ----------------------------------------------------------------------------------------------------------------------------------- REMOTE
        public static void Remote_LoadData()
        {
            Main.Log.LogInfo($"Instance is loading Remote Data");
            Globals.Areas = Serialization.Deserialize<Dictionary<string, Area>>(Globals.RemoteRaw.Areas);
            Globals.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTMods>>>(Globals.RemoteRaw.CTData);
            Globals.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSMods>>>(Globals.RemoteRaw.SSData);
            Globals.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSMods>>>(Globals.RemoteRaw.CSData);
            Globals.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAMods>>>(Globals.RemoteRaw.SAData);

            if (OnDataLoaded != null) OnDataLoaded.Invoke();
        }

        public static void Remote_ResetData()
        {
            Main.Log.LogInfo($"Instance is reseting Remote Data");
            Globals.RemoteRaw.Areas = "{}";
            Globals.RemoteRaw.CTData = "{}";
            Globals.RemoteRaw.SSData = "{}";
            Globals.RemoteRaw.CSData = "{}";
            Globals.RemoteRaw.SAData = "{}";
        }


        // ----------------------------------------------------------------------------------------------------------------------------------- CURRENT
        public static void Current_ResetData()
        {
            Main.Log.LogInfo($"Instance is reseting Current Data");
            Globals.Areas = new Dictionary<string, Area>();
            Globals.CTMods = new Dictionary<string, Dictionary<string, CTMods>>();
            Globals.SSMods = new Dictionary<string, Dictionary<int, SSMods>>();
            Globals.CSMods = new Dictionary<string, Dictionary<string, CSMods>>();
            Globals.SAMods = new Dictionary<string, Dictionary<string, SAMods>>();

            if (OnDataReset != null) OnDataReset.Invoke();
        }

    }

}
