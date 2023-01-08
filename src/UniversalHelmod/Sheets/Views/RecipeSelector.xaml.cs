using UniversalHelmod.Databases.Models;
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
using UniversalHelmod.Workspaces.Models;
using UniversalHelmod.Classes;
using System.Collections.ObjectModel;
using UniversalHelmod.Extensions;

namespace UniversalHelmod.Sheets.Views
{
    /// <summary>
    /// Logique d'interaction pour RecipeSelector.xaml
    /// </summary>
    public partial class RecipeSelector : Window
    {

        public RecipeSelector()
        {
            InitializeComponent();
        }

        private void RecipeSelector_Loaded(object sender, RoutedEventArgs e)
        {
            var recipes = WorkspacesModel.Intance.Current.Database.Recipes;
            var categories = new List<string>();
            foreach(var recipe in recipes )
            {
                categories.Add(recipe.ItemType);
            }
            categories = categories.Distinct().ToList();
            var category = categories.First();

            var recipesFiltered = recipes.Where(x => x.ItemType == category).ToList();

            var model = new RecipeSelectorModel();
            model.Recipes = recipesFiltered.ToObservableCollection();
            model.Categories = categories.ToObservableCollection();
            this.DataContext = model;
        }
        public RecipeSelectorModel Model => this.DataContext as RecipeSelectorModel;

        public class RecipeSelectorModel : NotifyProperty
        {
            private ObservableCollection<string> categories;
            public ObservableCollection<string> Categories
            {
                get { return categories; }
                set { categories = value; NotifyPropertyChanged(); }
            }
            private ObservableCollection<Recipe> recipes;
            public ObservableCollection<Recipe> Recipes
            {
                get { return recipes; }
                set { recipes = value; NotifyPropertyChanged(); }
            }
            public string FilterItem { get; set; }
        }

        private void ViewCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var category = (string)this.ViewCategories.SelectedItem;

            var recipes = WorkspacesModel.Intance.Current.Database.Recipes;
            var recipesFiltered = recipes.Where(x => x.ItemType == category).ToList();
            Model.Recipes = recipesFiltered.ToObservableCollection();
        }

    }
}
