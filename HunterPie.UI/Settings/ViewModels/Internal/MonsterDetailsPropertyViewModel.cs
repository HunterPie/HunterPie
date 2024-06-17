using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Domain.Enums;
using HunterPie.UI.Controls.Settings.Monsters.Builders;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using HunterPie.UI.Navigation;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

public class MonsterDetailsPropertyViewModel : ConfigurationPropertyViewModel
{
    public required ObservableCollection<MonsterConfiguration> Configurations { get; init; }
    public required GameProcess Game { get; init; }

    public void ConfigureParts()
    {
        ObservableCollection<MonsterConfigurationViewModel> monsterViewModels = MonsterPartsViewModelBuilder.Build(
            game: Game,
            configurations: Configurations
        );

        var vm = new MonsterConfigurationsViewModel(monsterViewModels);

        Navigator.Body.Navigate(vm);
    }
}