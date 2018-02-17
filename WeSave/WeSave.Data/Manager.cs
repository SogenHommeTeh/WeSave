using System;
using System.IO;
using Newtonsoft.Json;

namespace WeSave.Data
{
    public class Manager
    {
        public static void Level1()
        {
            var data = DeserializeFile<DataModel>();
            if (data == null)
            {
                Console.WriteLine("Une erreur est survenue lors de la récupération des données, veuillez vérifier que le fichier 'data.json' est présent et correct.");
                return;
            }
            object output = null;
            try
            {
                output = new Level1.Output()
                {
                    Rentals = Data.Level1.RentalModel.FromData(data),
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors du traitement des données, veuillez vérifier que le fichier 'data.json' contient des données valides.");
                return;
            }
            var serialized = SerializeFile(output);
            if (serialized == false)
            {
                Console.WriteLine("Une erreur est survenue lors de la création des données, un fichier 'output.json' doit pouvoir être créé.");
            }
        }

        public static void Level2()
        {
            var data = DeserializeFile<DataModel>();
            if (data == null)
            {
                Console.WriteLine("Une erreur est survenue lors de la récupération des données, veuillez vérifier que le fichier 'data.json' est présent et correct.");
                return;
            }
            object output = null;
            try
            {
                output = new Level2.Output()
                {
                    Rentals = Data.Level2.RentalModel.FromData(data),
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors du traitement des données, veuillez vérifier que le fichier 'data.json' contient des données valides.");
                return;
            }
            var serialized = SerializeFile(output);
            if (serialized == false)
            {
                Console.WriteLine("Une erreur est survenue lors de la création des données, un fichier 'output.json' doit pouvoir être créé.");
            }
        }

        public static void Level3()
        {
            var data = DeserializeFile<DataModel>();
            if (data == null)
            {
                Console.WriteLine("Une erreur est survenue lors de la récupération des données, veuillez vérifier que le fichier 'data.json' est présent et correct.");
                return;
            }
            object output = null;
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors du traitement des données, veuillez vérifier que le fichier 'data.json' contient des données valides.");
                return;
            }
            var serialized = SerializeFile(output);
            if (serialized == false)
            {
                Console.WriteLine("Une erreur est survenue lors de la création des données, un fichier 'output.json' doit pouvoir être créé.");
            }
        }

        public static void Level4()
        {
            var data = DeserializeFile<DataModel>();
            if (data == null)
            {
                Console.WriteLine("Une erreur est survenue lors de la récupération des données, veuillez vérifier que le fichier 'data.json' est présent et correct.");
                return;
            }
            object output = null;
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors du traitement des données, veuillez vérifier que le fichier 'data.json' contient des données valides.");
                return;
            }
            var serialized = SerializeFile(output);
            if (serialized == false)
            {
                Console.WriteLine("Une erreur est survenue lors de la création des données, un fichier 'output.json' doit pouvoir être créé.");
            }
        }

        public static void Level5()
        {
            var data = DeserializeFile<DataModel>();
            if (data == null)
            {
                Console.WriteLine("Une erreur est survenue lors de la récupération des données, veuillez vérifier que le fichier 'data.json' est présent et correct.");
                return;
            }
            object output = null;
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors du traitement des données, veuillez vérifier que le fichier 'data.json' contient des données valides.");
                return;
            }
            var serialized = SerializeFile(output);
            if (serialized == false)
            {
                Console.WriteLine("Une erreur est survenue lors de la création des données, un fichier 'output.json' doit pouvoir être créé.");
            }
        }

        public static void Level6()
        {
            var data = DeserializeFile<DataModel>();
            if (data == null)
            {
                Console.WriteLine("Une erreur est survenue lors de la récupération des données, veuillez vérifier que le fichier 'data.json' est présent et correct.");
                return;
            }
            object output = null;
            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine("Une erreur est survenue lors du traitement des données, veuillez vérifier que le fichier 'data.json' contient des données valides.");
                return;
            }
            var serialized = SerializeFile(output);
            if (serialized == false)
            {
                Console.WriteLine("Une erreur est survenue lors de la création des données, un fichier 'output.json' doit pouvoir être créé.");
            }
        }

        public static TType DeserializeFile<TType>(string filePath = "./data.json")
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
            catch (Exception e)
            {
                return default(TType);
            }
        }

        public static bool SerializeFile(object data, string filePath = "./output.json")
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
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
