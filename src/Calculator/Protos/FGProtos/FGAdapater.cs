using Calculator.Enums;
using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.FGProtos
{
    public class FGAdapater
    {
        private Database Instance;
        private FGDatabase Source => FGDatabase.Intance;
        
        private FGAdapater(Database instance)
        {
            this.Instance = instance;
        }

        public static void PopulateDatabase(Database instance)
        {
            var adaptater = new FGAdapater(instance);
            adaptater.Execute();
        }

        private void Execute()
        {
            Instance.Items = new List<Item>();
            foreach (FGItem proto in Source.Items)
            {
                var item = AdaptaterItem(proto);
                Instance.Items.Add(item);
            }

            Instance.Factories = new List<Factory>();
            foreach (FGFactory proto in Source.Factories)
            {
                var item = AdaptaterFactory(proto);
                Instance.Factories.Add(item);
            }

            Instance.Recipes = new List<Recipe>();
            foreach (FGRecipe proto in Source.Recipes)
            {
                var recipe = AdaptaterRecipe(proto);
                recipe.ComputeFlow(energy:recipe.Energy);
                Instance.Recipes.Add(recipe);
            }
        }

        private Item AdaptaterItem(FGItem proto)
        {
            var icon = GetIcon(proto.PersistentBigIcon);
            if (icon == null)
            {
                icon = GetIcon(proto.SmallIcon);
            }
            Item item = new Item()
            {
                Name = proto.ClassName,
                DisplayName = proto.DisplayName,
                Description = proto.Description,
                StackSize = proto.StackSize,
                EnergyValue = proto.EnergyValue,
                RadioactiveDecay = proto.RadioactiveDecay,
                Form = ConvertForm(proto.Form),
                ResourceSinkPoints = proto.ResourceSinkPoints,
                IconPath = icon,
                ItemType = proto.DisplayName.Contains("FICSMAS") || proto.DisplayName.Contains("Candy") || proto.DisplayName.Contains("Snow")
                ? "FICSMAS" : proto.ItemType
            };
            return item;
        }
        private string ConvertForm(object form)
        {
            switch (form.ToString())
            {
                case "RF_LIQUID":
                    return "Liquid";
                default:
                    return "Solid";
            }
        }
        private Factory AdaptaterFactory(FGFactory proto)
        {
            var icon = GetIcon(proto.PersistentBigIcon);
            if (icon == null)
            {
                icon = GetIcon(proto.SmallIcon);
            }
            if(proto is FGExtractor)
            {
                var extractor = proto as FGExtractor;
                var allowedResourceForms = new List<string>();
                if (!String.IsNullOrEmpty(extractor.AllowedResourceForms.ToString()))
                {
                    var resourceForms = extractor.AllowedResourceForms as List<object>;
                    if (resourceForms != null)
                    {
                        foreach(object resourceForm in resourceForms)
                        {
                            allowedResourceForms.Add(ConvertForm(resourceForm));
                        }
                    }
                }
                var allowedResources = new List<string>();
                if (extractor.AllowedResources != null)
                {
                    var resources = extractor.AllowedResources as List<object>;
                    if (resources != null)
                    {
                        foreach (object resource in resources)
                        {
                            var name = resource as string;
                            int index = name.LastIndexOf('.');
                            if (index > 0 && (index + 1) < name.Length)
                            {
                                name = name.Substring(index + 1);
                            }
                            name = name.Replace("\"'", "");
                            allowedResources.Add(name);
                        }
                    }
                }
                // TODO cycle 0.5s? d'ou le facteur 2
                var speed = 2 * extractor.ItemsPerCycle / extractor.ExtractCycleTime;
                if (Double.IsNaN(speed)) speed = 1;
                if (speed > 1000) speed /= 1000;
                Extractor item = new Extractor()
                {
                    Name = proto.ClassName,
                    DisplayName = proto.DisplayName,
                    Description = proto.Description,
                    IconPath = icon,
                    ItemType = proto.ItemType,
                    Speed = speed,
                    PowerConsumption = proto.PowerConsumption,
                    PowerConsumptionExponent = proto.PowerConsumptionExponent,
                    AllowedResourceForms = allowedResourceForms,
                    AllowedResources = allowedResources
                };
                return item;
            }
            else if (proto is FGGenerator)
            {
                var generator = proto as FGGenerator;
                Generator item = new Generator()
                {
                    Name = proto.ClassName,
                    DisplayName = proto.DisplayName,
                    Description = proto.Description,
                    IconPath = icon,
                    ItemType = proto.ItemType,
                    Speed = proto.Speed,
                    PowerConsumption = proto.PowerConsumption,
                    PowerConsumptionExponent = proto.PowerConsumptionExponent,
                    PowerProduction = generator.PowerProduction,
                    PowerProductionExponent = generator.PowerProductionExponent
                };
                return item;
            }
            else
            {
                Factory item = new Factory()
                {
                    Name = proto.ClassName,
                    DisplayName = proto.DisplayName,
                    Description = proto.Description,
                    IconPath = icon,
                    ItemType = proto.ItemType,
                    Speed = proto.Speed,
                    PowerConsumption = proto.PowerConsumption,
                    PowerConsumptionExponent = proto.PowerConsumptionExponent
                };
                return item;
            }
            
        }
        private Recipe AdaptaterRecipe(FGRecipe proto)
        {
            var ingredients = AdaptaterItems(proto.Ingredients);
            var products = AdaptaterItems(proto.Product);
            var masterProduct = products.FirstOrDefault();
            bool alternate = proto.DisplayName.StartsWith("Alternate");
            var iconPath = products.First().Item.IconPath;
            var icon = products.First().Item.Icon;
            var producedIns = proto.ProducedIn as List<object>;
            var madeIn = new List<string>();
            if (producedIns != null)
            {
                foreach (object producedIn in producedIns)
                {
                    var name = producedIn as string;
                    int index = name.LastIndexOf('.');
                    if (index > 0 && (index + 1) < name.Length)
                    {
                        name = name.Substring(index + 1);
                    }
                    if (name == "Build_Converter_C")
                    {
                        var item = masterProduct.Item;
                        var miners = Instance.Factories.OfType<Extractor>();
                        var factories = miners.Where(delegate(Extractor miner) {
                            return miner.AllowedResourceForms.Contains(item.Form) && (miner.AllowedResources.Count == 0 || miner.AllowedResources.Contains(item.Name));
                            }).ToList();
                        foreach(var factory in factories)
                        {
                            madeIn.Add(factory.Name);
                        }
                    }
                    else
                    {
                        madeIn.Add(name);
                    }
                }
            }
            Recipe recipe = new Recipe()
            {
                Name = proto.ClassName,
                DisplayName = proto.DisplayName,
                Energy = proto.ManufactoringDuration,
                MadeIn = madeIn,
                Type = typeof(Recipe).Name,
                Ingredients = ingredients,
                Products = products,
                IconPath = iconPath,
                Icon = icon,
                Alternate = alternate
            };
            return recipe;
        }

        private ObservableCollection<Amount> AdaptaterItems(object elements)
        {
            var array = elements as List<object>;

            ObservableCollection<Amount> items = new ObservableCollection<Amount>();
            foreach (object element in array)
            {
                var dico = element as Dictionary<string, object>;
                var itemClass = dico["ItemClass"].ToString();
                var amount = dico["Amount"].ToString();

                string itemName = ParseClassname(itemClass);
                int count = ParseAmount(amount);

                Item item = Instance.Items.FirstOrDefault(x => x.Name == itemName);
                double itemCount;
                if (item.Form == ConvertForm("RF_LIQUID"))
                {
                    itemCount = count / 1000;
                }
                else
                {
                    itemCount = count;
                }
                if (itemCount == 0) itemCount = 1;
                items.Add(new Amount(item, itemCount));
            }
            return items;
        }

        private string ParseClassname(string value)
        {
            string pattern = "\\.([^.\")]*)\"";
            Match match = Regex.Match(value, pattern);
            if (match.Success)
            {
                string name = match.Groups[1].Value;
                return name;
            }
            return null;
        }
        private int ParseAmount(string value)
        {
            try
            {
                int number;
                Int32.TryParse(value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out number);
                return number;
            }
            catch
            {
                return 0;
            }
        }
        private string GetIcon(string icon)
        {
            string pattern = "/([^ ]*)\\.[^.]*$";
            Match match = Regex.Match(icon, pattern);
            string image = null;
            if (match.Success)
            {
                string name = match.Groups[1].Value;
                name = name.Replace("512", "256");
                image = GetIconPath($"{name}.png");
            }
            if (image == null) return null;
            string dirApp = Directory.GetCurrentDirectory();
            string fileName = Path.Combine(dirApp, image);
            if (File.Exists(fileName))
                return image;
            return null;
        }
        private string GetIconPath(string name)
        {
            name = name.Replace('/', '\\');
            string fileName = Path.Combine("Images", name);
            return fileName;
        }
    }
}
