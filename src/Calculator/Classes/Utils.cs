using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Calculator.Classes
{
    public class Utils
    {
        public static string ImagesFolder()
        {
            string dirApp = Directory.GetCurrentDirectory();
            string folder = Path.Combine(dirApp, "Images");
            return folder;
        }
        public static void ExtractImages()
        {
            string gameFolder = Properties.Settings.Default.GameFolder;
            if (!Directory.Exists(gameFolder)) return;
            ExecuteUModel(gameFolder, "*_256.uasset");
            ExecuteUModel(gameFolder, "*_256_New.uasset");
        }

        private static void ExecuteUModel(string gameFolder, string filter)
        {

            string dirApp = Directory.GetCurrentDirectory();
            string pathPak = Path.Combine(gameFolder, "FactoryGame\\Content\\Paks");
            string cmd = Path.Combine(dirApp, "Data\\umodel_win32\\umodel_64.exe");
            string pathOut = ImagesFolder();

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
    }
}
