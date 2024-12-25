using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;

namespace HunterPie.GUI.Parts.Account.ViewModels;

public class AccountSignFlowViewModel : ViewModel
{
    private int _selectedIndex;
    public int SelectedIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }

    public void NavigateBack()
    {
        Navigator.App.Navigate<MainBodyViewModel>();
    }
}
