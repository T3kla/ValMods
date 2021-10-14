using System.Collections.Generic;
using System.IO;
using ColorfulSigns.TJson;

namespace ColorfulSigns
{
    public static class ColorfulSigns
    {
        public static Dictionary<string, string> Library = new();

        public static void UpdateColorLib()
        {
            if (!Configs.UseLibrary.Value)
                return;

            var filePath = Path.Combine(Path.GetDirectoryName(Main.assembly.Location), "color_library.json");
            Main.Log.LogInfo($"Searching for colors in path: '{filePath}'");

            if (!File.Exists(filePath))
            {
                using var sw = File.CreateText(filePath);
                sw.Write("{\"red\":\"#bf3b3b\",\"green\":\"#5fb350\",\"blue\":\"#3f4fe0\",\"white\":\"#ffffff\",\"grey\":\"#919191\",\"black\":\"#000000\",\"text\":\"#ededed\"}");
            }

            ColorfulSigns.Library = Serialization.DeserializeFile<Dictionary<string, string>>(filePath);
            Main.Log.LogInfo($"Loaded {Library.Count} colors!\n");
        }
    }
}
