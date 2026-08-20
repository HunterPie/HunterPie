using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game.Assets;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System.Threading.Tasks;

namespace HunterPie.Features.Statistics.ViewModels;

internal class MonsterSummaryViewModel : ViewModel
{
    public GameType GameType { get; set => SetValue(ref field, value); }
    public int Id { get; set => SetValue(ref field, value); }
    public string? Icon { get; set => SetValue(ref field, value); }
    public bool IsTarget { get; set => SetValue(ref field, value); }
    public MonsterHuntType? HuntType { get; set => SetValue(ref field, value); }
    public VariantType Variant { get; set => SetValue(ref field, value); }

    public static async Task<MonsterSummaryViewModel> CreateAsync(
        IMonsterIconResolver iconResolver,
        GameType game,
        PoogieMonsterSummaryModel model
    )
    {
        return new MonsterSummaryViewModel
        {
            GameType = game,
            Id = model.Id,
            Icon = await iconResolver.Get(game, model.Id),
            IsTarget = model.IsTarget,
            Variant = (VariantType?)model.Variant ?? VariantType.Normal
        };
    }
}