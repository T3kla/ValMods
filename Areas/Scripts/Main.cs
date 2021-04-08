using System.IO;
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

    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "Areas",
            AUTHOR = "",
            GUID = AUTHOR + "_" + MODNAME,
            VERSION = "1.0.0.0";

        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;

        public Main()
        {

            log = Logger;
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);

        }

        #endregion

        internal static Main Instance;

        private void Awake()
        {

            Instance = this;
            Globals.Path.Assembly = Path.GetDirectoryName(assembly.Location);
            Local_ReadFromDisk();
            harmony.PatchAll(assembly);

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
            Debug.Log($"[Areas] Instance is loading Local Data");
            Globals.Areas = Serialization.Deserialize<Dictionary<string, Area>>(Globals.LocalRaw.Areas);
            Globals.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTMods>>>(Globals.LocalRaw.CTData);
            Globals.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSMods>>>(Globals.LocalRaw.SSData);
            Globals.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSMods>>>(Globals.LocalRaw.CSData);
            Globals.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAMods>>>(Globals.LocalRaw.SAData);
        }

        public static void Local_ResetData()
        {
            Debug.Log($"[Areas] Instance is reseting Local Data");
            Globals.LocalRaw.Areas = "{}";
            Globals.LocalRaw.CTData = "{}";
            Globals.LocalRaw.SSData = "{}";
            Globals.LocalRaw.CSData = "{}";
            Globals.LocalRaw.SAData = "{}";
        }


        // ----------------------------------------------------------------------------------------------------------------------------------- REMOTE
        public static void Remote_LoadData()
        {
            Debug.Log($"[Areas] Instance is loading Remote Data");
            Globals.Areas = Serialization.Deserialize<Dictionary<string, Area>>(Globals.RemoteRaw.Areas);
            Globals.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTMods>>>(Globals.RemoteRaw.CTData);
            Globals.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSMods>>>(Globals.RemoteRaw.SSData);
            Globals.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSMods>>>(Globals.RemoteRaw.CSData);
            Globals.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAMods>>>(Globals.RemoteRaw.SAData);
        }

        public static void Remote_ResetData()
        {
            Debug.Log($"[Areas] Instance is reseting Remote Data");
            Globals.RemoteRaw.Areas = "{}";
            Globals.RemoteRaw.CTData = "{}";
            Globals.RemoteRaw.SSData = "{}";
            Globals.RemoteRaw.CSData = "{}";
            Globals.RemoteRaw.SAData = "{}";
        }


        // ----------------------------------------------------------------------------------------------------------------------------------- CURRENT
        public static void Current_ResetData()
        {
            Debug.Log($"[Areas] Instance is reseting Current Data");
            Globals.Areas = new Dictionary<string, Area>();
            Globals.CTMods = new Dictionary<string, Dictionary<string, CTMods>>();
            Globals.SSMods = new Dictionary<string, Dictionary<int, SSMods>>();
            Globals.CSMods = new Dictionary<string, Dictionary<string, CSMods>>();
            Globals.SAMods = new Dictionary<string, Dictionary<string, SAMods>>();
        }

    }

}
