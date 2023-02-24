using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;

public class PartyMemberDetailsViewModel : ViewModel
{
    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private Weapon _weapon;
    public Weapon Weapon { get => _weapon; set => SetValue(ref _weapon, value); }

    public Series Damages { get; init; }

    public ObservableCollection<AbnormalityDetailsViewModel> Abnormalities { get; } = new();
}