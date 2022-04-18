using Calculator.Extensions;
using Calculator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Calculator.Converter
{
    public class DatabaseConverter
    {
        public static string GetFilename()
        {
            string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(root, "Data/database.json");
        }
        public static void WriteJson(Database database, string path = null)
        {
            if (path == null) path = GetFilename();
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            if (Directory.Exists(directory))
            {
                SerializeItem(path, database);
            }
            else
            {
                System.Console.WriteLine($"Unable to write file: {path}");
            }
        }
        public static Database ReadJson(string path = null)
        {
            if (path == null) path = GetFilename();
            Database database = DeserializeItem(path);
            database.Prepare();
            return database;
        }

        internal static void SerializeItem(string fileName, Database database)
        {
            JsonDatabase jsonDatabase = ToJsonDatabase(database);
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
                return ToDatabase(jsonDatabase);
            }
            return null;
        }

        internal static JsonDatabase ToJsonDatabase(Database database)
        {
            JsonDatabase jsonDatabase = new JsonDatabase();
            foreach(Item item in database.Items)
            {
                JsonItem jsonItem = new JsonItem()
                {
                    Name = item.Name,
                    Description = item.Description,
                    Type = item.ItemType,
                    Form = item.Form,
                    StackSize = item.StackSize,
                    DisplayName = item.DisplayName,
                    EnergyValue = item.EnergyValue,
                    Icon = item.IconPath
                };
                jsonDatabase.Items.Add(jsonItem);
            }
            foreach (Factory factory in database.Factories)
            {
                if(factory is Extractor)
                {
                    Extractor extractor = factory as Extractor;
                    JsonFactory jsonExtractor = new JsonFactory()
                    {
                        Name = factory.Name,
                        Description = factory.Description,
                        Type = factory.ItemType,
                        DisplayName = factory.DisplayName,
                        Icon = factory.IconPath,
                        Speed = factory.Speed,
                        PowerConsumption = factory.PowerConsumption,
                        PowerConsumptionExponent = factory.PowerConsumptionExponent,
                        AllowedResourceForms = extractor.AllowedResourceForms,
                        AllowedResources = extractor.AllowedResources
                    };
                    jsonDatabase.Factories.Add(jsonExtractor);
                }
                else if(factory is Generator)
                {
                    Generator generator = factory as Generator;
                    JsonFactory jsonGenerator = new JsonFactory()
                    {
                        Name = factory.Name,
                        Description = factory.Description,
                        Type = factory.ItemType,
                        DisplayName = factory.DisplayName,
                        Icon = factory.IconPath,
                        Speed = factory.Speed,
                        PowerConsumption = factory.PowerConsumption,
                        PowerConsumptionExponent = factory.PowerConsumptionExponent,
                        PowerProduction = generator.PowerProduction,
                        PowerProductionExponent = generator.PowerProductionExponent
                    };
                    jsonDatabase.Factories.Add(jsonGenerator);
                }
                else
                {
                    JsonFactory jsonFactory = new JsonFactory()
                    {
                        Name = factory.Name,
                        Description = factory.Description,
                        Type = factory.ItemType,
                        DisplayName = factory.DisplayName,
                        Icon = factory.IconPath,
                        Speed = factory.Speed,
                        PowerConsumption = factory.PowerConsumption,
                        PowerConsumptionExponent = factory.PowerConsumptionExponent
                    };
                    jsonDatabase.Factories.Add(jsonFactory);
                }
            }
            foreach (Recipe recipe in database.Recipes)
            {
                List<JsonAmount> jsonProducts = new List<JsonAmount>();
                foreach(Amount amount in recipe.Products)
                {
                    JsonAmount jsonAmount = new JsonAmount()
                    {
                        Name  = amount.Item.Name,
                        Type = amount.Item.ItemType,
                        Count = amount.Count
                    };
                    jsonProducts.Add(jsonAmount);
                }
                List<JsonAmount> jsonIngredients = new List<JsonAmount>();
                foreach (Amount amount in recipe.Ingredients)
                {
                    JsonAmount jsonAmount = new JsonAmount()
                    {
                        Name = amount.Item.Name,
                        Type = amount.Item.ItemType,
                        Count = amount.Count
                    };
                    jsonIngredients.Add(jsonAmount);
                }
                JsonRecipe jsonRecipe = new JsonRecipe()
                {
                    Name = recipe.Name,
                    Type = recipe.Type,
                    DisplayName = recipe.DisplayName,
                    Icon = recipe.IconPath,
                    Energy = recipe.Energy,
                    Alternate = recipe.Alternate,
                    MadeIn = recipe.MadeIn,
                    Products = jsonProducts,
                    Ingredients = jsonIngredients
                };
                jsonDatabase.Recipes.Add(jsonRecipe);
            }
            return jsonDatabase;
        }
        internal static Database ToDatabase(JsonDatabase jsonDatabase)
        {
            Database database = new Database();
            if (jsonDatabase.Items != null)
            {
                foreach (JsonItem jsonItem in jsonDatabase.Items)
                {
                    Item item = new Item()
                    {
                        Name = jsonItem.Name,
                        Description = jsonItem.Description,
                        ItemType = jsonItem.Type,
                        Form = jsonItem.Form,
                        StackSize = jsonItem.StackSize,
                        DisplayName = jsonItem.DisplayName,
                        EnergyValue = jsonItem.EnergyValue,
                        IconPath = jsonItem.Icon
                    };
                    database.Items.Add(item);
                }
            }
            if (jsonDatabase.Factories != null)
            {
                foreach (JsonFactory jsonFactory in jsonDatabase.Factories)
                {
                    switch (jsonFactory.Type)
                    {
                        case "Extractor":
                            Extractor extractor = new Extractor()
                            {
                                Name = jsonFactory.Name,
                                Description = jsonFactory.Description,
                                ItemType = jsonFactory.Type,
                                DisplayName = jsonFactory.DisplayName,
                                IconPath = jsonFactory.Icon,
                                Speed = jsonFactory.Speed,
                                PowerConsumption = jsonFactory.PowerConsumption,
                                PowerConsumptionExponent = jsonFactory.PowerConsumptionExponent,
                                AllowedResourceForms = jsonFactory.AllowedResourceForms,
                                AllowedResources = jsonFactory.AllowedResources
                            };
                            database.Factories.Add(extractor);
                            break;
                        case "Generator":
                            Generator generator = new Generator()
                            {
                                Name = jsonFactory.Name,
                                Description = jsonFactory.Description,
                                ItemType = jsonFactory.Type,
                                DisplayName = jsonFactory.DisplayName,
                                IconPath = jsonFactory.Icon,
                                Speed = jsonFactory.Speed,
                                PowerConsumption = jsonFactory.PowerConsumption,
                                PowerConsumptionExponent = jsonFactory.PowerConsumptionExponent,
                                PowerProduction = jsonFactory.PowerProduction,
                                PowerProductionExponent = jsonFactory.PowerProductionExponent
                            };
                            database.Factories.Add(generator);
                            break;
                        default:
                            Factory factory = new Factory()
                            {
                                Name = jsonFactory.Name,
                                Description = jsonFactory.Description,
                                ItemType = jsonFactory.Type,
                                DisplayName = jsonFactory.DisplayName,
                                IconPath = jsonFactory.Icon,
                                Speed = jsonFactory.Speed,
                                PowerConsumption = jsonFactory.PowerConsumption,
                                PowerConsumptionExponent = jsonFactory.PowerConsumptionExponent
                            };
                            database.Factories.Add(factory);
                            break;
                    }
                }
            }
            if (jsonDatabase.Recipes != null) { 
                foreach (JsonRecipe jsonRecipe in jsonDatabase.Recipes)
                {
                    List<Amount> products = new List<Amount>();
                    foreach (JsonAmount jsonAmount in jsonRecipe.Products)
                    {
                        var item = database.SelectItem(jsonAmount.Name, jsonAmount.Type);
                        var count = jsonAmount.Count;
                        Amount amount = new Amount(item, count);
                        products.Add(amount);
                    }
                    List<Amount> ingredients = new List<Amount>();
                    foreach (JsonAmount jsonAmount in jsonRecipe.Ingredients)
                    {
                        var item = database.SelectItem(jsonAmount.Name, jsonAmount.Type);
                        var count = jsonAmount.Count;
                        Amount amount = new Amount(item, count);
                        ingredients.Add(amount);
                    }
                    Recipe recipe = new Recipe()
                    {
                        Name = jsonRecipe.Name,
                        Type = jsonRecipe.Type,
                        DisplayName = jsonRecipe.DisplayName,
                        IconPath = jsonRecipe.Icon,
                        Energy = jsonRecipe.Energy,
                        Alternate = jsonRecipe.Alternate,
                        MadeIn = jsonRecipe.MadeIn,
                        Products = products.ToObservableCollection(),
                        Ingredients = ingredients.ToObservableCollection()
                    };
                    database.Recipes.Add(recipe);
                }
            }
            return database;
        }
    }
}
