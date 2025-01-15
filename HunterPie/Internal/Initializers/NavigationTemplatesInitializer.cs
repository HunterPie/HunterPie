using HunterPie.Domain.Interfaces;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.GUI.Parts.Account.Views;
using HunterPie.GUI.Parts.Patches.ViewModels;
using HunterPie.GUI.Parts.Patches.Views;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.GUI.Parts.Settings.Views;
using HunterPie.GUI.Parts.Statistics.Details.ViewModels;
using HunterPie.GUI.Parts.Statistics.Details.Views;
using HunterPie.GUI.Parts.Statistics.ViewModels;
using HunterPie.GUI.Parts.Statistics.Views;
using HunterPie.UI.Controls.Settings.Abnormality.ViewModels;
using HunterPie.UI.Controls.Settings.Abnormality.Views;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using HunterPie.UI.Controls.Settings.Monsters.Views;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Home.Views;
using HunterPie.UI.Logging.ViewModels;
using HunterPie.UI.Logging.Views;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationTemplatesInitializer : IInitializer
{
    public Task Init()
    {
        NavigationProvider.Bind<MainBodyView, MainBodyViewModel>();
        NavigationProvider.Bind<HomeView, HomeViewModel>();
        NavigationProvider.Bind<AccountSignFlowView, AccountSignFlowViewModel>();

        NavigationProvider.Bind<ConsoleView, ConsoleViewModel>();
        NavigationProvider.Bind<SettingsView, SettingsViewModel>();
        NavigationProvider.Bind<PatchesView, PatchesViewModel>();
        NavigationProvider.Bind<AccountPreferencesView, AccountPreferencesViewModel>();
        NavigationProvider.Bind<AbnormalityWidgetSettingsView, AbnormalityWidgetSettingsViewModel>();
        NavigationProvider.Bind<QuestStatisticsSummariesView, QuestStatisticsSummariesViewModel>();
        NavigationProvider.Bind<QuestDetailsView, QuestDetailsViewModel>();
        NavigationProvider.Bind<MonsterConfigurationsView, MonsterConfigurationsViewModel>();

        return Task.CompletedTask;
    }
}