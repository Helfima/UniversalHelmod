using UniversalHelmod.Databases.Models;
using UniversalHelmod.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UniversalHelmod.Databases.Converter
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
                var jsonFactory = FormatFactory(factory);
                jsonDatabase.Factories.Add(jsonFactory);
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
                DisplayName = item.DisplayName,
                Description = item.Description,
                Type = item.Type,
                Icon = item.IconPath,
                Form = item.Form,
                StackSize = item.StackSize,
                EnergyValue = item.EnergyValue,
                Properties = item.Properties.ToList()
            };
            return jsonItem;
        }
        internal static JsonFactory FormatFactory(Factory factory)
        {
            JsonFactory jsonFactory = new JsonFactory()
            {
                Name = factory.Name,
                DisplayName = factory.DisplayName,
                Description = factory.Description,
                Type = factory.Type,
                Icon = factory.IconPath,
                Speed = factory.Speed,
                PowerConsumption = factory.PowerConsumption,
                PowerProduction = factory.PowerProduction,
                AllowedResourceForms = factory.AllowedResourceForms,
                AllowedResources = factory.AllowedResources,
                Properties = factory.Properties.ToList()
            };
            return jsonFactory;
        }
        internal static JsonAmount FormatAmount(Amount amount)
        {
            JsonAmount jsonAmount = new JsonAmount()
            {
                Name = amount.Item.Name,
                Type = amount.Item.Type,
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
                Tier = recipe.Tier,
                MadeIn = recipe.MadeIn,
                Products = jsonProducts,
                Ingredients = jsonIngredients
            };
            return jsonRecipe;
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
                    var factory = ParseFactory(jsonFactory, database);
                    database.Factories.Add(factory);
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
                DisplayName = jsonItem.DisplayName,
                Description = jsonItem.Description,
                Type = jsonItem.Type,
                IconPath = jsonItem.Icon,
                Form = jsonItem.Form,
                StackSize = jsonItem.StackSize,
                EnergyValue = jsonItem.EnergyValue,
                Properties = jsonItem.Properties.ToObservableCollection()
            };
            return item;
        }
        internal static Factory ParseFactory(JsonFactory jsonFactory, Database database)
        {
            Factory factory = new Factory()
            {
                Database = database,
                Name = jsonFactory.Name,
                DisplayName = jsonFactory.DisplayName,
                Description = jsonFactory.Description,
                Type = jsonFactory.Type,
                IconPath = jsonFactory.Icon,
                Speed = jsonFactory.Speed,
                PowerConsumption = jsonFactory.PowerConsumption,
                PowerProduction = jsonFactory.PowerProduction,
                AllowedResourceForms = jsonFactory.AllowedResourceForms,
                AllowedResources = jsonFactory.AllowedResources,
                Properties = jsonFactory.Properties.ToObservableCollection()
            };
            return factory;
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
                Tier = jsonRecipe.Tier,
                MadeIn = jsonRecipe.MadeIn,
                Products = products.ToObservableCollection(),
                Ingredients = ingredients.ToObservableCollection()
            };
            return recipe;
        }
        #endregion
    }
}
