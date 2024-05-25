using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalHelmod.Classes;
using UniversalHelmod.Databases.Models;
using UniversalHelmod.Extractors.Satisfactory.Models;
using UniversalHelmod.Workspaces.Models;

namespace UniversalHelmod.Extractors.Stationeers.Models
{
    public class RSAdaptater
    {
        private Database database;

        private RSAdaptater(Database instance)
        {
            this.database = instance;
        }

        public static async Task<Database> PopulateDatabaseAsync()
        {
            return await Task.Run(() => PopulateDatabase());
        }
        public static Database PopulateDatabase()
        {
            var instance = new Database();
            var adaptater = new RSAdaptater(instance);
            adaptater.Execute();
            return instance;
        }

        private void Execute()
        {
            Logger.Info("****** RSAdaptater started ******");
            SettingsModel.InvokeMessage(this, "Parse data");
            var source = new RSDatabase();
            source.Load();
            SettingsModel.InvokeMessage(this, "Convert Items...");
            foreach (var rsItem in source.Items)
            {
                ConvertItem(rsItem);
            }
            foreach (var rsFactory in source.Factories)
            {
                ConvertFactory(rsFactory);
            }
            foreach (var rsRecipe in source.Recipes)
            {
                ConvertRecipe(rsRecipe);
            }
            SettingsModel.InvokeMessage(this, "Database ready");
            Logger.Info("****** RSAdaptater finished ******");
        }
        private void ConvertItem(RSItem rsItem)
        {
            var item = new Item();
            item.Name = rsItem.PrefabName;
            item.DisplayName = rsItem.Name;
            item.Type = rsItem.Type;
            PrepareIcon(item.Name, item);
            database.Items.Add(item);
        }
        private void ConvertFactory(RSFactory rsFactory)
        {
            var factory = new Factory();
            factory.Name = rsFactory.PrefabName;
            factory.DisplayName = rsFactory.Name;
            factory.PowerConsumption = 1;
            factory.PowerProduction = 0;
            PrepareIcon(factory.Name, factory);
            database.Factories.Add(factory);
        }
        private Item ConvertAmount(RSAmount amount, string type)
        {
            var item = new Item();
            item.Name = amount.PrefabName;
            item.DisplayName = amount.Name;
            item.Type= type;
            PrepareIcon(item.Name, item);
            database.Items.Add(item);
            return item;
        }
        private List<string> ConvertMadeIn(HashSet<string> factoryNames)
        {
            var list = new List<string>();
            foreach (var factoryName in factoryNames)
            {
                var factory = database.Factories.FirstOrDefault(x => x.DisplayName == factoryName);
                list.Add(factory.Name);
            }
            return list;
        }
        private void ConvertRecipe(RSRecipe rsRecipe)
        {
            var recipe = new Recipe();
            recipe.Name = rsRecipe.PrefabName;
            recipe.DisplayName = rsRecipe.Name;
            recipe.Type = rsRecipe.Type;
            if (rsRecipe.Energy > 0 && rsRecipe.Time > 0)
            {
                recipe.Energy = rsRecipe.Energy / rsRecipe.Time;
            }
            else 
            {
                recipe.Energy = 1;
            }
            recipe.MadeIn = ConvertMadeIn(rsRecipe.MadeIn);
            PrepareIcon(recipe.Name, recipe);
            foreach (var rsProduct in rsRecipe.Products)
            {
                var item = database.Items.FirstOrDefault(x => x.Name == rsProduct.PrefabName);
                if (item == null)
                {
                    Logger.Trace($"Create missing item {rsProduct.PrefabName,-20} for {rsRecipe.PrefabName,-25} in {String.Join(",", rsRecipe.MadeIn)}");
                    item = ConvertAmount(rsProduct, rsRecipe.Type);
                }
                var product = new Amount(item, rsProduct.Count);
                recipe.Products.Add(product);
            }
            foreach (var rsIngredient in rsRecipe.Ingredients)
            {
                var item = database.Items.FirstOrDefault(x => x.Name == rsIngredient.PrefabName);
                if (item == null)
                {
                    Logger.Trace($"Create missing item {rsIngredient.PrefabName,-20} for {rsRecipe.PrefabName,-25} in {String.Join(",", rsRecipe.MadeIn)}");
                    item = ConvertAmount(rsIngredient, rsRecipe.Type);
                }
                var ingredient = new Amount(item, rsIngredient.Count);
                recipe.Ingredients.Add(ingredient);
            }
            database.Recipes.Add(recipe);
        }
        private void PrepareIcon(string prefabName, BaseIcon baseIcon)
        {
            switch (prefabName)
            {
                case "ItemKitLiquidRegulator":
                    prefabName = "ItemKiLiquidRegulator";
                    break;
                case "CartridgePlantAnalyser":
                    prefabName = "ItemCartridge";
                    break;
            }
            var workspace = WorkspacesModel.Intance.Current;
            var targetFolder = Path.Combine(workspace.PathFolder, "Images");
            if(Directory.Exists(targetFolder) == false)
            {
                Directory.CreateDirectory(targetFolder);
            }
            var targetPath = Path.Combine(targetFolder, $"{prefabName}.png");
            var sourceFolder = "C:\\Temp\\export_stationeers\\resources.assets\\ExportedProject\\Assets\\Texture2D";
            var sourcePath = Path.Combine(sourceFolder, $"{prefabName}.png");
            if(File.Exists(sourcePath) == true)
            {
                File.Copy(sourcePath, targetPath, true);
                baseIcon.IconPath = targetPath;
            }
            else
            {
                Logger.Error($"Icon not found for {prefabName}");
            }
        }
    }
}
