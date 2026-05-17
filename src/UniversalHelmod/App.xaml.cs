using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UniversalHelmod.Classes;

namespace UniversalHelmod
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            Logger.ApplicationInfo(assembly);
            // Ajouter des ressources au niveau de l'application
            Resources["CellIconNormalLeft"] = -3.0;
            Resources["CellIconNormalTop"] = -3.0;
            Resources["CellIconNormalWidth"] = 60.0;
            Resources["CellIconNormalHeight"] = 50.0;

            Resources["CellIconSmallLeft"] = -1.0;
            Resources["CellIconSmallTop"] = -1.0;
            Resources["CellIconSmallWidth"] = 30.0;
            Resources["CellIconSmallHeight"] = 25.0;
        }
    }
}
