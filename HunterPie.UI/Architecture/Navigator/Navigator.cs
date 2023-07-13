using System.Windows;

namespace HunterPie.UI.Architecture.Navigator;

#nullable enable
public class Navigator
{
    private static INavigator? _instance;

    private Navigator() { }

    internal static void SetNavigator(INavigator navigator)
    {
        _instance = navigator;
    }

    public static void Navigate<T>(T element, bool forceRefresh = false) where T : UIElement =>
        _instance?.Navigate(element, forceRefresh);

    public static void Return() => _instance?.Return();

    public static bool IsInstanceOf<T>() => _instance?.IsInstanceOf<T>() ?? false;
}