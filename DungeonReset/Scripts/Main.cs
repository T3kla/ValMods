using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace DungeonReset
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]
        public const string
            NAME = "DungeonReset",
            AUTHOR = "Tekla",
            GUID = AUTHOR + "_" + NAME,
            VERSION = "5.4.1502";

        internal readonly Harmony Harmony;
        internal readonly Assembly Assembly;
        internal readonly string Folder;
        #endregion

        internal static Main Instance;
        internal static ManualLogSource Log;

        public Main()
        {
            Log = new ManualLogSource(NAME);
            Harmony = new Harmony(GUID);
            Assembly = Assembly.GetExecutingAssembly();
            Folder = Path.GetDirectoryName(Assembly.Location);
            Instance = this;
        }

        public void Awake()
        {
            InitConfigs();

            if (Global.Config.DungeonResetEnable.Value)
                Harmony.PatchAll(Assembly);
        }

        public void InitConfigs()
        {
            Global.Config.DungeonResetEnable = Config.Bind("Dungeon Reset", "Enable", true,
                new ConfigDescription("Enables or disables dungeon regeneration.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Global.Config.DungeonResetInterval = Config.Bind("Dungeon Reset", "Interval", 82800f,
                new ConfigDescription("Set the amount of seconds it takes each dungeon to try to regenerate.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Global.Config.DungeonResetAllowedThemes = Config.Bind("Dungeon Reset", "Themes", "Crypt, SunkenCrypt, Cave, ForestCrypt",
                new ConfigDescription("Set allowed dungeon themes to reset. Possible themes are: Crypt, SunkenCrypt, Cave, ForestCrypt, GoblinCamp, MeadowsVillage, MeadowsFarm", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Global.Config.DungeonResetPlayerProtection = Config.Bind("Dungeon Reset", "Player Protection", true,
                new ConfigDescription("If enabled, dungeons won't reset while players are inside.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));
            Global.Config.DungeonResetPlayerProtectionInterval = Config.Bind("Dungeon Reset", "Player Protection Interval", 600f,
                new ConfigDescription("Time it takes to retry a reset on a dungeon that wasn't reset due to Player Protection.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = true }));

            Global.Config.LoggerEnable = Config.Bind("Logger", "Enable", true,
                new ConfigDescription("Enables or disables debugging logs.", null,
                new ConfigurationManagerAttributes { IsAdminOnly = false }));
            if (Global.Config.LoggerEnable.Value) BepInEx.Logging.Logger.Sources.Add(Log);
        }
    }
}
