using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Controls.Settings.Custom;
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

    public void CreateNewTray()
    {
        Trays.Add(new AbnormalityWidgetConfig());
    }

    public void DeleteTray(AbnormalityWidgetConfig tray)
    {
        Trays.Remove(tray);
    }

    public void ConfigureTray(AbnormalityWidgetConfig tray)
    {
        var view = new AbnormalityWidgetConfigView(Game, tray);

        Navigator.Navigate(view);
    }
}