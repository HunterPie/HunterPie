using HunterPie.UI.Architecture;
using HunterPie.UI.Main.Navigators;
using HunterPie.UI.Main.ViewModels;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountSignFlowViewModel(MainNavigator mainNavigator) : ViewModel
{
    private readonly MainNavigator _mainNavigator = mainNavigator;

    public int SelectedIndex { get; set => SetValue(ref field, value); }

    public void NavigateBack()
    {
        _mainNavigator.Navigate<MainBodyViewModel>();
    }
}