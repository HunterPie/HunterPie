using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Navigation;

namespace HunterPie.UI.Main;

internal class MainModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<NavigationProvider>()
            .WithSingle<MainPageViewModel>()
            .WithSingle<MainBodyNavigator>()
            .WithSingle<MainViewModel>()
            .WithSingle<MainNavigator>()
            .WithSingle<NavigatorController>()
            .WithSingle(static (r) =>
                new MainView(
                    localizationRepository: r.Get<ILocalizationRepository>()
                )
                {
                    DataContext = r.Get<MainViewModel>()
                }
            );
    }
}