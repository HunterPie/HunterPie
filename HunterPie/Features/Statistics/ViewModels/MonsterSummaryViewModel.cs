using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Adapter;

namespace HunterPie.Features.Statistics.ViewModels;

public class MonsterSummaryViewModel : ViewModel
{
    public GameType GameType { get; set => SetValue(ref field, value); }
    public int Id { get; set => SetValue(ref field, value); }
    public string? Icon { get; set => SetValue(ref field, value); }
    public bool IsTarget { get; set => SetValue(ref field, value); }
    public MonsterHuntType? HuntType { get; set => SetValue(ref field, value); }
    public VariantType Variant { get; set => SetValue(ref field, value); }

    public MonsterSummaryViewModel() { }

    internal MonsterSummaryViewModel(
        GameType game,
        PoogieMonsterSummaryModel model)
    {
        GameType = game;
        Id = model.Id;
        IsTarget = model.IsTarget;
        HuntType = model.HuntType;
        Variant = (VariantType?)model.Variant ?? VariantType.Normal;

        FetchMonsterIcon();
    }

    private async void FetchMonsterIcon()
    {
        Icon = await MonsterIconAdapter.UriFrom(GameType, Id);
    }
}