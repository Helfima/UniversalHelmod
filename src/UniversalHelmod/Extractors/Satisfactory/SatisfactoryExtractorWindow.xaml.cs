using UniversalHelmod.Classes;
using UniversalHelmod.Extractors.Satisfactory.Models;
using UniversalHelmod.Workspaces.Models;
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

namespace UniversalHelmod.Extractors.Satisfactory
{
    /// <summary>
    /// Logique d'interaction pour SatisfactoryExtractorWindow.xaml
    /// </summary>
    public partial class SatisfactoryExtractorWindow : Window
    {
        public SatisfactoryExtractorWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var model = new SettingsModel();
            model.Path = Properties.Settings.Default.GameFolder;
            this.DataContext = model;
        }
        private SettingsModel Model => this.DataContext as SettingsModel;
        
        private void ButtonDirectory_Click(object sender, RoutedEventArgs e)
        {
            // To use System.Windows.Forms add <UseWindowsForms>true</UseWindowsForms> in .csproj file
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                string path = dialog.SelectedPath;
                Model.Path = path;
                Properties.Settings.Default.GameFolder = path;
            }
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
            PopulateDatabase();
        }

        private async void PopulateDatabase()
        {
            var result = await FGAdapater.PopulateDatabaseAsync();
            WorkspacesModel.Intance.Current.Database = result;
            Utils.ExtractImages(WorkspacesModel.Intance.Current.PathFolder, Model.Path);
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
