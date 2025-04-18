using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Constants;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class GameTypeToGameNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (GameType)value switch
        {
            GameType.Rise => Games.MONSTER_HUNTER_RISE,
            GameType.World => Games.MONSTER_HUNTER_WORLD,
            GameType.Wilds => Games.MONSTER_HUNTER_WILDS,
            _ => "unknown"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}