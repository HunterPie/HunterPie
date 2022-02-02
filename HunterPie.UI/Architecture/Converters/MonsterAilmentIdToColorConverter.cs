using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Converters
{
    public class MonsterAilmentIdToColorConverter : IValueConverter
    {

        private readonly static SolidColorBrush _default = new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xFF));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Brush brush = Application.Current.TryFindResource($"COLOR_MONSTER_{value}") as Brush;
            return brush ?? _default;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
