using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace Areas
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "Monster_Level",
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

            Global.Path.Assembly = Path.GetDirectoryName(assembly.Location);
            AreaHandler.Initialize();

            harmony.PatchAll(assembly);
        }

    }
}
