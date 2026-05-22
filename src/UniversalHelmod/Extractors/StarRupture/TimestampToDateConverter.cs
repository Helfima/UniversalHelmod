using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace UniversalHelmod.Extractors.StarRupture
{
    public class TimestampToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string timestamp &&
                DateTime.TryParseExact(timestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                return date.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
