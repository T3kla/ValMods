using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace AutoRepair
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]
        public const string
            NAME = "AutoRepair",
            AUTHOR = "Tekla",
            GUID = AUTHOR + "_" + NAME,
            VERSION = "5.4.1600";

        internal readonly ManualLogSource log;
        internal readonly Harmony harmony;
        internal readonly Assembly assembly;
        public readonly string modFolder;
        #endregion

        internal static ManualLogSource Log;

        public Main()
        {
            Log = new ManualLogSource(NAME);
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            modFolder = Path.GetDirectoryName(assembly.Location);
        }

        public void Start()
        {
            BepInEx.Logging.Logger.Sources.Add(Log);
            harmony.PatchAll(assembly);
        }
    }
}
