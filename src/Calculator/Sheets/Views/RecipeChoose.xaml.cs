using Calculator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Calculator.Sheets.Views
{
    
    /// <summary>
    /// Logique d'interaction pour RecipeChoose.xaml
    /// </summary>
    public partial class RecipeChoose : Window
    {
        private RecipeChooseModel viewModel;
        public RecipeChoose()
        {
            InitializeComponent();
        }

        public void SetRecipeByProduct(Item item)
        {
            List<Recipe> recipes = Database.Intance.SelectRecipeByProduct(item);
            viewModel = new RecipeChooseModel()
            {
                Recipes = recipes,
                ByProduct = item
            };
            this.DataContext = this.viewModel;
            this.ViewRecipes.Items.Refresh();
        }
    }

    public class RecipeChooseModel
    {
        public List<Recipe> Recipes { get; set; }
        public Item ByProduct { get; set; }
    }
}
