using System.Windows;

namespace HunterPie.UI.Architecture.Navigator;

public interface INavigator
{
    public void Navigate<T>(T view, bool forceRefresh) where T : UIElement;
    public void Return();
    public bool IsInstanceOf<T>();
}