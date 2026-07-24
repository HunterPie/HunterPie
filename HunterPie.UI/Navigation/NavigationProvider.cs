using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation.Service;
using HunterPie.UI.Settings;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HunterPie.UI.Navigation;

#nullable enable
internal class NavigationProvider : INavigationProvider, INavigationRegistry
{
    private readonly Dictionary<Type, DataTemplate> Templates = new();

    /// <summary>
    /// Binds a view model to an specific view type
    /// </summary>
    /// <typeparam name="TActivity">Type of the view</typeparam>
    /// <typeparam name="TViewModel">Type of the view's view model</typeparam>
    public INavigationRegistry Bind<TActivity, TViewModel>()
        where TActivity : Activity
        where TViewModel : ViewModel
    {
        DataTemplate dataTemplate = DataTemplateFactory.Create<TActivity>();
        Templates.Add(typeof(TViewModel), dataTemplate);

        return this;
    }

    public DataTemplate? FindBy(Type viewModelType)
    {
        return Templates.GetValueOrDefault(viewModelType);
    }
}