using HunterPie.Features.Account;
using HunterPie.GUI.Parts.Statistics.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System;

namespace HunterPie.Domain.Sidebar.Elements;

internal class QuestStatisticsSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type Type => typeof(QuestStatisticsSummariesViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='QUEST_STATISTICS_STRING']";

    public string Icon => "ICON_TRAP";

    private bool _isAvailable;
    public bool IsAvailable { get => _isAvailable; private set => SetValue(ref _isAvailable, value); }

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public QuestStatisticsSideBarViewModel()
    {
        AccountManager.OnSignIn += (_, __) => IsAvailable = true;
        AccountManager.OnSessionStart += (_, __) => IsAvailable = true;
        AccountManager.OnSignOut += (_, __) => IsAvailable = false;
    }

    public void Execute()
    {
        var viewModel = new QuestStatisticsSummariesViewModel();

        Navigator.Body.Navigate(viewModel);
    }
}