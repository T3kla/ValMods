using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace BuidlingUnleashed
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "BuildingUnleashed",
            AUTHOR = "Tekla",
            GUID = AUTHOR + "_" + MODNAME,
            VERSION = "1.0.0.0";

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
            Globals.configToolDelay = Config.Bind(
                "General",
                "Tool Delay",
                0.15f,
                "Changes the default value for tool delay. Official value is 0.5");
        }
    }
}
