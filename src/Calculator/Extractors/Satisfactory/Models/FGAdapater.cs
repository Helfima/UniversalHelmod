using Calculator.Enums;
using Calculator.Databases.Models;
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
using Calculator.Classes;
using System.Threading.Tasks;

namespace Calculator.Extractors.Satisfactory.Models
{
    public class FGAdapater
    {
        private Database Instance;
        private FGDatabase Source => FGDatabase.Intance;
        
        private FGAdapater(Database instance)
        {
            this.Instance = instance;
        }

        public static async Task<Database> PopulateDatabaseAsync()
        {
            return await Task.Run(() => PopulateDatabase());
        }
        public static Database PopulateDatabase()
        {
            var instance = new Database();
            var adaptater = new FGAdapater(instance);
            adaptater.Execute();
            return instance;
        }

        private void Execute()
        {
            SettingsModel.InvokeMessage(this, "Convert Items...");
            Instance.Items = new List<Item>();
            foreach (FGItem proto in Source.Items)
            {
                var item = AdaptaterItem(proto);
                Instance.Items.Add(item);
            }

            SettingsModel.InvokeMessage(this, "Convert Factories...");
            Instance.Factories = new List<Factory>();
            foreach (FGFactory proto in Source.Factories)
            {
                var item = AdaptaterFactory(proto);
                Instance.Factories.Add(item);
            }

            SettingsModel.InvokeMessage(this, "Convert Recipes...");
            Instance.Recipes = new List<Recipe>();
            foreach (FGRecipe proto in Source.Recipes)
            {
                var recipe = AdaptaterRecipe(proto);
                recipe.ComputeFlow(energy:recipe.Energy);
                Instance.Recipes.Add(recipe);
            }
            SettingsModel.InvokeMessage(this, "Instance Prepare...");
            Instance.Prepare();
            SettingsModel.InvokeMessage(this, "Database ready");
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
                Database = Instance,
                Name = proto.ClassName,
                DisplayName = proto.DisplayName,
                Description = proto.Description,
                StackSize = proto.StackSize,
                EnergyValue = proto.EnergyValue,
                Form = ConvertForm(proto.Form),
                IconPath = icon,
                Type = proto.DisplayName.Contains("FICSMAS") || proto.DisplayName.Contains("Candy") || proto.DisplayName.Contains("Snow")
                ? "FICSMAS" : proto.Type
            };
            item.Properties.Add(new Property("RadioactiveDecay", proto.RadioactiveDecay));
            item.Properties.Add(new Property("ResourceSinkPoints", proto.ResourceSinkPoints));
            return item;
        }
        private string ConvertForm(object form)
        {
            switch (form.ToString())
            {
                case "RF_LIQUID":
                    return "Liquid";
                case "RF_GAS":
                    return "Gas";
                case "RF_HEAT":
                    return "Heat";
                default:
                    return "Solid";
            }
        }
        private Factory AdaptaterFactory(FGFactory proto)
        {
            var item = Instance.Items.Where(x => x.Name == proto.Item.ClassName).FirstOrDefault();
            item.DisplayName = proto.DisplayName;
            item.Description = proto.Description;
            var speed = proto.Speed;
            var allowedResourceForms = new List<string>();
            var allowedResources = new List<string>();
            if (proto.Type == "Extractor")
            {
                // TODO cycle 0.5s? d'ou le facteur 2
                speed = 2 * proto.ItemsPerCycle / proto.ExtractCycleTime;
                if (Double.IsNaN(speed)) speed = 1;
                if (speed > 1000) speed /= 1000;

                if (!String.IsNullOrEmpty(proto.AllowedResourceForms.ToString()))
                {
                    var resourceForms = proto.AllowedResourceForms as List<object>;
                    if (resourceForms != null)
                    {
                        foreach (object resourceForm in resourceForms)
                        {
                            allowedResourceForms.Add(ConvertForm(resourceForm));
                        }
                    }
                }
                if (proto.AllowedResources != null)
                {
                    var resources = proto.AllowedResources as List<object>;
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
            }
            Factory factory = new Factory()
            {
                Item = item,
                Type = proto.Type,
                Speed = speed,
                PowerConsumption = proto.PowerConsumption,
                PowerProduction = proto.PowerProduction,
                AllowedResourceForms = allowedResourceForms,
                AllowedResources = allowedResources
            };
            factory.Properties.Add(new Property("PowerProductionExponent", proto.PowerProductionExponent));
            factory.Properties.Add(new Property("PowerConsumptionExponent", proto.PowerConsumptionExponent));
            return factory;
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
                            madeIn.Add(factory.Item.Name);
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
                Database = Instance,
                Name = proto.ClassName,
                DisplayName = proto.DisplayName,
                Energy = proto.ManufactoringDuration,
                MadeIn = madeIn,
                Type = typeof(Recipe).Name,
                Ingredients = ingredients,
                Products = products,
                IconPath = iconPath,
                Icon = icon,
                Tier = alternate ? 1 : 0
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
