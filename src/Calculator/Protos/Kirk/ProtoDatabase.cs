using Calculator.Models;
using Calculator.Protos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Media.Imaging;

namespace Calculator.Protos.Kirk
{
    [DataContract]
    class ProtoDatabase
    {
        [DataMember(Name = "belts", IsRequired = true)]
        public List<ProtoBelt> Belts { get; set; }

        [DataMember(Name = "pipes", IsRequired = true)]
        public List<ProtoPipe> Pipes { get; set; }

        [DataMember(Name = "buildings", IsRequired = true)]
        public List<ProtoBuilding> Buildings { get; set; }

        [DataMember(Name = "miners", IsRequired = true)]
        public List<ProtoMiner> Miners { get; set; }

        [DataMember(Name = "items", IsRequired = true)]
        public List<ProtoItem> Items { get; set; }

        [DataMember(Name = "recipes", IsRequired = true)]
        public List<ProtoRecipe> Recipes { get; set; }

        [DataMember(Name = "resources", IsRequired = true)]
        public List<ProtoResource> Resources { get; set; }

        private static string GetFilename()
        {
            string root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(root, "Data/update5.json");
        }
        public static string Filename => GetFilename();

        public static ProtoDatabase Intance => GetInstance();

        private static ProtoDatabase instance;
        private static ProtoDatabase GetInstance()
        {
            if (instance == null) Load();
            return instance;
        }
        public static void Load()
        {
            using (StreamReader stream = new StreamReader(Filename))
            {
                instance = (ProtoDatabase)(new DataContractJsonSerializer(typeof(ProtoDatabase))).ReadObject(stream.BaseStream);
            }
            instance.FinalizeData();
        }

        public void Save()
        {
            using (StreamWriter stream = new StreamWriter(Filename))
            {
                new DataContractJsonSerializer(typeof(ProtoDatabase)).WriteObject(stream.BaseStream, this);
            }
        }

        internal void FinalizeData()
        {
            foreach (ProtoBelt proto in instance.Belts)
            {
                proto.Icon = GetImage(proto.Name);
            }
            foreach (ProtoPipe proto in instance.Pipes)
            {
                proto.Icon = GetImage(proto.Name);
            }
            foreach (ProtoBuilding proto in instance.Buildings)
            {
                proto.Icon = GetImage(proto.Name);
            }
            foreach (ProtoMiner proto in instance.Miners)
            {
                proto.Icon = GetImage(proto.Name);
            }
            foreach (ProtoItem proto in instance.Items)
            {
                proto.Icon = GetImage(proto.Name);
            }

            foreach (ProtoRecipe recipe in instance.Recipes)
            {
                recipe.Ingredients = GetItems(recipe.ArrayIngredients);
                recipe.Products = new List<ProtoItem>();
                // product
                var product = GetItem(recipe.Product);
                recipe.Products.Add(product);
                // by product
                var byproduct = GetItem(recipe.ByProduct);
                if(byproduct.Count < 999) recipe.Products.Add(byproduct);
                ProtoItem mainItem = recipe.Products.FirstOrDefault();
                recipe.Icon = mainItem.Icon;
            }
        }
        internal BitmapImage GetImage(string name)
        {
            Uri uri = new Uri($"pack://application:,,,/Images/{name}.png");
            return new BitmapImage(uri);
        }
        internal List<ProtoItem> GetItems(List<object> arrayItems)
        {
            List<ProtoItem> newItems = new List<ProtoItem>();
            foreach (object arrayItem in arrayItems)
            {
                var newItem = GetItem(arrayItem);
                newItems.Add(newItem);
            }
            return newItems;
        }
        internal ProtoItem GetItem(object arrayItem)
        {
            var items = ProtoDatabase.Intance.Items;
            object[] item = arrayItem as object[];
            var newItem = items.FirstOrDefault(x => x.Key == (string)item[0])?.Clone();
            newItem.Count = (dynamic)item[1];
            return newItem;
        }
    }
}