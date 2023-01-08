using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Xml.Linq;
using UniversalHelmod.Extractors.Satisfactory.Models;
using System.Globalization;
using UniversalHelmod.Classes;
using System.Linq;
using UniversalHelmod.Databases.Models;

namespace UniversalHelmod.Extractors.Stationeers.Models
{
    public class RSDatabase
    {
        private static string GetDataFolder()
        {
            string root = Properties.Settings.Default.StationeersFolder;
            return Path.Combine(root, "rocketstation_Data\\StreamingAssets\\Data");
        }
        public static string DataFolder => GetDataFolder();
        public List<RSRecipe> Recipes { get; set; } = new List<RSRecipe>();
        public List<RSItem> Items { get; set; } = new List<RSItem>();
        public List<RSFactory> Factories { get; set; } = new List<RSFactory>();
        public void Load()
        {
            var path = GetDataFolder();
            var files = Directory.GetFiles(path, "*.xml");
            // recipes
            foreach(var file in files)
            {
                var filename = Path.GetFileName(file);
                var doc = XElement.Load(file);
                var root = doc.FirstNode as XElement;
                if (root != null)
                {
                    var name = root.Name.LocalName;
                    if (name.EndsWith("Recipes"))
                    {
                        var factoryName = name.Replace("Recipes", "");
                        if (factoryName == "GasCanister") factoryName = "HydraulicPipeBender";
                        if (factoryName == "Ingot") factoryName = "Furnace";
                        ParseFactory(factoryName);
                        foreach (var element in root.Elements())
                        {
                            var recipeName = element.Name.LocalName;
                            switch (recipeName)
                            {
                                case "RecipeData":
                                    ParseRecipeData(element, factoryName);
                                    break;
                                case "ProcessingData":
                                    ParseProcessingData(element, factoryName);
                                    break;
                            }

                        }
                    }
                }
            }
            // items
            var ingredientList = new HashSet<string>();
            foreach(var recipe in Recipes)
            {
                var item = new RSItem();
                item.Name = recipe.Name;
                item.PrefabName = recipe.PrefabName;
                item.Type= recipe.Type;
                this.Items.Add(item);
                foreach (var amount in recipe.Ingredients)
                {
                    ingredientList.Add(amount.Name);
                }
            }
            foreach(var ingredient in ingredientList)
            {
                Logger.Info($"reagent:{ingredient}");
            }
        }
        private void ParseFactory(string factoryName)
        {
            if (Factories.FirstOrDefault(x => x.Name == factoryName) == null)
            {
                var prefabName = ConvertPrefabNameFactory(factoryName);
                var factory = new RSFactory();
                factory.Name = factoryName;
                factory.PrefabName = prefabName;
                Factories.Add(factory);
            }
        }
        private string ConvertPrefabNameFactory(string factoryName)
        {
            switch (factoryName)
            {
                case "Chemistry":
                    return "ApplianceChemistryStation";
                case "Microwave":
                    return "ApplianceMicrowave";
                case "PackagingMachine":
                    return "AppliancePackagingMachine";
                case "PaintMix":
                    return "AppliancePaintMixer";
                case "ReagentGrinder":
                    return "ApplianceReagentProcessor";
                case "Recycle":
                    return "StructureRecycler";
                default:
                    return $"Structure{factoryName}";
            }
        }
        private void ParseProcessingData(XElement data, string factory)
        {
            var recipe = new RSRecipe();
            var product = new RSAmount();
            var ingredient = new RSAmount();
            recipe.Type = factory;
            recipe.Products.Add(product);
            recipe.Ingredients.Add(ingredient);
            foreach (var element in data.Elements())
            {
                var name = element.Name.LocalName;
                switch (name)
                {
                    case "InputPrefab":
                        ingredient.PrefabName = element.Value;
                        break;
                    case "OutputPrefab":
                        recipe.Name = element.Value;
                        recipe.PrefabName = element.Value;
                        product.PrefabName = element.Value;
                        break;
                    case "Time":
                        recipe.Time = Convert.ToDouble(element.Value, CultureInfo.InvariantCulture);
                        break;
                    case "In":
                        ingredient.Count = Convert.ToDouble(element.Value, CultureInfo.InvariantCulture);
                        break;
                    case "Out":
                        product.Count = Convert.ToDouble(element.Value, CultureInfo.InvariantCulture);
                        break;
                }
            }
            if (recipe.Name != null)
            {
                var previous = this.Recipes.FirstOrDefault(x => x.PrefabName == recipe.PrefabName);
                if (previous == null)
                {
                    recipe.MadeIn.Add(factory);
                    this.Recipes.Add(recipe);
                }
                else
                {
                    previous.MadeIn.Add(factory);
                }
            }
            else
            {
                Logger.Error("No name for the recipe");
            }
        }
        private void ParseRecipeData(XElement data, string factory)
        {
            var recipe = new RSRecipe();
            recipe.Type = factory;
            var product = new RSAmount();
            recipe.Products.Add(product);
            foreach (var element in data.Elements())
            {
                var name = element.Name.LocalName;
                switch(name)
                {
                    case "PrefabName":
                        recipe.Name = element.Value;
                        recipe.PrefabName = element.Value;
                        product.PrefabName = element.Value;
                        product.Count = 1;
                        break;
                    case "Recipe":
                        ParseRecipe(recipe, element, factory);
                        break;
                    case "RecipeTier":
                        if (element.Value == "TierTwo") recipe.Tier = 2;
                        break;
                }
            }
            if (recipe.Name != null)
            {
                var previous = this.Recipes.FirstOrDefault(x => x.PrefabName == recipe.PrefabName);
                if (previous == null)
                {
                    recipe.MadeIn.Add(factory);
                    this.Recipes.Add(recipe);
                }
                else
                {
                    previous.MadeIn.Add(factory);
                }
            }
            else
            {
                Logger.Error("No name for the recipe");
            }
        }
        
