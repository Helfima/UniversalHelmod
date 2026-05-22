using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniversalHelmod.Classes;
using UniversalHelmod.Views;
using UniversalHelmod.Workspaces.Models;

namespace UniversalHelmod
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(WorkspacesModel.Intance.Current == null)
            {
                MenuWorkspace_Click(sender, e);
            }
        }
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MenuWorkspace_Click(object sender, RoutedEventArgs e)
        {
            var view = new Workspaces.WorkspacesWindow();
            view.Owner = this;
            view.ShowDialog();
        }
        public void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            WorkspacesModel.Intance.Current.SaveDataModel();
        }
        private void MenuItemDatabaseSave_Click(object sender, RoutedEventArgs e)
        {
            WorkspacesModel.Intance.Current.SaveDatabase();
        }
        private void MenuItemDatabase_Click(object sender, RoutedEventArgs e)
        {
            var view = new Databases.DatabaseWindow();
            view.Show();
        }
        private void MenuItemExtractSatisfactory_Click(object sender, RoutedEventArgs e)
        {
            var view = new Extractors.Satisfactory.SatisfactoryExtractorWindow();
            view.Show();
        }
        private void MenuItemExtractStationeers_Click(object sender, RoutedEventArgs e)
        {
            var view = new Extractors.Stationeers.StationeersExtractorWindow();
            view.Show();
        }
        private void MenuItemExtractStarRupture_Click(object sender, RoutedEventArgs e)
        {
            var view = new Extractors.StarRupture.StarRuptureSaveExtrator();
            view.Show();
        }
        private void MenuItemThemeMode_Click(object sender, RoutedEventArgs e)
        {
#pragma warning disable WPF0001 // Le type est utilisé à des fins d’évaluation uniquement et est susceptible d’être modifié ou supprimé dans les futures mises à jour. Supprimez ce diagnostic pour continuer.
            if(Application.Current.ThemeMode == ThemeMode.Light)
            {
                Application.Current.ThemeMode = ThemeMode.Dark;
            }
            else
            {
                Application.Current.ThemeMode = ThemeMode.Light;
            }
#pragma warning restore WPF0001 // Le type est utilisé à des fins d’évaluation uniquement et est susceptible d’être modifié ou supprimé dans les futures mises à jour. Supprimez ce diagnostic pour continuer.
        }
        private void MenuItemObjectivesShow_Click(object sender, RoutedEventArgs e)
        {
            WorkspacesModel.Intance.IsObjectivesShow = !WorkspacesModel.Intance.IsObjectivesShow;
        }
        private void MenuItemDarkMode_Click(object sender, RoutedEventArgs e)
        {
            bool isDarkMode = MenuItemDarkMode.IsChecked;
            ApplyTheme(isDarkMode);
            SaveThemePreference(isDarkMode);
        }

        private void ApplyTheme(bool isDarkMode)
        {
            var theme = new ResourceDictionary();
            if (isDarkMode)
            {
                theme.Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
            }
            else
            {
                theme.Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative);
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }

        private void SaveThemePreference(bool isDarkMode)
        {
            Properties.Settings.Default.IsDarkMode = isDarkMode;
            Properties.Settings.Default.Save();
        }

        private void LoadThemePreference()
        {
            bool isDarkMode = Properties.Settings.Default.IsDarkMode;
            MenuItemDarkMode.IsChecked = isDarkMode;
            ApplyTheme(isDarkMode);
        }

        class MainModel : NotifyProperty
        {
            private NotifyProperty currentModel;
            public NotifyProperty CurrentModel
            {
                get { return currentModel; }
                set { currentModel = value; NotifyPropertyChanged(); }
            }
            public MainModel()
            {
                currentModel = WorkspacesModel.Intance;
            }
        }
    }
}
