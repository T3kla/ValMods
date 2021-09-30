using System;
using System.IO;
using Newtonsoft.Json;

namespace ColorfulSigns.TJson
{
    public sealed class Serialization
    {
        public static T Deserialize<T>(string str)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(str);
            }
            catch (Exception) { throw; }
        }

        public static string Serialize(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(
                    obj,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
            }
            catch (Exception) { throw; }
        }

        public static T DeserializeFile<T>(string path)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(
                    File.ReadAllText(path),
                    new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } });
            }
            catch (Exception) { throw; }
        }

        public static void SerializeFile(object obj, string fileName, string path)
        {
            try
            {
                var str = JsonConvert.SerializeObject(
                    obj,
                    Formatting.Indented,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });

                using var sr = File.CreateText(path + fileName);
                sr.Write(str);
            }
            catch (Exception) { throw; }
        }
    }
}
