using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using UniversalHelmod.Enums;

namespace UniversalHelmod.Sheets.Views
{
    public class CellBackgroundConverter : IValueConverter
    {
        public Brush NoneBrush { get; set; } = Brushes.Transparent;
        public Brush DefaultBrush { get; set; } = Brushes.Transparent;
        public Brush NormalBrush { get; set; }
        public Brush MainBrush { get; set; }
        public Brush ResidualBrush { get; set; }
        public Brush OverflowBrush { get; set; }
        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemState state = ItemState.Normal;
            if(value != null)
            {
                Enum.TryParse(value.ToString(), out state);
            }
            switch (state)
            {
                case ItemState.Normal:
                    return NormalBrush == null ? DefaultBrush : NormalBrush;
                case ItemState.Main:
                    return MainBrush == null ? DefaultBrush : MainBrush;
                case ItemState.Overflow:
                    return OverflowBrush == null ? DefaultBrush : OverflowBrush;
                case ItemState.Residual:
                    return ResidualBrush == null ? DefaultBrush : ResidualBrush;
                default:
                    return NoneBrush;
            }
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
