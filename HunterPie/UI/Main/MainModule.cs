using HunterPie.DI;
using HunterPie.DI.Modules;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;

namespace HunterPie.UI.Main;

internal class MainModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<MainBodyViewModel>()
            .WithSingle<MainBodyController>()
            .WithSingle<MainController>()
            .WithSingle<MainViewModel>()
            .WithSingle(() =>
                new MainView
                {
                    DataContext = registry.Get<MainViewModel>()
                }
            );
    }
}