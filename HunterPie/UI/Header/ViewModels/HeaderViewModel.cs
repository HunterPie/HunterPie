using HunterPie.Core.Client;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Header.ViewModels;

internal class HeaderViewModel(AccountMenuViewModel accountMenuViewModel) : ViewModel
{
    public string Version { get; set => SetValue(ref field, value); } = $"v{ClientInfo.Version}";
    public bool IsAdmin { get; set => SetValue(ref field, value); } = ClientInfo.IsAdmin;

    public AccountMenuViewModel AccountMenuViewModel { get; } = accountMenuViewModel;
}