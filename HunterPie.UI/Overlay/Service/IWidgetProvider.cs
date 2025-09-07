using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.UI.Overlay.Service;

#nullable enable
public interface IWidgetProvider
{
    /// <summary>
    /// Binds a ViewModel type to a data template
    /// </summary>
    /// <typeparam name="TViewModel">ViewModel type</typeparam>
    /// <param name="template">DataTemplate of the Widget that should be rendered for that ViewModel</param>
    public void Bind<TViewModel>(DataTemplate template) where TViewModel : ViewModel;

    /// <summary>
    /// Provides the DataTemplate that should be used in the given context
    /// </summary>
    /// <param name="context">Widget context</param>
    /// <returns>DataTemplate for the widget</returns>
    public DataTemplate? Provide(ViewModel context);
}