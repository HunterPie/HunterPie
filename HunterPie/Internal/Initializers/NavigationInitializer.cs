using HunterPie.Domain.Interfaces;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationInitializer : IInitializer
{
    private readonly MainBodyNavigator _mainBodyNavigator;
    private readonly MainNavigator _mainNavigator;
    private readonly MainBodyViewModel _mainBodyViewModel;

    public NavigationInitializer(
        MainBodyNavigator mainBodyNavigator,
        MainNavigator mainNavigator,
        MainBodyViewModel mainBodyViewModel)
    {
        _mainBodyNavigator = mainBodyNavigator;
        _mainNavigator = mainNavigator;
        _mainBodyViewModel = mainBodyViewModel;
    }

    public Task Init()
    {
        Navigator.SetNavigators(
            body: _mainBodyNavigator,
            app: _mainNavigator
        );

        Navigator.Body.Navigate<HomeViewModel>();
        Navigator.App.Navigate(
            viewModel: _mainBodyViewModel
        );

        return Task.CompletedTask;
    }
}