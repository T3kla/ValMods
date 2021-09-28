using System.Collections.Generic;
using BepInEx.Configuration;

namespace ColorfulSigns
{
    public static class Globals
    {
        public static Dictionary<string, string> colorLibrary;

        public static ConfigEntry<string> configDefColor;
        public static ConfigEntry<bool> configEnableColorLibrary;
        public static ConfigEntry<int> configMaxFontSize;
    }
}