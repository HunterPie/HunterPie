using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Game.Assets;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

public class MonsterConfigurationViewModel(
    IMonsterIconResolver monsterIconResolver
) : ViewModel
{
    public required string Name { get; init; }

    public required GameType GameType { get; init; }
    public string? Icon { get; private set => SetValue(ref field, value); }

    public required MonsterConfiguration Configuration { get; init; }
    public bool IsEditing { get; set => SetValue(ref field, value); }
    public bool IsOverriding { get; set => SetValue(ref field, value); }
    public bool IsMatch { get; set => SetValue(ref field, value); } = true;

    public async void FetchIcon()
    {
        Icon = await monsterIconResolver.Get(GameType, Configuration.Id);
    }
}