using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace Calculator.Views
{
    public class PowerShardConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int test = System.Convert.ToInt32(value);
            int param = System.Convert.ToInt32(parameter);
            if(test >= param)
            {
                return "Orange";
            }
            return "Gray";
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
