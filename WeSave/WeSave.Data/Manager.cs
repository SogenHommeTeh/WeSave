using System;
using System.IO;
using Newtonsoft.Json;

namespace WeSave.Data
{
    public class Manager
    {
        public static TType DeserializeFile<TType>(string filePath = "./data.json")
        {
            try
            {
                using (var file = File.OpenText(filePath))
                {
                    var serializer = new JsonSerializer();
                    var data = (TType)serializer.Deserialize(file, typeof(TType));
                    return data;
                }
            }
            catch (Exception e)
            {
                return default(TType);
            }
        }

        public static void SerializeFile(object data, string filePath = "./output.json")
        {
            using (var file = File.CreateText(filePath))
            {
                var serializer = new JsonSerializer
                {
                    Formatting = Formatting.Indented
                };
                serializer.Serialize(file, data);
            }
        }
    }
}
