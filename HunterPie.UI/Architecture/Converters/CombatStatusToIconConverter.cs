using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;

public class CombatStatusToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CombatStatus CombatStatus)
        {
            string iconName;

            switch (CombatStatus)
            {
                case CombatStatus.Battle:
                    iconName = "ICON_BATTLE_EYE";
                    break;
                case CombatStatus.Caution:
                    iconName = "ICON_CAUTION_EYE";
                    break;
                case CombatStatus.Battle_Move:
                    iconName = "ICON_BATTLE_MOVE_EYE";
                    break;
                default:
                    return null;
            }

            return Resources.Icon(iconName);
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
