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
        private ObservableCollection<Factory> factories;
        public ObservableCollection<Factory> Factories
        {
            get { return factories; }
            set { factories = value; NotifyPropertyChanged(); }
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

        private ObservableCollection<Recipe> recipes;
        public ObservableCollection<Recipe> Recipes
        {
            get { return recipes; }
            set { recipes = value; NotifyPropertyChanged(); NotifyPropertyChanged("CountAlternateRecipe"); }
        }
        public int CountAlternateRecipe
        {
            get { return Recipes.Where(x => x.Alternate).Count(); }
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
    }
}
