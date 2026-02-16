using HunterPie.DI;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Statistics.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class QuestStatisticsSideBarViewModel : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator;
    private readonly IAccountUseCase _accountUseCase;

    public Type Type => typeof(QuestStatisticsSummariesViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='QUEST_STATISTICS_STRING']";

    public string Icon => "ICON_TRAP";

    public bool IsAvailable { get; private set => SetValue(ref field, value); }
    public bool IsSelected { get; set => SetValue(ref field, value); }

    public QuestStatisticsSideBarViewModel(
        IBodyNavigator bodyNavigator,
        IAccountUseCase accountUseCase)
    {
        _bodyNavigator = bodyNavigator;
        _accountUseCase = accountUseCase;

        Subscribe();
    }

    public Task ExecuteAsync()
    {
        QuestStatisticsSummariesViewModel viewModel = DependencyContainer.Get<QuestStatisticsSummariesViewModel>();

        _bodyNavigator.Navigate(viewModel);

        return Task.CompletedTask;
    }

    private void Subscribe()
    {
        _accountUseCase.SignIn += (_, _) => IsAvailable = true;
        _accountUseCase.SessionStart += (_, _) => IsAvailable = true;
        _accountUseCase.SignOut += (_, _) => IsAvailable = false;

        _accountUseCase.GetAsync()
            .ContinueWith(t => IsAvailable = t.Result is not null);
    }
}