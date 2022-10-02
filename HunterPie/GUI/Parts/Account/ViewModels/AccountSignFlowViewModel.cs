using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Account.ViewModels;

#nullable enable
public class AccountSignFlowViewModel : ViewModel
{
    private int _selectedIndex;

    public int SelectedIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }
}
#nullable restore