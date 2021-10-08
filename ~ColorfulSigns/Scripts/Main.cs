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
        public const string
            NAME = "ColorfulSigns",
            AUTHOR = "Tekla",
            GUID = AUTHOR + "_" + NAME,
            VERSION = "5.4.1602";

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

        public void Awake()
        {
            Configs.Awake(this);

            if (Configs.LoggerEnable.Value)
                BepInEx.Logging.Logger.Sources.Add(Log);

            if (!Configs.Enable.Value)
                return;

            if (Configs.UseLibrary.Value)
                ColorfulSigns.Awake(assembly);

            harmony.PatchAll(assembly);
        }
    }
}
