using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Logger
{
    public class LogLevelToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((LogLevel)value)
            {
                case LogLevel.Debug:
                    return "[DEBUG]";
                case LogLevel.Info:
                    return "[INFO]";
                case LogLevel.Warn:
                    return "[WARN]";
                case LogLevel.Error:
                    return "[ERROR]";
                case LogLevel.Panic:
                    return "[PANIC]";
                case LogLevel.Benchmark:
                    return "[BENCHMARK]";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
