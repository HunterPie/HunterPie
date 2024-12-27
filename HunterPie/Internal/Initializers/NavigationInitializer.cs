using HunterPie.Domain.Interfaces;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationInitializer : IInitializer
{
    private readonly IBodyNavigator _mainBodyNavigator;
    private readonly IAppNavigator _mainNavigator;
    private readonly MainBodyViewModel _mainBodyViewModel;

    public NavigationInitializer(
        IBodyNavigator mainBodyNavigator,
        IAppNavigator mainNavigator,
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