using Calculator.Extensions;
using Calculator.Databases.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Calculator.Databases.Converter
{
    public class DatabaseConverter
    {
        public static void WriteJson(Database database, string path)
        {
            if (path == null) throw new Exception("Path file error");
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (Directory.Exists(directory))
            {
                SerializeItem(database, path);
            }
            else
            {
                System.Console.WriteLine($"Unable to write file: {path}");
            }
        }
        public static Database ReadJson(string path)
        {
            if (path == null) throw new Exception("Path file error");
            Database database = DeserializeItem(path);
            database.Prepare();
            return database;
        }

        internal static void SerializeItem(Database database, string fileName)
        {
            JsonDatabase jsonDatabase = JsonConverter.Format(database);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string jsonString = JsonSerializer.Serialize(jsonDatabase, options);
            File.WriteAllText(fileName, jsonString);
        }

        internal static Database DeserializeItem(string fileName)
        {
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                JsonDatabase jsonDatabase = JsonSerializer.Deserialize<JsonDatabase>(jsonString)!;
                return JsonConverter.Parse(jsonDatabase);
            }
            return null;
        }

        
    }
}
