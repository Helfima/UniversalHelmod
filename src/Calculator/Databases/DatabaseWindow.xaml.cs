using Calculator.Classes;
using Calculator.Databases.Models;
using Calculator.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calculator.Databases
{
    /// <summary>
    /// Logique d'interaction pour DatabaseWindow.xaml
    /// </summary>
    public partial class DatabaseWindow : Window
    {
        public DatabaseWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseModel model = new DatabaseModel();
            model.Database = Workspaces.Models.WorkspacesModel.Intance.Current.Database;
            model.ItemTypes = model.Database.ItemTypes.ToObservableCollection();
            model.Items = model.Database.Items.ToObservableCollection();
            model.Factories = model.Database.Factories.OfType<Factory>().ToObservableCollection();
            model.FactoryTypes = model.Database.FactoryTypes.ToObservableCollection();
            model.Recipes = model.Database.Recipes.ToObservableCollection();
            this.DataContext = model;
        }

        private DatabaseModel Model => DataContext as DatabaseModel;
        private void ItemFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                var filter = button.Content.ToString();
                if (filter == "None")
                {
                    Model.Items = Model.Database.Items.ToObservableCollection();
                }
                else
                {
                    Model.Items = Model.Database.Items.Where(x => x.ItemType == filter).ToObservableCollection();
                }
            }
        }
        private void FactoryFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                var filter = button.Content.ToString();
                if (filter == "None")
                {
                    Model.Factories = Model.Database.Factories.ToObservableCollection();
                }
                else
                {
                    Model.Factories = Model.Database.Factories.Where(x => x.ItemType == filter).ToObservableCollection();
                }
            }
        }
        private void RecipeFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                var filter = button.Content.ToString();
                if (filter == "None")
                {
                    Model.Recipes = Model.Database.Recipes.ToObservableCollection();
                }
                else
                {
                    Model.Recipes = Model.Database.Recipes.Where(x => x.ItemType == filter).ToObservableCollection();
                }
            }
        }

        class DatabaseModel : NotifyProperty
        {
            public Database Database { get; set; }

            private ObservableCollection<string> itemTypes;
            public ObservableCollection<string> ItemTypes
            {
                get { return itemTypes; }
                set { itemTypes = value; NotifyPropertyChanged(); }
            }
            private ObservableCollection<Item> items;
            public ObservableCollection<Item> Items
            {
                get { return items; }
                set { items = value; NotifyPropertyChanged(); }
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
        }
    }
}
