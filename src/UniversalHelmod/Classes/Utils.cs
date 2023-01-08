using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using UniversalHelmod.Databases.Models;
using UniversalHelmod.Exceptions;
using UniversalHelmod.Workspaces.Models;

namespace UniversalHelmod.Classes
{
    public class Utils
    {
        public static string GetApplicationFolder()
        {
            string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = Path.Combine(local, "UniversalHelmod");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path;
        }
        public static string ImagesFolder()
        {
            string dirApp = WorkspacesModel.Intance.Current.PathFolder;
            string folder = Path.Combine(dirApp, "Images");
            return folder;
        }
        public static void ExtractImages(string workspaceFolder, string gameFolder)
        {
            if (!Directory.Exists(gameFolder)) return;
            ExecuteUModel(workspaceFolder, gameFolder, "*_256.uasset");
            ExecuteUModel(workspaceFolder, gameFolder, "*_256_New.uasset");
        }

        private static void ExecuteUModel(string workspaceFolder, string gameFolder, string filter)
        {

            string dirApp = Directory.GetCurrentDirectory();
            string pathPak = Path.Combine(gameFolder, "FactoryGame\\Content\\Paks");
            string cmd = Path.Combine(dirApp, "Data\\umodel_win32\\umodel_64.exe");
            string pathOut = Path.Combine(workspaceFolder, "Images");

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = false;

            startInfo.FileName = "cmd.exe";

            startInfo.Arguments = $"/C {cmd} -path=\"{pathPak}\" -out=\"{pathOut}\" -png -export {filter} -game=ue4.25";

            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();

            if (process.ExitCode == 0)
            {

            }
            else
            {

            }
        }

        public static BitmapImage GetImage(string path)
        {
            try
            {
                string dirApp = WorkspacesModel.Intance.Current.PathFolder;
                string fileName = Path.Combine(dirApp, path);
                if (File.Exists(fileName))
                {
                    Uri uri = new Uri(fileName);
                    var img = new BitmapImage(uri);
                    return img;
                }
                else
                {
                    return GetUnknownImage();
                }
            }
            catch
            {
                return GetUnknownImage();
            }
        }
        internal static BitmapImage GetUnknownImage()
        {
            Uri uri = new Uri($"pack://application:,,,/Images/Unknown.png");
            return new BitmapImage(uri);
        }

        public static void SelecElementIconPath(BaseIcon element)
        {
            // To use System.Windows.Forms add <UseWindowsForms>true</UseWindowsForms> in .csproj file
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Filter = "Image PNG: (*.png)|*.png|Image JPEG: (*.jpg)|*.jpg;*.jpeg|AllFiles:(*.*)|*.*";
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                string path = dialog.FileName;
                if (!String.IsNullOrEmpty(path))
                {
                    try
                    {
                        var imageFile = WorkspacesModel.Intance.Current.SaveImageIntoWorkspace(path, false);
                        element.IconPath = imageFile;
                    }
                    catch (ImageException ex)
                    {
                        // Configure message box
                        string message = "Hello, MessageBox!";
                        string caption = $"{ex.Message}\nDo you want to overwrite the image?";
                        MessageBoxButton buttons = MessageBoxButton.YesNo;
                        // Show message box
                        MessageBoxResult resultImage = MessageBox.Show(message, caption, buttons);
                        if (resultImage == MessageBoxResult.Yes)
                        {
                            var imageFile = WorkspacesModel.Intance.Current.SaveImageIntoWorkspace(path, true);
                            element.IconPath = imageFile;
                        }
                    }
                }
            }
        }
    }
}
