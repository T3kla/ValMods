using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace RemoveDeathPins
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]
        public const string
            MODNAME = "RemoveDeathPins",
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
            Globals.configAlwaysGen = Config.Bind(
                "General",
                "Always generate Death Pin",
                false,
                "Makes it so Death Pin will be generated even if inventory was empty and Tombstone was not generated.");
        }
    }
}
