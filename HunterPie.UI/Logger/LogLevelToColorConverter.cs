using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Logger
{
    public class LogLevelToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorName;
            switch ((LogLevel)value)
            {
                case LogLevel.Debug:
                    colorName = "LOGGER_LEVEL_DEBUG";
                    break;
                case LogLevel.Info:
                    colorName = "LOGGER_LEVEL_INFO";
                    break;
                case LogLevel.Warn:
                    colorName = "LOGGER_LEVEL_WARN";
                    break;
                case LogLevel.Error:
                    colorName = "LOGGER_LEVEL_ERROR";
                    break;
                case LogLevel.Panic:
                    colorName = "LOGGER_LEVEL_PANIC";
                    break;
                default:
                    return null;
            }

            return Application.Current.FindResource(colorName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
