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
                recipe.ComputeFlow();
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
                Form = proto.Form == "RF_SOLID" ? ItemForm.Solid : ItemForm.Liquid,
                ResourceSinkPoints = proto.ResourceSinkPoints,
                Icon = icon,
                ItemType = proto.DisplayName.Contains("FICSMAS") || proto.DisplayName.Contains("Candy") || proto.DisplayName.Contains("Snow")
                ? ItemType.FICSMAS : proto.ItemType
            };
            return item;
        }
        private Factory AdaptaterFactory(FGFactory proto)
        {
            var icon = GetIcon(proto.PersistentBigIcon);
            if (icon == null)
            {
                icon = GetIcon(proto.SmallIcon);
            }
            var allowedResourceForms = ItemForm.Solid;
            var isMiner = false;
            if (!String.IsNullOrEmpty(proto.AllowedResourceForms.ToString()))
            {
                var resourceForms = proto.AllowedResourceForms as List<object>;
                if(resourceForms != null)
                {
                    if(resourceForms[0].ToString() == "RF_LIQUID")
                    {
                        allowedResourceForms = ItemForm.Liquid;
                    }
                }
                isMiner = true;
            }
            var allowedResources = new List<string>();
            if (proto.AllowedResources != null)
            {
                var resources = proto.AllowedResources as List<object>;
                if(resources != null)
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
            Factory item = new Factory()
            {
                Name = proto.ClassName,
                DisplayName = proto.DisplayName,
                Description = proto.Description,
                Icon = icon,
                ItemType = proto.ItemType,
                Speed = proto.Speed,
                PowerConsumption = proto.PowerConsumption,
                PowerConsumptionExponent = proto.PowerConsumptionExponent,
                PowerProduction = proto.PowerProduction,
                PowerProductionExponent = proto.PowerProductionExponent,
                AllowedResourceForms = allowedResourceForms,
                AllowedResources = allowedResources,
                IsMiner = isMiner
            };
            return item;
        }
        private Recipe AdaptaterRecipe(FGRecipe proto)
        {
            var ingredients = AdaptaterItems(proto.Ingredients);
            var products = AdaptaterItems(proto.Product);
            var itemType = ItemType.None;
            var masterProduct = products.FirstOrDefault();
            bool alternate = proto.DisplayName.StartsWith("Alternate");
            if (masterProduct != null)
            {
                itemType = masterProduct.ItemType;
            }
            var icon = products.First().Icon;
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
                        var miners = Instance.Factories.Where(x => x.IsMiner);
                        var factories = miners.Where(delegate(Factory miner) {
                            return miner.AllowedResourceForms == item.Form && (miner.AllowedResources.Count == 0 || miner.AllowedResources.Contains(item.Name));
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
                Icon = icon,
                ItemType = itemType,
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
                if (item.Form == ItemForm.Liquid)
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
        private BitmapImage GetIcon(string icon)
        {
            string pattern = "/([^ ]*)\\.[^.]*$";
            Match match = Regex.Match(icon, pattern);
            BitmapImage image = null;
            if (match.Success)
            {
                string name = match.Groups[1].Value;
                name = name.Replace("512", "256");
                image = GetImage($"{name}.png");
            }
            if (image != null) return image;
            return GetUnknownImage();
        }
        internal BitmapImage GetUnknownImage()
        {
            Uri uri = new Uri($"pack://application:,,,/Images/Unknown.png");
            return new BitmapImage(uri);
        }
        private BitmapImage GetImage(string name)
        {
            try
            {
                name = name.Replace('/', '\\');
                string dirApp = Directory.GetCurrentDirectory();
                string fileName = Path.Combine(dirApp, "Images", name);
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                var img = new BitmapImage();
                img.BeginInit();
                img.StreamSource = fileStream;
                img.EndInit();
                return img;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
