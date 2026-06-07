using System;
using System.Globalization;
using System.Windows.Data;
using UniversalHelmod.Databases.Models;
using UniversalHelmod.Workspaces.Models;

namespace UniversalHelmod.Views             
{
    public class DisplayValueConverter : IMultiValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0 || values[0] == null) return null;
            double result = (double)values[0];
            if (values.Length > 1 && values[1] is Element element)
            {
                result /= element.Count;
            }
            if(parameter != null)
            {
                return $"{result:N2}{parameter}";
            }
            return $"{result:N2}";
        }
    }
}
