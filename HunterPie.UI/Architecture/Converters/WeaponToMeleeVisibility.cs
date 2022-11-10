using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HunterPie.UI.Architecture.Converters;
public class WeaponToMeleeVisibility : IValueConverter
{

    private readonly HashSet<Weapon> _rangedWeapons = new() { Weapon.Bow, Weapon.LightBowgun, Weapon.HeavyBowgun };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Weapon weapon)
        {
            return _rangedWeapons.Contains(weapon) ? Visibility.Collapsed : Visibility.Visible;
        }

        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}
