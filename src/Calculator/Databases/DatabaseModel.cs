using Calculator.Classes;
using Calculator.Databases.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using Calculator.Extensions;

namespace Calculator.Databases
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

        private ObservableCollection<Factory> logistics;
        public ObservableCollection<Factory> Logistics
        {
            get { return logistics; }
            set { logistics = value; NotifyPropertyChanged(); }
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
        }
        public void Save()
        {
            this.Database.Items = this.Items.Select(x => x.Clone()).ToList();
            this.Database.Save();
            this.Database.RefreshInternalList();
        }
        #region ==== Item ====
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
        }
        public void AddItem(Item item)
        {
            var databaseItem = this.Items.Where(x => x.Name == item.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Items.Add(item);
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
                if (this.Items.Remove(databaseItem))
                {
                    this.SelectedItem = new Item();
                }
            }
        }
        #endregion

        #region ==== Factory ====
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
            var databaseItem = this.Factories.Where(x => x.Item == factory.Item).FirstOrDefault();
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
        }
        public void AddFactory(Factory factory)
        {
            var databaseItem = this.Factories.Where(x => x.Item == factory.Item).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Factories.Add(factory);
            }
            else
            {
                throw new Exception("Already exist!");
            }
        }
        public void DeleteFactory(Factory factory)
        {
            var databaseItem = this.Factories.Where(x => x.Item == factory.Item).FirstOrDefault();
            if (databaseItem != null)
            {
                if (this.Factories.Remove(databaseItem))
                {
                    this.SelectedFactory = new Factory();
                }
            }
        }
        #endregion

        #region ==== Recipe ====
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
        }
        public void AddRecipe(Recipe recipe)
        {
            var databaseItem = this.Recipes.Where(x => x.Name == recipe.Name).FirstOrDefault();
            if (databaseItem == null)
            {
                this.Recipes.Add(recipe);
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
            }
        }
        #endregion
    }
}
