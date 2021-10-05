using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace QoLPins
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region Declarations
        public const string
            NAME = "QoLPins",
            AUTHOR = "Tekla",
            GUID = AUTHOR + "_" + NAME,
            VERSION = "5.4.1600";

        internal static ManualLogSource Log { get; private set; }
        internal readonly Harmony Harmony;
        internal readonly Assembly Assembly;
        public readonly string ModFolder;
        #endregion

        public Main()
        {
            Log = new ManualLogSource(NAME);
            Harmony = new Harmony(GUID);
            Assembly = Assembly.GetExecutingAssembly();
            ModFolder = Path.GetDirectoryName(Assembly.Location);
        }

        public void Awake()
        {
            Configs.Awake(this);

            if (Configs.LoggerEnable.Value)
                BepInEx.Logging.Logger.Sources.Add(Log);

            if (Configs.Enable.Value)
                Harmony.PatchAll(Assembly);
        }
    }
}
