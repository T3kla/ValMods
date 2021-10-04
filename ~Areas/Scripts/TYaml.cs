using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Areas.TYaml
{
    public sealed class Serialization
    {
        public static T Deserialize<T>(string str)
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();

                using var reader = new StringReader(str);
                return deserializer.Deserialize<T>(reader);
            }
            catch (Exception) { throw; }
        }

        public static string Serialize(object obj)
        {
            try
            {
                return new SerializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .Build()
                    .Serialize(obj);
            }
            catch (Exception) { throw; }
        }

        public static T DeserializeFile<T>(string path)
        {
            try
            {
                return new DeserializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build()
                    .Deserialize<T>(File.ReadAllText(path));
            }
            catch (Exception) { throw; }
        }

        public static void SerializeFile(object obj, string fileName, string path)
        {
            try
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(UnderscoredNamingConvention.Instance)
                    .Build();

                using var sw = File.CreateText(path + fileName);
                serializer.Serialize(sw, obj);
            }
            catch (Exception) { throw; }
        }
    }
}
