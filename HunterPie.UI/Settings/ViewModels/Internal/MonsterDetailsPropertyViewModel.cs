using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Controls.Settings.Monsters.Builders;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using HunterPie.UI.Navigation;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

public class MonsterDetailsPropertyViewModel : ConfigurationPropertyViewModel
{
    public required ObservableHashSet<MonsterConfiguration> Configurations { get; init; }
    public required GameProcess Game { get; init; }

    public void ConfigureParts()
    {
        ObservableCollection<MonsterConfigurationViewModel> monsterViewModels = MonsterPartsViewModelBuilder.Build(
            game: Game,
            configurations: Configurations
        );
        OverlayConfig configuration = ClientConfigHelper.GetOverlayConfigFrom(Game);

        var vm = new MonsterConfigurationsViewModel(
            configuration: configuration.BossesWidget.Details,
            elements: monsterViewModels
        );

        Navigator.Body.Navigate(vm);
    }
}