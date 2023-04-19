using HunterPie.Features.Account;
using HunterPie.GUI.Parts.Host;
using HunterPie.GUI.Parts.Statistics.Views;
using HunterPie.UI.Architecture;
using HunterPie.UI.Assets.Application;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class QuestStatisticsSideBarElementViewModel : ViewModel, ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_TRAP");
    public string Text => "Hunts";
    public bool IsActivable => true;
    public bool ShouldNotify => false;

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    public QuestStatisticsSideBarElementViewModel()
    {
        VerifyIfShouldEnable();
    }

    public void ExecuteOnClick()
    {
        QuestStatisticsSummariesView view = new();

        MainHost.SetMain(view);
    }

    private async void VerifyIfShouldEnable()
    {
        IsEnabled = await AccountManager.IsLoggedIn();

        AccountManager.OnSignOut += (_, __) => IsEnabled = false;
        AccountManager.OnSignIn += (_, __) => IsEnabled = true;
    }
}
