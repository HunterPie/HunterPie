using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationViewModel : ViewModel
{
    public required string Name { get; init; }

    public required MonsterConfiguration Configuration { get; init; }

    private bool _isOverriding;
    public bool IsOverriding { get => _isOverriding; set => SetValue(ref _isOverriding, value); }
}
