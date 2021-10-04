using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace BuildingUnleashed
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]
        public const string
            MODNAME = "BuildingUnleashed",
            AUTHOR = "Tekla",
            GUID = AUTHOR + "_" + MODNAME,
            VERSION = "5.4.1600";

        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        #endregion

        public Main()
        {
            log = Logger;
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
        }

        public void Start()
        {
            InitializeConfig();
            harmony.PatchAll(assembly);
        }

        public void InitializeConfig()
        {
            Globals.configPlaceDelay = Config.Bind(
                "General",
                "Place Delay",
                0.3f,
                "Changes the default delay value for building pieces or using tools. Official value is 0.4");

            Globals.configRemoveDelay = Config.Bind(
                "General",
                "Remove Delay",
                0.25f,
                "Changes the default default value for removing pieces. Official value is 0.25");
        }
    }
}
