using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using ColorfulSigns.TJson;
using HarmonyLib;
using UnityEngine;

namespace ColorfulSigns
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Main : BaseUnityPlugin
    {
        #region[Declarations]

        public const string
            MODNAME = "ColorfulSigns",
            AUTHOR = "Tekla",
            GUID = AUTHOR + "_" + MODNAME,
            VERSION = "5.4.1500";

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

            if (Globals.configEnableColorLibrary.Value)
                InitializeColorLibrary();

            harmony.PatchAll(assembly);
        }

        public void InitializeConfig()
        {
            Globals.configDefColor = Config.Bind(
                "Color",
                "Default Color",
                "#ededed",
                "Changes the default color of signs. Use hexadecimals as hash followed by six digits.");

            Globals.configEnableColorLibrary = Config.Bind(
                "Color",
                "Enable Color Library",
                true,
                "Enables or disables color library functionality.");

            Globals.configMaxFontSize = Config.Bind(
                "Font",
                "Max Font Size",
                8,
                "Stablish a max size for sign fonts. ");
        }

        public void InitializeColorLibrary()
        {
            string filePath = $@"{Path.GetDirectoryName(assembly.Location)}\color_library.json";

            if (File.Exists(filePath))
            {
                Globals.colorLibrary = Serialization.DeserializeFile<Dictionary<string, string>>(filePath);
                if (Globals.colorLibrary != null)
                    Debug.Log($"[ColorfulSigns] colorLibrary count: {Globals.colorLibrary.Count}");
                else
                    Debug.Log($"[ColorfulSigns] Couldn't load colorLibrary");
            }
        }
    }
}
