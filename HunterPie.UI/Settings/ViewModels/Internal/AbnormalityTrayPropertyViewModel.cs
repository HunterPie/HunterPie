using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

internal class AbnormalityTrayPropertyViewModel : ConfigurationPropertyViewModel
{
    public ObservableCollection<AbnormalityWidgetConfig> Trays { get; }
    public GameProcess Game { get; }

    public AbnormalityTrayPropertyViewModel(
        ObservableCollection<AbnormalityWidgetConfig> trays,
        GameProcess game
    )
    {
        Trays = trays;
        Game = game;
    }
}