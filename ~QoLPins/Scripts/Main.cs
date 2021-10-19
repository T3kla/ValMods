using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Jotunn.Utils;

namespace QoLPins
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(Jotunn.Main.ModGuid, BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.NotEnforced, VersionStrictness.None)]
    public class Main : BaseUnityPlugin
    {
        #region Declarations
        public const string NAME = "QoLPins";
        public const string AUTHOR = "Tekla";
        public const string GUID = AUTHOR + "_" + NAME;
        public const string VERSION = "5.4.1604";
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

            if (Configs.LoggerEnable.Value)
                BepInEx.Logging.Logger.Sources.Add(Log);

            if (Configs.Enable.Value)
            {
                Harmony.PatchAll(Assembly);
                PinColor.UpdateColorLib();
                PinAuto.UpdateAutoPinData();
            }
        }
    }
}
