using HunterPie.UI.Architecture;

namespace HunterPie.UI.Navigation;

#nullable enable
public static class Navigator
{
    private static INavigator? _navigator;

    internal static void SetNavigator(INavigator navigator)
    {
        _navigator = navigator;
    }

    public static void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        _navigator?.Navigate(viewModel);
    }
}