using HunterPie.Core.Domain.Dialog;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters
{
    class DialogButtonToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NativeDialogButtons button = (NativeDialogButtons)parameter;
            NativeDialogButtons current = (NativeDialogButtons)value;
            return ((current & button) != 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
