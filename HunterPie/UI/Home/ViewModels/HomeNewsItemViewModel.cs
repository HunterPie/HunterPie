using HunterPie.UI.Architecture;

namespace HunterPie.UI.Home.ViewModels;

internal class HomeNewsItemViewModel : ViewModel
{
    public string Title { get; set => SetValue(ref field, value); } = string.Empty;
    public string Description { get; set => SetValue(ref field, value); } = string.Empty;
    public string? Banner { get; set => SetValue(ref field, value); } = null;
}