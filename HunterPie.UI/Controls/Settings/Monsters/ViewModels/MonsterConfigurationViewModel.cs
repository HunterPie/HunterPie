using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationViewModel : ViewModel
{
    public required string Name { get; init; }

    public required MonsterConfiguration Configuration { get; init; }

    private bool _isEditing;
    public bool IsEditing { get => _isEditing; set => SetValue(ref _isEditing, value); }

    private bool _isOverriding;
    public bool IsOverriding { get => _isOverriding; set => SetValue(ref _isOverriding, value); }

    private bool _isMatch = true;
    public bool IsMatch { get => _isMatch; set => SetValue(ref _isMatch, value); }
}
