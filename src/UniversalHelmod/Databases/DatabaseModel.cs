using UniversalHelmod.Classes;
using UniversalHelmod.Databases.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using UniversalHelmod.Extensions;
using System.Windows;

namespace UniversalHelmod.Databases
{
    class DatabaseModel : NotifyProperty
    {
        public Database Database { get; set; }

        private ObservableCollection<string> itemTypes;
        public ObservableCollection<string> ItemTypes
        {
            get { return itemTypes; }
            set { itemTypes = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<string> itemForms;
        public ObservableCollection<string> ItemForms
        {
            get { return itemForms; }
            set { itemForms = value; NotifyPropertyChanged(); }
        }
        
        
        private ObservableCollection<string> factoryTypes;
        public ObservableCollection<string> FactoryTypes
        {
            get { return factoryTypes; }
            set { factoryTypes = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<Factory> modules;
        public ObservableCollection<Factory> Modules
        {
            get { return modules; }
            set { modules = value; NotifyPropertyChanged(); }
        }
        public bool InitItemFilter { get; } = true;
        public bool InitRecipeFilter { get; } = true;
        public void Prepare()
        {
            var database = Workspaces.Models.WorkspacesModel.Intance.Current.Database;
            this.Database = database;
            this.ItemTypes = database.ItemTypes.ToObservableCollection();
            this.ItemForms = database.ItemForms.ToObservableCollection();
            this.Items = database.Items.Select(x => x.Clone()).ToObservableCollection();
            this.Factories = database.Factories.OfType<Factory>().ToObservableCollection();
            this.FactoryTypes = database.FactoryTypes.ToObservableCollection();
            this.Recipes = database.Recipes.ToObservableCollection();
            this.Logistics = database.Logistics.ToObservableCollection();
            RefreshViews();
        }
        public void RefreshViews()
        {
            RefreshFactoriesView();
            RefreshItemsView();
            RefreshRecipesView();
            RefreshLogisticsView();
        }

        public void Save()
        {
            this.Database.Items = this.Items.Select(x => x.Clone()).ToList();
            this.Database.Factories = this.Factories.Select(x => x.Clone()).ToList();
            this.Database.Recipes = this.Recipes.Select(x => x.Clone()).ToList();
            this.Database.Logistics = this.Logistics.Select(x => x.Clone()).ToList();
            this.Database.RefreshInternalList();
            Workspaces.Models.WorkspacesModel.Intance.Current.SaveDatabase();
        }
        #region ==== Item ====
        private ObservableCollection<Item> itemsView;
        public ObservableCollection<Item> ItemsView
        {
            get { return itemsView; }
            set { itemsView = value; NotifyPropertyChanged(); }
        }
        private string itemFilter;
        public string ItemFilter
        {
            get { return itemFilter; }
            set { itemFilter = value; NotifyPropertyChanged(); RefreshItemsView(); }
        }
        public void RefreshItemsView()
        {
            if (ItemFilter == null)
            {
                ItemsView = Items.ToObservableCollection();
            }
            else
            {
                ItemsView = Items.Where(x => x.Type == ItemFilter).ToObservableCollection();
            }
        }
        private ObservableCollection<Item> items;
        public ObservableCollection<Item> Items
        {
            get { return items; }
            set { items = value; NotifyPropertyChanged(); }
        }
        private Item selectedItem = new Item();
        public Item SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; NotifyPropertyChanged(); NotifyPropertyChanged("CountAlternateRecipe"); }
        }
        public void SaveItem(Item item)
        {
            var databaseItem = this.Items.Where(x => x.Name == item.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Items.Add(item);
            }
            else
            {
                if (this.Items.Remove(databaseItem))
                {
                    this.Items.Add(item);
                }
            }
            RefreshItemsView();
        }
        public void AddItem(Item item)
        {
            var databaseItem = this.Items.Where(x => x.Name == item.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Items.Add(item);
                RefreshItemsView();
            }
            else
            {
                throw new Exception("Already exist!");
            }
        }
        public void DeleteItem(Item item)
        {
            var databaseItem = this.Items.Where(x => x.Name == item.Name).FirstOrDefault();
            if (databaseItem != null)
            {
                if (CheckUsedItem(databaseItem)) return;
                if (this.Items.Remove(databaseItem))
                {
                    this.SelectedItem = new Item();
                }
                RefreshItemsView();
            }
        }
        private bool CheckUsedItem(Item item)
        {
            var recipes = new List<Recipe>();
            foreach(var recipe in this.Recipes)
            {
                var found = false;
                foreach (var product in recipe.Products)
                {
                    if (product.Name == item.Name)
                    {
                        recipes.Add(recipe);
                        found = true;
                        break;
                    }
                }
                if (found) continue;
                foreach (var product in recipe.Ingredients)
                {
                    if (product.Name == item.Name)
                    {
                        recipes.Add(recipe);
                        found = true;
                        break;
                    }
                }
            }
            if (recipes.Count > 0)
            {
                var messageBuilder = new StringBuilder();
                messageBuilder.AppendLine("Cannot be deleted, this item used in recipes:");
                foreach (var recipe in recipes)
                {
                    messageBuilder.AppendLine(recipe.Name);
                }
                MessageBox.Show(messageBuilder.ToString());
            }
            return recipes.Count > 0;
        }
        #endregion

        #region ==== Factory ====
        private ObservableCollection<Factory> factoriesView;
        public ObservableCollection<Factory> FactoriesView
        {
            get { return factoriesView; }
            set { factoriesView = value; NotifyPropertyChanged(); }
        }
        private string factoryFilter;
        public string FactoryFilter
        {
            get { return factoryFilter; }
            set { factoryFilter = value; NotifyPropertyChanged(); RefreshFactoriesView(); }
        }
        public void RefreshFactoriesView()
        {
            if (FactoryFilter == null)
            {
                FactoriesView = Factories.ToObservableCollection();
            }
            else
            {
                FactoriesView = Factories.Where(x => x.Type == FactoryFilter).ToObservableCollection();
            }
        }
        private ObservableCollection<Factory> factories;
        public ObservableCollection<Factory> Factories
        {
            get { return factories; }
            set { factories = value; NotifyPropertyChanged(); }
        }
        private Factory selectedFactory = new Factory();
        public Factory SelectedFactory
        {
            get { return selectedFactory; }
            set { selectedFactory = value; NotifyPropertyChanged(); NotifyPropertyChanged("CountAlternateRecipe"); }
        }
        public void SaveFactory(Factory factory)
        {
            var databaseItem = this.Factories.Where(x => x.Name == factory.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Factories.Add(factory);
            }
            else
            {
                if (this.Factories.Remove(databaseItem))
                {
                    this.Factories.Add(factory);
                }
            }
            RefreshFactoriesView();
        }
        public void AddFactory(Factory factory)
        {
            var databaseItem = this.Factories.Where(x => x.Name == factory.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Factories.Add(factory);
                RefreshFactoriesView();
            }
            else
            {
                throw new Exception("Already exist!");
            }
        }
        public void DeleteFactory(Factory factory)
        {
            var databaseItem = this.Factories.Where(x => x.Name == factory.Name).FirstOrDefault();
            if (databaseItem != null)
            {
                if (this.Factories.Remove(databaseItem))
                {
                    this.SelectedFactory = new Factory();
                }
                RefreshFactoriesView();
            }
        }
        #endregion

        #region ==== Logistic ====
        private ObservableCollection<Logistic> logisticsView;
        public ObservableCollection<Logistic> LogisticsView
        {
            get { return logisticsView; }
            set { logisticsView = value; NotifyPropertyChanged(); }
        }
        private string logisticFilter;
        public string LogisticFilter
        {
            get { return logisticFilter; }
            set { logisticFilter = value; NotifyPropertyChanged(); RefreshLogisticsView(); }
        }
        public void RefreshLogisticsView()
        {
            if (LogisticFilter == null)
            {
                LogisticsView = Logistics.ToObservableCollection();
            }
            else
            {
                LogisticsView = Logistics.Where(x => x.Form == LogisticFilter).ToObservableCollection();
            }
        }
        private ObservableCollection<Logistic> logistics;
        public ObservableCollection<Logistic> Logistics
        {
            get { return logistics; }
            set { logistics = value; NotifyPropertyChanged(); }
        }
        private Logistic selectedLogistic = new Logistic();
        public Logistic SelectedLogistic
        {
            get { return selectedLogistic; }
            set { selectedLogistic = value; NotifyPropertyChanged(); }
        }
        public void SaveLogistic(Logistic item)
        {
            var databaseItem = this.Logistics.Where(x => x.Name == item.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Logistics.Add(item);
            }
            else
            {
                if (this.Logistics.Remove(databaseItem))
                {
                    this.Logistics.Add(item);
                }
            }
            RefreshLogisticsView();
        }
        public void AddLogistic(Logistic item)
        {
            var databaseItem = this.Logistics.Where(x => x.Name == item.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Logistics.Add(item);
                RefreshLogisticsView();
            }
            else
            {
                throw new Exception("Already exist!");
            }
        }
        public void DeleteLogistic(Logistic item)
        {
            var databaseItem = this.Logistics.Where(x => x.Name == item.Name).FirstOrDefault();
            if (databaseItem != null)
            {
                if (this.Logistics.Remove(databaseItem))
                {
                    this.SelectedLogistic = new Logistic();
                }
                RefreshLogisticsView();
            }
        }
        
        #endregion

        #region ==== Recipe ====
        private ObservableCollection<Recipe> recipesView;
        public ObservableCollection<Recipe> RecipesView
        {
            get { return recipesView; }
            set { recipesView = value; NotifyPropertyChanged(); }
        }
        private string recipeFilter;
        public string RecipeFilter
        {
            get { return recipeFilter; }
            set { recipeFilter = value; NotifyPropertyChanged(); RefreshRecipesView(); }
        }
        public void RefreshRecipesView()
        {
            if (RecipeFilter == null)
            {
                RecipesView = Recipes.ToObservableCollection();
            }
            else
            {
                RecipesView = Recipes.Where(x => x.MainProduct.Type == RecipeFilter).ToObservableCollection();
            }
        }
        private ObservableCollection<Recipe> recipes;
        public ObservableCollection<Recipe> Recipes
        {
            get { return recipes; }
            set { recipes = value; NotifyPropertyChanged(); NotifyPropertyChanged("CountAlternateRecipe"); }
        }
        private Recipe selectedRecipe = new Recipe();
        public Recipe SelectedRecipe
        {
            get { return selectedRecipe; }
            set { selectedRecipe = value; NotifyPropertyChanged(); NotifyPropertyChanged("CountAlternateRecipe"); }
        }
        public void SaveRecipe(Recipe recipe)
        {
            var databaseItem = this.Recipes.Where(x => x.Name == recipe.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Recipes.Add(recipe);
            }
            else
            {
                if (this.Recipes.Remove(databaseItem))
                {
                    this.Recipes.Add(recipe);
                }
            }
            RefreshRecipesView();
        }
        public void AddRecipe(Recipe recipe)
        {
            var databaseItem = this.Recipes.Where(x => x.Name == recipe.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Recipes.Add(recipe);
                RefreshRecipesView();
            }
            else
            {
                throw new Exception("Already exist!");
            }
        }
        public void DeleteRecipe(Recipe recipe)
        {
            var databaseItem = this.Recipes.Where(x => x.Name == recipe.Name).FirstOrDefault();
            if (databaseItem != null)
            {
                if (this.Recipes.Remove(databaseItem))
                {
                    this.SelectedRecipe = new Recipe();
                }
                RefreshRecipesView();
            }
        }
        #endregion
    }
}
