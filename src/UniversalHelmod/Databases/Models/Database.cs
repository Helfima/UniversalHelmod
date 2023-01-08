using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UniversalHelmod.Databases.Converter;

namespace UniversalHelmod.Databases.Models
{
    public class Database
    {
        private Dictionary<string, List<Recipe>> recipesByProduct = new Dictionary<string, List<Recipe>>();
        private Dictionary<string, List<Recipe>> recipesByIngredient = new Dictionary<string, List<Recipe>>();
        private Dictionary<Item, ItemCost> itemCosts = new Dictionary<Item, ItemCost>();

        public List<Item> Items { get; set; } = new List<Item>();
        public List<string> ItemTypes { get; set; }
        public List<string> ItemForms { get; set; }
        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
        public List<Factory> Factories { get; set; } = new List<Factory>();
        public List<string> FactoryTypes { get; set; }
        public List<Logistic> Logistics { get; set; } = new List<Logistic>();

        public Item SelectItem(string name, string type)
        {
            return Items.FirstOrDefault(x => x.Type == type && x.Name == name);
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
        public void Prepare()
        {
            foreach (Recipe recipe in Recipes)
            {
                AddRecipeByProduct(recipe);
                WhereUse(recipe);
            }
            //ComputeCost();
            ComputeItemTypes();
            ComputeItemForms();
            ComputeFactoryTypes();
            Items.Sort((x, y) => x.Name.CompareTo(y.Name));
            ItemTypes.Sort();
            Factories.Sort((x, y) => x.Name.CompareTo(y.Name));
            FactoryTypes.Sort();
            Recipes.Sort((x, y) => x.MainProduct.Name.CompareTo(y.MainProduct.Name));
        }
        public void RefreshInternalList()
        {
            ComputeItemTypes();
            ComputeItemForms();
            ComputeFactoryTypes();
        }
        private void ComputeItemTypes()
        {
            ItemTypes = new List<string>();
            foreach (Item item in Items)
            {
                if (!ItemTypes.Contains(item.Type)) ItemTypes.Add(item.Type);
            }
        }
        private void ComputeItemForms()
        {
            ItemForms = new List<string>();
            foreach (Item item in Items)
            {
                if (!ItemForms.Contains(item.Form)) ItemForms.Add(item.Form);
            }
        }
        private void ComputeFactoryTypes()
        {
            FactoryTypes = new List<string>();
            foreach (Factory factory in Factories)
            {
                if (!FactoryTypes.Contains(factory.Type)) FactoryTypes.Add(factory.Type);
            }
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
            if (item.Type == "Resource" || item.Form == "Liquid")
            {
                itemCost.Add(item, 1);
            }
            else
            {
                Recipe recipe = SelectRecipeByProduct(item)?.FirstOrDefault(x => x.Tier <= 0);
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
                    if ((product.Item.Type != "Building"
                        && product.Item.Type != "FICSMAS")
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
