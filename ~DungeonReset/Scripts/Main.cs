using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Jotunn.Utils;

namespace DungeonReset
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.ServerMustHaveMod, VersionStrictness.Minor)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]
        public const string NAME = "DungeonReset";
        public const string AUTHOR = "Tekla";
        public const string GUID = AUTHOR + "_" + NAME;
        public const string VERSION = "5.4.1606";
        #endregion

        internal static Main Instance;
        internal static ManualLogSource Log { get; private set; }
        internal static Harmony Harmony { get; private set; }
        internal static Assembly Assembly { get; private set; }
        internal static string Folder { get; private set; }

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
            Configs.Awake(this);

            if (Configs.CommandsEnable.Value)
                Commands.Awake();

            if (Configs.LoggerEnable.Value)
                BepInEx.Logging.Logger.Sources.Add(Log);

            if (Configs.Enable.Value)
                Harmony.PatchAll(Assembly);
        }
    }
}

// MeadowsFarm          => abandoned farm in meadows that spawns greyling and boar
// ForestCrypt          => black forest skeleton dungeon
// SunkenCrypt          => swamp draugr dungeon
// GoblinCamp           => plains funling camps
// Crypt                => 
// Cave                 => ! caves are handled by locations
// MeadowsVillage       => 
