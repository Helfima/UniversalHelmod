using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calculator.Views
{
    /// <summary>
    /// Logique d'interaction pour RecipeSelector.xaml
    /// </summary>
    public partial class RecipeSelector : Window
    {
        private RecipeSelectorModel viewModel;

        public RecipeSelector()
        {
            InitializeComponent();
        }

        private void RecipeSelector_Loaded(object sender, RoutedEventArgs e)
        {
            var recipes = Database.Intance.Recipes;
            var categories = recipes.Select(x => x.MadeIn).Distinct().ToList();
            var category = categories.First();
            var recipesFiltered = recipes.Where(x => x.ItemType == Enums.ItemType.Item).ToList();
            this.viewModel = new RecipeSelectorModel()
            {
                Recipes = recipesFiltered
            };
            this.DataContext = this.viewModel;
            this.ViewCategories.SelectedItem = category;
        }

        public class RecipeSelectorModel
        {
            public List<string> Categories { get; set; }
            public List<Recipe> Recipes { get; set; }
            public string FilterItem { get; set; }
        }

        private void ViewCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var recipes = Database.Intance.Recipes;
            var category = (string)this.ViewCategories.SelectedItem;
            this.viewModel.Recipes = recipes;
            this.DataContext = this.viewModel;
            this.ViewRecipes.ItemsSource = recipes;
        }

    }
}
