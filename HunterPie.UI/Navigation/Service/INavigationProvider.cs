using System;
using System.Windows;

namespace HunterPie.UI.Navigation.Service;

#nullable enable
public interface INavigationProvider
{
    public DataTemplate? FindBy(Type viewModelType);
}