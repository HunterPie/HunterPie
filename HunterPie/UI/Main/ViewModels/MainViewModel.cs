using HunterPie.UI.Architecture;
using HunterPie.UI.Header.ViewModels;

namespace HunterPie.UI.Main.ViewModels;

internal class MainViewModel(HeaderViewModel headerViewModel) : ViewModel
{
    public HeaderViewModel HeaderViewModel { get; } = headerViewModel;
    public ViewModel? ContentViewModel { get; set => SetValue(ref field, value); }
}