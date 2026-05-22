using System;
using System.Globalization;
using System.Windows.Data;

namespace UniversalHelmod.Extractors.StarRupture
{
    public class SecondsToTimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double seconds)
            {
                var timeSpan = TimeSpan.FromSeconds(seconds);
                return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes:D2}m {timeSpan.Seconds:D2}s";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
