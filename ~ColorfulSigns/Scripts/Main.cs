using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Jotunn.Utils;

namespace ColorfulSigns
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]
        public const string NAME = "ColorfulSigns";
        public const string AUTHOR = "Tekla";
        public const string GUID = AUTHOR + "_" + NAME;
        public const string VERSION = "5.4.1603";

        internal static Harmony harmony { get; private set; }
        internal static string folder { get; private set; }
        internal static Assembly assembly { get; private set; }
        internal static ManualLogSource Log { get; private set; }
        #endregion

        public Main()
        {
            Log = new ManualLogSource(NAME);
            harmony = new Harmony(GUID);
            assembly = Assembly.GetExecutingAssembly();
            folder = Path.GetDirectoryName(assembly.Location);
        }

        public void Awake()
        {
            Configs.Awake(this);

            if (Configs.LoggerEnable.Value)
                BepInEx.Logging.Logger.Sources.Add(Log);

            if (!Configs.Enable.Value)
                return;

            ColorfulSigns.UpdateColorLib();
            harmony.PatchAll(assembly);
        }
    }
}
