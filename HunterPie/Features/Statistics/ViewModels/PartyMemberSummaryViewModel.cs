using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Statistics.ViewModels;
public class PartyMemberSummaryViewModel : ViewModel
{
    private string _name;
    private int _damage;
    private Weapon _weapon;

    public string Name { get => _name; set => SetValue(ref _name, value); }
    public int Damage { get => _damage; set => SetValue(ref _damage, value); }
    public Weapon Weapon { get => _weapon; set => SetValue(ref _weapon, value); }
    public Series Series { get; init; }
    public ObservableCollection<AbnormalitySummaryViewModel> Abnormalities { get; } = new();
}
