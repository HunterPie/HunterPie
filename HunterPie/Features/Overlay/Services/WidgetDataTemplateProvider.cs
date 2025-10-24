using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Settings;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HunterPie.Features.Overlay.Services;

internal class WidgetDataTemplateProvider : IWidgetProvider
{
    private readonly Dictionary<Type, DataTemplate> _templates = new();

    public void Bind<TViewModel, TView>()
        where TViewModel : ViewModel
        where TView : Widget
    {
        _templates.Add(typeof(TViewModel), DataTemplateFactory.Create<TView>());
    }

    public DataTemplate? Provide(ViewModel context)
    {
        return _templates.GetValueOrDefault(context.GetType());
    }
}