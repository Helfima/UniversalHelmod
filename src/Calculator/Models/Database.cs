using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Linq;
using System.Windows.Media.Imaging;
using Calculator.Protos;
using System.Collections.ObjectModel;
using Calculator.Protos.Kirk;
using Calculator.Protos.FGProtos;
using Calculator.Enums;

namespace Calculator.Models
{
    public class Database
    {
        public List<Item> Items { get; set; }

        public List<Recipe> Recipes { get; set; }

        private Dictionary<string, List<Recipe>> recipesByProduct = new Dictionary<string, List<Recipe>>();
        private Dictionary<string, List<Recipe>> recipesByIngredient = new Dictionary<string, List<Recipe>>();
        public List<Factory> Factories { get; set; }

        public List<Logistic> Logistics { get; set; }

        private Dictionary<Item, ItemCost> itemCosts = new Dictionary<Item, ItemCost>();

        public static Database Intance => GetInstance();

        private static Database instance;

        public Item SelectItem(string name, string type)
        {
            ItemType itemType = ItemType.Item;
            Enum.TryParse<ItemType>(type, out itemType);
            return Items.FirstOrDefault(x => x.ItemType == itemType && x.Name == name);
        }
        public Recipe SelectRecipe(string name)
        {
            return Recipes.FirstOrDefault(x => x.Name == name);
        }
        public Factory SelectFactory(string name)
        {
            return Factories.FirstOrDefault(x => x.Name == name);
        }
        public List<Recipe> SelectRecipeByProduct(Item item)
        {
            if (!recipesByProduct.ContainsKey(item.Name)) return null;
            List<Recipe> recipes = recipesByProduct[item.Name];
            return recipes;
        }
        public List<Recipe> SelectRecipeByIngredient(Item item)
        {
            if (!recipesByIngredient.ContainsKey(item.Name)) return null;
            List<Recipe> recipes = recipesByIngredient[item.Name];
            return recipes;
        }

        #region ==== Initialize ====
        private static Database GetInstance()
        {
            if (instance == null)
            {
                instance = new Database();
                instance.Load();
            }
            return instance;
        }
        public void Load()
        {
            FGAdapater.PopulateDatabase(instance);
            foreach (Recipe recipe in Recipes)
            {
                AddRecipeByProduct(recipe);
                WhereUse(recipe);
            }
            ComputeCost();
            Recipes.Sort((x, y) => x.MainProduct.Name.CompareTo(y.MainProduct.Name));
        }
        private void ComputeCost()
        {
            foreach(Item item in Items)
            {
                ComputeCost(item);
            }
            foreach (Recipe recipe in Recipes)
            {
                ComputeCost(recipe);
            }
        }
        
        private ItemCost ComputeCost(Item item)
        {
            var itemCost = new ItemCost(item);
            if (itemCosts.ContainsKey(item)) return itemCosts[item];
            if (item.ItemType == Enums.ItemType.Resource || item.Form == Enums.ItemForm.Liquid)
            {
                itemCost.Add(item, 1);
            }
            else
            {
                Recipe recipe = SelectRecipeByProduct(item)?.FirstOrDefault(x => !x.Alternate);
                if (recipe != null)
                {
                    var product = recipe.Products.FirstOrDefault();
                    foreach (Amount ingredient in recipe.Ingredients)
                    {
                        if (item != ingredient.Item)
                        {
                            var previousCost = ComputeCost(ingredient.Item);
                            itemCost.Add(previousCost, ingredient.Count / product.Count);
                        }
                    }
                }
                else
                {
                    itemCost.Add(item, 1);
                }
            }
            itemCosts.Add(item, itemCost);
            item.ItemCost = itemCost;
            return itemCost;
        }

        private void ComputeCost(Recipe recipe)
        {
            var recipeCost = new RecipeCost(recipe);
            var product = recipe.Products.FirstOrDefault();
            foreach (Amount ingredient in recipe.Ingredients)
            {
                recipeCost.Add(ingredient.Item.ItemCost, ingredient.Count / product.Count);
            }
            recipe.RecipeCost = recipeCost;
        }
        private void WhereUse(Recipe recipe)
        {
            foreach (Amount ingredient in recipe.Ingredients)
            {
                foreach (Amount product in recipe.Products)
                {
                    var whereUsed = ingredient.Item.WhereUsed;
                    if ((product.Item.ItemType == Enums.ItemType.Item
                        || product.Item.ItemType == Enums.ItemType.FICSMAS
                        || product.Item.ItemType == Enums.ItemType.Consumable
                        || product.Item.ItemType == Enums.ItemType.Ammo)
                        && !whereUsed.Contains(product.Item))
                    {
                        whereUsed.Add(product.Item);
                    }
                }
            }
        }

        private void AddRecipeByProduct(Recipe recipe)
        {
            foreach (Amount product in recipe.Products)
            {
                if (!recipesByProduct.ContainsKey(product.Name))
                {
                    recipesByProduct.Add(product.Name, new List<Recipe>());
                }
                List<Recipe> recipes = recipesByProduct[product.Name];
                recipes.Add(recipe);
            }
            foreach (Amount ingredient in recipe.Ingredients)
            {
                if (!recipesByIngredient.ContainsKey(ingredient.Name))
                {
                    recipesByIngredient.Add(ingredient.Name, new List<Recipe>());
                }
                List<Recipe> recipes = recipesByIngredient[ingredient.Name];
                recipes.Add(recipe);
            }
        }

        #endregion

        public void Save()
        {
        }

    }
}
