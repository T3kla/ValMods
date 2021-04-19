using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Areas.TJson
{

    public sealed class Serialization
    {

        /// <summary> Serialize any object into a text file. Path should contain the separator but not the file name. </summary>
        /// <param name="fileName"> File name should contain the file extension. </param>
        /// <param name="path"> Path sould contain separator at the end. </param>
        public static void SerializeFile(object obj, string fileName, string path)
        {
            try
            {
                string str = JsonConvert.SerializeObject(
                    obj,
                    Formatting.Indented,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });

                var sr = File.CreateText(path + fileName);
                sr.WriteLine(str);
                sr.Close();

                Main.GLog.LogInfo($"Serialized object with name \"{fileName}\" into \"{path}\"");
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"Couldn't serialize object \"{fileName}\"\n{e.Message}\n{e.StackTrace}");
            }
        }

        /// <summary> Deserialize any object found in the given path into the specified type. </summary>
        public static T DeserializeFile<T>(string path)
        {
            try
            {
                string str = File.ReadAllText(path);
                T obj = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } });
                Main.GLog.LogInfo($"Deserialized object \"{Path.GetFileName(path)}\" of type \"{typeof(T).Name}\" from \"{path}\"");
                return obj;
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"Couldn't deserialize object \"{Path.GetFileName(path)}\" of type \"{typeof(T).Name}\" from \"{path}\"\n{e.Message}\n{e.StackTrace}");
                return default(T);
            }
        }

        /// <summary> Serialize any object found in the given path into a string. </summary>
        /// <return> If seralization fails, empty string "" will be returned. </return>
        public static string Serialize(object obj)
        {
            try
            {
                string str = JsonConvert.SerializeObject(
                    obj,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });

                Main.GLog.LogInfo($"Serialized object with name \"{obj.ToString()}\"");
                return str;
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"Couldn't serialize object \"{obj.ToString()}\"\n{e.Message}\n{e.StackTrace}");
                return "";
            }
        }

        /// <summary> Deserialize any object into the specified type. </summary>
        /// <return> If deseralization fails, default type will be returned. </return>
        public static T Deserialize<T>(string obj)
        {
            try
            {
                string str = obj;
                T newObj = JsonConvert.DeserializeObject<T>(str);
                Main.GLog.LogInfo($"Deserialized object of type \"{typeof(T).Name}\"");
                return newObj;
            }
            catch (Exception e)
            {
                Main.GLog.LogError($"Couldn't deserialize object of type \"{typeof(T).Name}\"\n{e.Message}\n{e.StackTrace}");
                return default(T);
            }
        }

    }

}
