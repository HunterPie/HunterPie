using HunterPie.UI.Architecture;

namespace HunterPie.Features.Tools.Localization.ViewModels;

internal class LocalizableStringViewModel : ViewModel
{
    public string Id { get; set => SetValue(ref field, value); }
    public string Original { get; set => SetValue(ref field, value); }
    public string Localized { get; set => SetValue(ref field, value); }
}