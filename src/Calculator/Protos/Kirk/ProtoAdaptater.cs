using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Calculator.Protos.Kirk
{
    public class ProtoAdaptater
    {
        public static void PopulateDatabase(Database instance)
        {
            instance.Items = new List<Item>();
            foreach (ProtoItem protoItem in ProtoDatabase.Intance.Items)
            {
                var item = AdaptaterItem(protoItem);
                instance.Items.Add(item);
            }

            instance.Factories = new List<Factory>();
            foreach (ProtoBuilding protoBuilding in ProtoDatabase.Intance.Buildings)
            {
                var factory = AdaptaterFactory(protoBuilding);
                instance.Factories.Add(factory);
            }
            foreach (ProtoMiner protoMiner in ProtoDatabase.Intance.Miners)
            {
                var factory = AdaptaterFactory(protoMiner);
                instance.Factories.Add(factory);
            }

            instance.Logistics = new List<Logistic>();
            foreach (ProtoBelt protoBelt in ProtoDatabase.Intance.Belts)
            {
                var logistic = AdaptaterLogistic(protoBelt);
                instance.Logistics.Add(logistic);
            }
            foreach (ProtoPipe protoPipe in ProtoDatabase.Intance.Pipes)
            {
                var logistic = AdaptaterLogistic(protoPipe);
                instance.Logistics.Add(logistic);
            }

            instance.Recipes = new List<Recipe>();
            foreach (ProtoRecipe protoRecipe in ProtoDatabase.Intance.Recipes)
            {
                var recipe = AdaptaterRecipe(protoRecipe, instance);
                recipe.ComputeFlow();
                instance.Recipes.Add(recipe);
            }
        }
        private static Item AdaptaterItem(ProtoItem protoItem)
        {
            Item item = new Item()
            {
                Name = protoItem.Key,
                Icon = protoItem.Icon
            };
            return item;
        }
        private static Amount AdaptaterAmount(ProtoItem protoItem)
        {
            Item item = new Item()
            {
                Name = protoItem.Key,
                Icon = protoItem.Icon
            };
            return new Amount(item, protoItem.Count);
        }
        private static Logistic AdaptaterLogistic(ProtoBelt protoBelt)
        {
            Logistic logistic = new Logistic()
            {
                Name = protoBelt.Key,
                Icon = protoBelt.Icon,
                Rate = protoBelt.Rate,
                Transport = Enums.ItemForm.Solid
            };
            return logistic;
        }
        private static Logistic AdaptaterLogistic(ProtoPipe protoPipe)
        {
            Logistic logistic = new Logistic()
            {
                Name = protoPipe.Key,
                Icon = protoPipe.Icon,
                Rate = protoPipe.Rate,
                Transport = Enums.ItemForm.Liquid
            };
            return logistic;
        }
        private static Factory AdaptaterFactory(ProtoBuilding protoBuilding)
        {
            Factory factory = new Factory()
            {
                Name = protoBuilding.Key,
                Icon = protoBuilding.Icon,
                Category = protoBuilding.Category,
                Max = protoBuilding.Max,
                Speed = 1,
                PowerConsumption = protoBuilding.Power,
                IsMiner = false
            };
            return factory;
        }
        private static Factory AdaptaterFactory(ProtoMiner protoMiner)
        {
            Factory factory = new Factory()
            {
                Name = protoMiner.Key,
                Icon = protoMiner.Icon,
                Category = protoMiner.Category,
                Rate = protoMiner.Rate,
                Speed = protoMiner.Rate * 2,
                IsMiner = true
            };
            return factory;
        }

        private static ObservableCollection<Amount> AdaptaterItems(List<ProtoItem> protoItems)
        {
            ObservableCollection<Amount> items = new ObservableCollection<Amount>();
            foreach (ProtoItem protoItem in protoItems)
            {
                items.Add(AdaptaterAmount(protoItem));
            }
            return items;
        }

        private static Recipe AdaptaterRecipe(ProtoRecipe protoRecipe, Database instance)
        {
            var factory = instance.Factories.Where(x => x.Category == protoRecipe.Category).OrderBy(x => x.Speed).FirstOrDefault();
            Recipe recipe = new Recipe()
            {
                Name = protoRecipe.Key,
                Icon = protoRecipe.Icon,
                Energy = protoRecipe.Time,
                MadeIn = new List<string> { protoRecipe.Category },
                Builder = new Builder(factory),
                Type = typeof(Node).Name,
                Ingredients = AdaptaterItems(protoRecipe.Ingredients),
                Products = AdaptaterItems(protoRecipe.Products)
            };
            return recipe;
        }

        
    }
}
