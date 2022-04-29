using Calculator.Databases.Models;
using Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Databases.Converter
{
    class JsonConverter
    {
        #region ======== Format ========
        public static JsonDatabase Format(Database database)
        {
            JsonDatabase jsonDatabase = new JsonDatabase();
            foreach (Item item in database.Items)
            {
                var jsonItem = FormatItem(item);
                jsonDatabase.Items.Add(jsonItem);
            }
            foreach (Factory factory in database.Factories)
            {
                if (factory is Extractor extractor)
                {
                    var jsonExtractor = FormatExtractor(extractor);
                    jsonDatabase.Factories.Add(jsonExtractor);
                }
                else if (factory is Generator generator)
                {
                    var jsonGenerator = FormatGenerator(generator);
                    jsonDatabase.Factories.Add(jsonGenerator);
                }
                else
                {
                    var jsonFactory = FormatFactory(factory);
                    jsonDatabase.Factories.Add(jsonFactory);
                }
            }
            foreach (Recipe recipe in database.Recipes)
            {
                var jsonRecipe = FormatRecipe(recipe);
                jsonDatabase.Recipes.Add(jsonRecipe);
            }
            return jsonDatabase;
        }
        internal static JsonItem FormatItem(Item item)
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
            return jsonItem;
        }
        internal static JsonAmount FormatAmount(Amount amount)
        {
            JsonAmount jsonAmount = new JsonAmount()
            {
                Name = amount.Item.Name,
                Type = amount.Item.ItemType,
                Count = amount.Count
            };
            return jsonAmount;
        }
        internal static JsonRecipe FormatRecipe(Recipe recipe)
        {
            List<JsonAmount> jsonProducts = new List<JsonAmount>();
            foreach (Amount amount in recipe.Products)
            {
                var jsonAmount = FormatAmount(amount);
                jsonProducts.Add(jsonAmount);
            }
            List<JsonAmount> jsonIngredients = new List<JsonAmount>();
            foreach (Amount amount in recipe.Ingredients)
            {
                var jsonAmount = FormatAmount(amount);
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
            return jsonRecipe;
        }
        internal static JsonFactory FormatExtractor(Extractor extractor)
        {
            JsonFactory jsonExtractor = new JsonFactory()
            {
                Name = extractor.Name,
                Description = extractor.Description,
                Type = extractor.ItemType,
                DisplayName = extractor.DisplayName,
                Icon = extractor.IconPath,
                Speed = extractor.Speed,
                PowerConsumption = extractor.PowerConsumption,
                PowerConsumptionExponent = extractor.PowerConsumptionExponent,
                AllowedResourceForms = extractor.AllowedResourceForms,
                AllowedResources = extractor.AllowedResources
            };
            return jsonExtractor;
        }
        internal static JsonFactory FormatGenerator(Generator generator)
        {
            JsonFactory jsonGenerator = new JsonFactory()
            {
                Name = generator.Name,
                Description = generator.Description,
                Type = generator.ItemType,
                DisplayName = generator.DisplayName,
                Icon = generator.IconPath,
                Speed = generator.Speed,
                PowerConsumption = generator.PowerConsumption,
                PowerConsumptionExponent = generator.PowerConsumptionExponent,
                PowerProduction = generator.PowerProduction,
                PowerProductionExponent = generator.PowerProductionExponent
            };
            return jsonGenerator;
        }
        internal static JsonFactory FormatFactory(Factory factory)
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
            return jsonFactory;
        }
        #endregion

        #region ======== Parse ========
        internal static Database Parse(JsonDatabase jsonDatabase)
        {
            Database database = new Database();
            if (jsonDatabase.Items != null)
            {
                foreach (JsonItem jsonItem in jsonDatabase.Items)
                {
                    var item = ParseItem(jsonItem, database);
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
                            var extractor = ParseExtractor(jsonFactory, database);
                            database.Factories.Add(extractor);
                            break;
                        case "Generator":
                            var generator = ParseGenerator(jsonFactory, database);
                            database.Factories.Add(generator);
                            break;
                        default:
                            var factory = ParseFactory(jsonFactory, database);
                            database.Factories.Add(factory);
                            break;
                    }
                }
            }
            if (jsonDatabase.Recipes != null)
            {
                foreach (JsonRecipe jsonRecipe in jsonDatabase.Recipes)
                {
                    var recipe = ParseRecipe(jsonRecipe, database);
                    database.Recipes.Add(recipe);
                }
            }
            return database;
        }
        internal static Item ParseItem(JsonItem jsonItem, Database database)
        {
            Item item = new Item()
            {
                Database = database,
                Name = jsonItem.Name,
                Description = jsonItem.Description,
                ItemType = jsonItem.Type,
                Form = jsonItem.Form,
                StackSize = jsonItem.StackSize,
                DisplayName = jsonItem.DisplayName,
                EnergyValue = jsonItem.EnergyValue,
                IconPath = jsonItem.Icon
            };
            return item;
        }
        internal static Factory ParseFactory(JsonFactory jsonFactory, Database database)
        {
            Factory factory = new Factory()
            {
                Database = database,
                Name = jsonFactory.Name,
                Description = jsonFactory.Description,
                ItemType = jsonFactory.Type,
                DisplayName = jsonFactory.DisplayName,
                IconPath = jsonFactory.Icon,
                Speed = jsonFactory.Speed,
                PowerConsumption = jsonFactory.PowerConsumption,
                PowerConsumptionExponent = jsonFactory.PowerConsumptionExponent
            };
            return factory;
        }
        internal static Generator ParseGenerator(JsonFactory jsonFactory, Database database)
        {
            Generator generator = new Generator()
            {
                Database = database,
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
            return generator;
        }
        internal static Extractor ParseExtractor(JsonFactory jsonFactory, Database database)
        {
            Extractor extractor = new Extractor()
            {
                Database = database,
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
            return extractor;
        }
        internal static Amount ParseAmount(JsonAmount jsonAmount, Database database)
        {
            var item = database.SelectItem(jsonAmount.Name, jsonAmount.Type);
            var count = jsonAmount.Count;
            Amount amount = new Amount(item, count);
            return amount;
        }
        internal static Recipe ParseRecipe(JsonRecipe jsonRecipe, Database database)
        {
            List<Amount> products = new List<Amount>();
            foreach (JsonAmount jsonAmount in jsonRecipe.Products)
            {
                var amount = ParseAmount(jsonAmount, database);
                products.Add(amount);
            }
            List<Amount> ingredients = new List<Amount>();
            foreach (JsonAmount jsonAmount in jsonRecipe.Ingredients)
            {
                var amount = ParseAmount(jsonAmount, database);
                ingredients.Add(amount);
            }
            Recipe recipe = new Recipe()
            {
                Database = database,
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
            return recipe;
        }
        #endregion
    }
}
