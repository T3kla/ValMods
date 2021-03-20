using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace ColorfulSigns.TJson
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

                Debug.Log($"[ColorfulSigns.TJson] Serialized object with name \"{fileName}\" into \"{path}\"");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ColorfulSigns.TJson] Couldn't serialize object \"{fileName}\"\n{e.Message}\n{e.StackTrace}");
            }
        }

        /// <summary> Deserialize any object found in the given path into the specified type. </summary>
        public static T DeserializeFile<T>(string path)
        {
            try
            {
                string str = File.ReadAllText(path);
                T obj = JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } });
                Debug.Log($"[ColorfulSigns.TJson] Deserialized object \"{Path.GetFileName(path)}\" of type \"{nameof(T)}\" from \"{path}\"");
                return obj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[ColorfulSigns.TJson] Couldn't deserialize object \"{Path.GetFileName(path)}\" of type \"{nameof(T)}\" from \"{path}\"\n{e.Message}\n{e.StackTrace}");
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

                Debug.Log($"[ColorfulSigns.TJson] Serialized object with name \"{obj.ToString()}\"");
                return str;
            }
            catch (Exception e)
            {
                Debug.LogError($"[ColorfulSigns.TJson] Couldn't serialize object \"{obj.ToString()}\"\n{e.Message}\n{e.StackTrace}");
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
                Debug.Log($"[ColorfulSigns.TJson] Deserialized object of type \"{nameof(T)}\"");
                return newObj;
            }
            catch (Exception e)
            {
                Debug.LogError($"[ColorfulSigns.TJson] Couldn't deserialize object of type \"{nameof(T)}\"\n{e.Message}\n{e.StackTrace}");
                return default(T);
            }
        }

    }

}
