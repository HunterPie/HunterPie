using System.Collections.ObjectModel;

namespace HunterPie.UI.Controls.Settings.Abnormality.ViewModels;

public class AbnormalityCategoryViewModel : Architecture.ViewModel
{
    public string Name { get; set => SetValue(ref field, value); }
    public string Description { get; set => SetValue(ref field, value); }
    public string? Icon { get; set => SetValue(ref field, value); }

    public ObservableCollection<AbnormalityElementViewModel> Elements { get; init; }
}