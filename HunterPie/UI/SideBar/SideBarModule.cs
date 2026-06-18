using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Settings.Navigation;
using HunterPie.Features.Statistics.Navigation;
using HunterPie.Integrations.Patreon.Navigation;
using HunterPie.UI.SideBar.Services;
using HunterPie.UI.SideBar.ViewModels;

namespace HunterPie.UI.SideBar;

internal class SideBarModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<SideBarService>()
            .WithSingle<PatreonNavigationHandler>()
            .WithSingle<QuestStatisticsNavigationHandler>()
            .WithSingle<SettingsNavigationHandler>()
            .WithSingle<SideBarViewModel>();
    }
}