        private void ParseRecipe(RSRecipe recipe, XElement data, string factory)
        {
            foreach (var element in data.Elements())
            {
                var name = element.Name.LocalName;
                switch (name)
                {
                    case "Time":
                        recipe.Time = Convert.ToDouble(element.Value, CultureInfo.InvariantCulture);
                        break;
                    case "Energy":
                        recipe.Energy = Convert.ToInt32(element.Value);
                        break;
                    case "RecipeType":
                        recipe.Type = element.Value;
                        break;
                    case "Temperature":
                    case "Pressure":
                        //TODO
                        break;
                    default:
                        var value = element.Value;
                        var count = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                        if (count > 0)
                        {
                            var amount = new RSAmount();
                            amount.Count = count;
                            amount.Name = name;
                            amount.PrefabName = ConvertPrefabName(factory, amount.Name);
                            recipe.Ingredients.Add(amount);
                        }
                        break;
                }
            }
        }
        private string ConvertPrefabName(string factory, string name)
        {
            switch (factory)
            {
                case "ArcFurnace":
                case "Furnace":
                case "AdvancedFurnace":
                case "Centrifuge":
                    return ConvertPrefabNameFurnace(name);
                case "Microwave":
                case "AutomatedOven":
                case "PaintMix":
                case "PackagingMachine":
                case "Chemistry":
                    return ConvertPrefabNameChemistry(name);
                default:
                    return ConvertPrefabNameFabricator(name);
            }
        }
        private string ConvertPrefabNameFurnace(string name)
        {
            switch (name)
            {
                case "ItemBiomass":
                case "Biomass":
                    return "ItemOrganicMaterial";
                case "Steel":
                    return "ItemSteelIngot";
                case "Carbon":
                case "Hydrocarbon":
                    return "ItemCoalOre";
                default:
                    return $"Item{name}Ore";
            }
        }
        private string ConvertPrefabNameChemistry(string name)
        {
            switch (name)
            {
                case "Carbon":
                    return "ItemCoalOre";
                case "Biomass":
                    return "ItemOrganicMaterial";
                case "Steel":
                    return "ItemSteelIngot";
                case "Soy":
                    return $"ItemSoybean";
                case "Oil":
                    return "ItemSoyOil";
                case "Cobalt":
                case "Silver":
                case "Gold":
                    return $"Item{name}Ore";
                case "Fenoxitone":
                    return "ItemFern";
                case "ColorBlue":
                case "ColorRed":
                case "ColorYellow":
                case "ColorGreen":
                case "ColorOrange":
                    return $"Reagent{name}";
                default:
                    return $"Item{name}";
            }
        }
        private string ConvertPrefabNameFabricator(string name)
        {
            switch (name)
            {
                case "Biomass":
                    return "ItemOrganicMaterial";
                default:
                    return $"Item{name}Ingot";
            }
        }
    }
}
