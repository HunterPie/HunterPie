using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Statistics.ViewModels;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Entity;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Features.Statistics.Navigation;

internal class QuestStatisticsNavigationHandler(
    IAccountUseCase accountUseCase,
    IBodyNavigator navigator,
    ILocalizationRepository localizationRepository,
    IDependencyRegistry dependencies
) : NavigationHandler<QuestStatisticsSummariesViewModel>(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='QUEST_STATISTICS_STRING']"),
    icon: Resources.Icon("ICON_TRAP")
)
{

    public override async Task InitializeAsync()
    {
        State = SideBarButtonState.Loading;
        Subscribe();

        UserAccount? account = await accountUseCase.GetAsync();
        State = account switch
        {
            { } => SideBarButtonState.Enabled,
            _ => SideBarButtonState.Disabled
        };
    }

    public override Task ExecuteAsync()
    {
        QuestStatisticsSummariesViewModel viewModel = dependencies.Get<QuestStatisticsSummariesViewModel>();

        navigator.Navigate(viewModel);

        return Task.CompletedTask;
    }

    private void Subscribe()
    {
        accountUseCase.SignIn += (_, _) => State = SideBarButtonState.Enabled;
        accountUseCase.SessionStart += (_, _) => State = SideBarButtonState.Enabled;
        accountUseCase.SignOut += (_, _) => State = SideBarButtonState.Disabled;
    }
}