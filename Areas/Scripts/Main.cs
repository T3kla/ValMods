using System.IO;
using System.Collections.Generic;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModConfigEnforcer;
using Areas.Containers;
using Areas.TYaml;

namespace Areas
{

    public delegate void DVoid();

    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency("pfhoenix.modconfigenforcer", BepInDependency.DependencyFlags.HardDependency)]
    public class Main : BaseUnityPlugin
    {

        public static DVoid OnDataLoaded;
        public static DVoid OnDataReset;

        private const string NAME = "Areas";
        private const string GUID = "Tekla_" + NAME;
        private const string VERSION = "1.0.0";

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
            Local_ReadFromDisk();

            OnDataLoaded += SpawnerHandler.Set_SSDataDicFlag;

            OnDataReset += CritterHandler.ResetData;
            OnDataReset += SpawnerHandler.ResetData;

            ConfigManager.RegisterMod(GUID, Config);

            Configs();

            if (Globals.Config.Debug.Value)
                BepInEx.Logging.Logger.Sources.Add(GLog);
            else
                BepInEx.Logging.Logger.Sources.Remove(GLog);

            harmony.PatchAll(assembly);

        }

        public void Configs()
        {

            Globals.Config.Loot = ConfigManager.RegisterModConfigVariable<int>(GUID,
                "Loot Fix", 10,
                "General", "Number of levels it takes to get the next vanilla level reward. Only works for LvL3+. For example a value of 5 will result in: LvL5 monster = LvL4 reward, LvL10 monster = LvL5 reward, LvL15 monster = LvL6 reward.",
                false);


            Globals.Config.DungeonRegenEnable = ConfigManager.RegisterModConfigVariable<bool>(GUID,
                "Enable", false,
                "Dungeon Regen", "Enables or disables Dungeon Regeneration.",
                false);

            Globals.Config.DungeonRegenCooldown = ConfigManager.RegisterModConfigVariable<long>(GUID,
                "Cooldown", 60,
                "Dungeon Regen", "Set the amount of minutes it takes each dungeon to try to regenerate.",
                false);

            Globals.Config.DungeonRegenAllowedThemes = ConfigManager.RegisterModConfigVariable<string>(GUID,
                "Themes", "Crypt, SunkenCrypt, Cave, ForestCrypt",
                "Dungeon Regen", "Set allowed dungeon themes to regen. Possible themes are: Crypt, SunkenCrypt, Cave, ForestCrypt",
                false);

            Globals.Config.DungeonRegenPlayerProtection = ConfigManager.RegisterModConfigVariable<bool>(GUID,
                "Player Protection", true,
                "Dungeon Regen", "If enabled, Dungeons won't regen while players are inside.",
                false);


            Globals.Config.Debug = ConfigManager.RegisterModConfigVariable<bool>(GUID,
                "Enable", true,
                "Logger", "Enables or disables debugging logs.",
                true);

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
            Main.GLog.LogInfo($"Instance is loading Local Data");
            Globals.Areas = Serialization.Deserialize<Dictionary<string, Area>>(Globals.LocalRaw.Areas);
            Globals.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTData>>>(Globals.LocalRaw.CTData);
            Globals.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSData>>>(Globals.LocalRaw.SSData);
            Globals.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSData>>>(Globals.LocalRaw.CSData);
            Globals.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAData>>>(Globals.LocalRaw.SAData);

            if (OnDataLoaded != null) OnDataLoaded.Invoke();
        }

        public static void Local_ResetData()
        {
            Main.GLog.LogInfo($"Instance is reseting Local Data");
            Globals.LocalRaw.Areas = "{}";
            Globals.LocalRaw.CTData = "{}";
            Globals.LocalRaw.SSData = "{}";
            Globals.LocalRaw.CSData = "{}";
            Globals.LocalRaw.SAData = "{}";
        }


        // ----------------------------------------------------------------------------------------------------------------------------------- REMOTE
        public static void Remote_LoadData()
        {
            Main.GLog.LogInfo($"Instance is loading Remote Data");
            Globals.Areas = Serialization.Deserialize<Dictionary<string, Area>>(Globals.RemoteRaw.Areas);
            Globals.CTMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CTData>>>(Globals.RemoteRaw.CTData);
            Globals.SSMods = Serialization.Deserialize<Dictionary<string, Dictionary<int, SSData>>>(Globals.RemoteRaw.SSData);
            Globals.CSMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, CSData>>>(Globals.RemoteRaw.CSData);
            Globals.SAMods = Serialization.Deserialize<Dictionary<string, Dictionary<string, SAData>>>(Globals.RemoteRaw.SAData);

            if (OnDataLoaded != null) OnDataLoaded.Invoke();
        }

        public static void Remote_ResetData()
        {
            Main.GLog.LogInfo($"Instance is reseting Remote Data");
            Globals.RemoteRaw.Areas = "{}";
            Globals.RemoteRaw.CTData = "{}";
            Globals.RemoteRaw.SSData = "{}";
            Globals.RemoteRaw.CSData = "{}";
            Globals.RemoteRaw.SAData = "{}";
        }


        // ----------------------------------------------------------------------------------------------------------------------------------- CURRENT
        public static void Current_ResetData()
        {
            Main.GLog.LogInfo($"Instance is reseting Current Data");
            Globals.Areas = new Dictionary<string, Area>();
            Globals.CTMods = new Dictionary<string, Dictionary<string, CTData>>();
            Globals.SSMods = new Dictionary<string, Dictionary<int, SSData>>();
            Globals.CSMods = new Dictionary<string, Dictionary<string, CSData>>();
            Globals.SAMods = new Dictionary<string, Dictionary<string, SAData>>();

            if (OnDataReset != null) OnDataReset.Invoke();
        }

    }

}
