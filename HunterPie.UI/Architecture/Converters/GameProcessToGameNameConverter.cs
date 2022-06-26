using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters
{
    public class GameProcessToGameNameConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GameProcess game)
            {
                return game switch
                {
                    GameProcess.None => "None",
                    GameProcess.MonsterHunterRiseSunbreakDemo => Games.MONSTER_HUNTER_RISE_SUNBREAK_DEMO,
                    GameProcess.MonsterHunterRise => Games.MONSTER_HUNTER_RISE,
                    GameProcess.MonsterHunterWorld => Games.MONSTER_HUNTER_WORLD,
                    _ => throw new NotImplementedException(),
                };
            }

            throw new ArgumentException($"Expected type {typeof(GameProcess)}, found {targetType}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string game)
            {
                return game switch
                {
                    "None" => GameProcess.None,
                    Games.MONSTER_HUNTER_RISE_SUNBREAK_DEMO => GameProcess.MonsterHunterRiseSunbreakDemo,
                    Games.MONSTER_HUNTER_RISE => GameProcess.MonsterHunterRise,
                    Games.MONSTER_HUNTER_WORLD => GameProcess.MonsterHunterWorld,
                    _ => throw new NotImplementedException(),
                };
            }

            throw new ArgumentException($"Expected type {typeof(GameProcess)}, found {targetType}");
        }
    }
}
