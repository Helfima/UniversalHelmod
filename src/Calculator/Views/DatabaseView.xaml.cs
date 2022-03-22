using Calculator.Enums;
using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using Calculator.Extensions;

namespace Calculator.Views
{
    /// <summary>
    /// Logique d'interaction pour DatabaseView.xaml
    /// </summary>
    public partial class DatabaseView : Window
    {
        public DatabaseView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DatabaseModel model = new DatabaseModel();
            model.Items = Database.Intance.Items.ToObservableCollection();
            model.Recipes = Database.Intance.Recipes.ToObservableCollection();
            this.DataContext = model;
        }

        private DatabaseModel Model => DataContext as DatabaseModel;
        private void ItemFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                var content = button.Content.ToString();
                ItemType filter = ItemType.None;
                Enum.TryParse(content, out filter);
                if (filter == ItemType.None)
                {
                    Model.Items = Database.Intance.Items.ToObservableCollection();
                }
                else
                {
                    Model.Items = Database.Intance.Items.Where(x => x.ItemType == filter).ToObservableCollection();
                }
            }
        }
        private void RecipeFilter_Checked(object sender, RoutedEventArgs e)
        {
            var button = e.Source as RadioButton;
            if (button != null)
            {
                var content = button.Content.ToString();
                ItemType filter = ItemType.None;
                Enum.TryParse(content, out filter);
                if (filter == ItemType.None)
                {
                    Model.Recipes = Database.Intance.Recipes.ToObservableCollection();
                }
                else
                {
                    Model.Recipes = Database.Intance.Recipes.Where(x => x.ItemType == filter).ToObservableCollection();
                }
            }
        }

        class DatabaseModel : NotifyProperty
        {
            private ObservableCollection<Item> items;
            public ObservableCollection<Item> Items
            {
                get { return items; }
                set { items = value; NotifyPropertyChanged(); }
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
