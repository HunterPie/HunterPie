using HunterPie.UI.Architecture;

namespace HunterPie.Features.Theme.ViewModels;

internal class ThemeHomeTabViewModel : ViewModel
{
    private string _title = string.Empty;
    public string Title { get => _title; set => SetValue(ref _title, value); }
}