using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

#nullable enable
public class GameProcessToGameNameConverter : IValueConverter
{

    public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is GameProcess game
            ? game switch
            {
                GameProcess.None => "None",
                GameProcess.MonsterHunterRise => Games.MONSTER_HUNTER_RISE,
                GameProcess.MonsterHunterWorld => Games.MONSTER_HUNTER_WORLD,
                _ => throw new NotImplementedException(),
            }
            : null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is string game
            ? (object)(game switch
            {
                "None" => GameProcess.None,
                Games.MONSTER_HUNTER_RISE => GameProcess.MonsterHunterRise,
                Games.MONSTER_HUNTER_WORLD => GameProcess.MonsterHunterWorld,
                _ => throw new NotImplementedException(),
            })
            : throw new ArgumentException($"Expected type {typeof(GameProcess)}, found {targetType}");
    }
}
