using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace UniversalHelmod.Views
{
    public class RoundNumberConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object paramater, CultureInfo culture)
        {
            return value;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double result = 0;
            if (value != null && (double.TryParse(value.ToString(), out result)))
                return $"{result:N2}";
            else
                return value;
        }
    }
}
