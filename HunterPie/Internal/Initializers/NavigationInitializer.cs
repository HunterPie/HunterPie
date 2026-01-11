using HunterPie.Domain.Interfaces;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationInitializer(
    IBodyNavigator mainBodyNavigator,
    IAppNavigator mainNavigator) : IInitializer
{
    private readonly IBodyNavigator _mainBodyNavigator = mainBodyNavigator;
    private readonly IAppNavigator _mainNavigator = mainNavigator;

    public Task Init()
    {
        Navigator.SetNavigators(
            body: _mainBodyNavigator,
            app: _mainNavigator
        );

        return Task.CompletedTask;
    }
}