using HunterPie.Domain.Interfaces;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationInitializer : IInitializer
{
    private readonly MainBodyController _mainBodyController;
    private readonly MainController _mainController;
    private readonly MainBodyViewModel _mainBodyViewModel;

    public NavigationInitializer(
        MainBodyController mainBodyController,
        MainController mainController,
        MainBodyViewModel mainBodyViewModel)
    {
        _mainBodyController = mainBodyController;
        _mainController = mainController;
        _mainBodyViewModel = mainBodyViewModel;
    }

    public Task Init()
    {
        Navigator.SetNavigators(
            body: _mainBodyController,
            app: _mainController
        );

        Navigator.Body.Navigate<HomeViewModel>();
        Navigator.App.Navigate(
            viewModel: _mainBodyViewModel
        );

        return Task.CompletedTask;
    }
}