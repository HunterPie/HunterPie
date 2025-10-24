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
    /// /// <typeparam name="TView">Widget type</typeparam>
    public void Bind<TViewModel, TView>()
        where TViewModel : ViewModel
        where TView : Widget;

    /// <summary>
    /// Provides the DataTemplate that should be used in the given context
    /// </summary>
    /// <param name="context">Widget context</param>
    /// <returns>DataTemplate for the widget</returns>
    public DataTemplate? Provide(ViewModel context);
}