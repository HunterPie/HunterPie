using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account.ViewModels;
using HunterPie.Features.Account.Views;
using HunterPie.Features.Extensions.ViewModels;
using HunterPie.Features.Extensions.Views;
using HunterPie.Features.Patches.ViewModels;
using HunterPie.Features.Patches.Views;
using HunterPie.Features.Settings.ViewModels;
using HunterPie.Features.Settings.Views;
using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.Features.Statistics.Details.Views;
using HunterPie.Features.Statistics.ViewModels;
using HunterPie.Features.Statistics.Views;
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
using HunterPie.UI.Navigation.Service;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationTemplatesInitializer(
    INavigationRegistry registry
) : IInitializer
{
    public async Task Init()
    {
        registry
            .Bind<MainActivity, MainActivityViewModel>()
            .Bind<HomeActivity, HomeViewModel>()
            .Bind<AccountSignFlowActivity, AccountSignFlowViewModel>();

        registry
            .Bind<ConsoleActivity, ConsoleViewModel>()
            .Bind<SettingsActivity, SettingsViewModel>()
            .Bind<PatchesActivity, PatchesViewModel>()
            .Bind<AccountPreferencesActivity, AccountPreferencesViewModel>()
            .Bind<AbnormalityWidgetSettingsActivity, AbnormalityWidgetSettingsViewModel>()
            .Bind<QuestStatisticsSummariesActivity, QuestStatisticsSummariesViewModel>()
            .Bind<QuestDetailsActivity, QuestDetailsViewModel>()
            .Bind<MonsterConfigurationsActivity, MonsterConfigurationsViewModel>()
            .Bind<ThemeHomeActivity, ThemeHomeViewModel>();

    }
}