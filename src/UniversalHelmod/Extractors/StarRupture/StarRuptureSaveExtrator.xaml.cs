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
using UniversalHelmod.Classes;
using UniversalHelmod.Extractors.StarRupture.Models;

namespace UniversalHelmod.Extractors.StarRupture
{
    /// <summary>
    /// Logique d'interaction pour StarRuptureSaveExtrator.xaml
    /// </summary>
    public partial class StarRuptureSaveExtrator : Window
    {
        public StarRuptureSaveExtrator()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = new StarRuptureSaveModel();
                model.Path = Properties.Settings.Default.StarRuptureFolderSaves;
                var process = new ProcessStarRupture();
                process.LoadFiles(model);
                this.DataContext = model;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        private StarRuptureSaveModel Model => this.DataContext as StarRuptureSaveModel;

        private void ButtonDirectory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // To use System.Windows.Forms add <UseWindowsForms>true</UseWindowsForms> in .csproj file
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    string path = dialog.SelectedPath;
                    Model.Path = path;
                    Properties.Settings.Default.StarRuptureFolderSaves = path;
                    Properties.Settings.Default.Save();
                    var process = new ProcessStarRupture();
                    process.LoadFiles(Model);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void ExtractSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var source = (dynamic) e.Source;
                var item = source.DataContext as FileSave;
                if (item != null)
                {
                    var process = new ProcessStarRupture();
                    process.ExtractSave(item.SaveFilename);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
