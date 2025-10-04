using HunterPie.UI.Assets.Application;
using HunterPie.UI.Logging.Entity;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Logging.Presentation;

public class LogLevelToColorConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string colorName;
        switch ((LogLevel)value)
        {
            case LogLevel.Debug:
                colorName = "Brushes.HunterPie.Log.Debug";
                break;
            case LogLevel.Native:
                colorName = "Brushes.HunterPie.Log.Native";
                break;
            case LogLevel.Info:
                colorName = "Brushes.HunterPie.Log.Info";
                break;
            case LogLevel.Warn:
                colorName = "Brushes.HunterPie.Log.Warn";
                break;
            case LogLevel.Error:
                colorName = "Brushes.HunterPie.Log.Error";
                break;
            case LogLevel.Panic:
                colorName = "Brushes.HunterPie.Log.Panic";
                break;
            case LogLevel.Benchmark:
                colorName = "Brushes.HunterPie.Log.Benchmark";
                break;
            default:
                return null;
        }

        return Resources.Get<object>(colorName);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}