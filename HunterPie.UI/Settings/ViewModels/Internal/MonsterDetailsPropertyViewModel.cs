using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Domain.Enums;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings.ViewModels.Internal;

public class MonsterDetailsPropertyViewModel : ConfigurationPropertyViewModel
{
    public required ObservableCollection<MonsterConfiguration> Configurations { get; init; }
    public required GameProcess Game { get; init; }
}