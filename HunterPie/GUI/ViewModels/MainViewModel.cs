using HunterPie.UI.Architecture;

namespace HunterPie.GUI.ViewModels;

internal class MainViewModel : ViewModel
{
    private bool _isNotificationsOpen;
    private bool _shouldShowPromo;

    public bool IsNotificationsOpen { get => _isNotificationsOpen; set => SetValue(ref _isNotificationsOpen, value); }
    public bool ShouldShowPromo { get => _shouldShowPromo; set => SetValue(ref _shouldShowPromo, value); }

}
