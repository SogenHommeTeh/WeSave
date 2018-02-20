using System;
using System.IO;
using Newtonsoft.Json;

namespace WeSave.Data
{
    public class Manager
    {
        public static void Level<TData, TOutput>() where TOutput : AOutput<TData>, new()
        {
            var data = DeserializeFile<TData>();
            if (data == null)
            {
                Console.WriteLine("Une erreur est survenue lors de la récupération des données, veuillez vérifier que le fichier 'data.json' est présent et correct.");
                return;
            }
            try
            {
                var output = new TOutput().FromData(data);
                var serialized = SerializeFile(output);
                if (serialized == false)
                {
                    Console.WriteLine("Une erreur est survenue lors de la création des données, un fichier 'output.json' doit pouvoir être créé.");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Une erreur est survenue lors du traitement des données, veuillez vérifier que le fichier 'data.json' contient des données valides.");
            }
        }

        private static TType DeserializeFile<TType>(string filePath = "./data.json")
        {
            try
            {
                TType data;
                using (var file = File.OpenText(filePath))
                {
                    var serializer = new JsonSerializer();
                    data = (TType)serializer.Deserialize(file, typeof(TType));
                }
                return data;
            }
            catch (Exception)
            {
                return default(TType);
            }
        }

        private static bool SerializeFile(object data, string filePath = "./output.json")
        {
            try
            {
                using (var file = File.CreateText(filePath))
                {
                    var serializer = new JsonSerializer
                    {
                        Formatting = Formatting.Indented
                    };
                    serializer.Serialize(file, data);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
