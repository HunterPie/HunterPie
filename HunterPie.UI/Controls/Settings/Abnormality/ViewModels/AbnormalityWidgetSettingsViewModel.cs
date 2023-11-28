using HunterPie.Core.Architecture;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Settings.Models;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Controls.Settings.Abnormality.ViewModels;

public class AbnormalityWidgetSettingsViewModel : Architecture.ViewModel
{
    public ObservableCollection<AbnormalityCategoryViewModel> Categories { get; }
    public ObservableHashSet<string> SelectedAbnormalities { get; }
    public ConfigurationCategory Configuration { get; }

    public AbnormalityWidgetSettingsViewModel(
        ConfigurationCategory configuration,
        ObservableCollection<AbnormalityCategoryViewModel> categories,
        ObservableHashSet<string> selectedAbnormalities
    )
    {
        Configuration = configuration;
        Categories = categories;
        SelectedAbnormalities = selectedAbnormalities;
    }

    public void ExitScreen()
    {
        Navigator.Return();
    }
}