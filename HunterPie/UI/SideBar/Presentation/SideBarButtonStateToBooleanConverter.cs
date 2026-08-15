using HunterPie.UI.Client.Sidebar.Entity;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HunterPie.UI.SideBar.Presentation;

internal class SideBarButtonStateToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not SideBarButtonState state)
            return false;

        return (state) switch
        {
            SideBarButtonState.Disabled => false,
            SideBarButtonState.Enabled => true,
            SideBarButtonState.Loading => false,
            _ => false,
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
}