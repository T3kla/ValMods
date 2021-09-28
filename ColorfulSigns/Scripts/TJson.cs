using System;
using System.IO;
using Newtonsoft.Json;

namespace ColorfulSigns.TJson
{
    public sealed class Serialization
    {
        /// <summary> Serialize any object into a text file. Path should contain the separator but not the file name. </summary>
        /// <param name="fileName"> File name should contain the file extension. </param>
        /// <param name="path"> Path should contain separator at the end. </param>
        public static void SerializeFile(object obj, string fileName, string path)
        {
            try
            {
                var str = JsonConvert.SerializeObject(
                    obj,
                    Formatting.Indented,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
                var sr = File.CreateText(path + fileName);
                sr.WriteLine(str);
                sr.Close();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary> Deserialize any object found in the given path into the specified type. </summary>
        public static T DeserializeFile<T>(string path)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(
                    File.ReadAllText(path),
                    new JsonSerializerSettings { Error = (se, ev) => { ev.ErrorContext.Handled = true; } });
                return obj;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary> Serialize any object found in the given path into a string. </summary>
        /// <return> If serialization fails, empty string "" will be returned. </return>
        public static string Serialize(object obj)
        {
            try
            {
                var str = JsonConvert.SerializeObject(
                    obj,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, NullValueHandling = NullValueHandling.Ignore });
                return str;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <summary> Deserialize any object into the specified type. </summary>
        /// <return> If deserialization fails, default type will be returned. </return>
        public static T Deserialize<T>(string obj)
        {
            try
            {
                T newObj = JsonConvert.DeserializeObject<T>(obj);
                return newObj;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
