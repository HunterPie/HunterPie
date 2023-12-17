using HunterPie.UI.Architecture;
using HunterPie.UI.Settings;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HunterPie.UI.Navigation;

#nullable enable
public static class NavigationProvider
{
    private static readonly Dictionary<Type, DataTemplate> Templates = new();

    public static void Register<TView, TViewModel>()
        where TView : FrameworkElement
        where TViewModel : ViewModel
    {
        DataTemplate dataTemplate = DataTemplateFactory.Create<TView>();
        Templates.Add(typeof(TViewModel), dataTemplate);
    }

    public static DataTemplate? FindBy(Type viewModelType)
    {
        return Templates.GetValueOrDefault(viewModelType);
    }
}