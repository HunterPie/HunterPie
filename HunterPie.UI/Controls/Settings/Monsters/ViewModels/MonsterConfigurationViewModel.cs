using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Adapter;

namespace HunterPie.UI.Controls.Settings.Monsters.ViewModels;

#nullable enable
public class MonsterConfigurationViewModel : ViewModel
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
        Icon = await MonsterIconAdapter.UriFrom(GameType, Configuration.Id);
    }
}