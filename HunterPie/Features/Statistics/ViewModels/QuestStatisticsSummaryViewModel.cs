using HunterPie.Core.Architecture.Collections;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game.Assets;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Utils;
using HunterPie.DI;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using HunterPie.UI.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameType = HunterPie.Core.Client.Configuration.Enums.GameType;

namespace HunterPie.Features.Statistics.ViewModels;

internal class QuestStatisticsSummaryViewModel : ViewModel
{
    public required string UploadId { get; init; }
    public string QuestName { get; set => SetValue(ref field, value); }
    public QuestLevel? QuestLevel { get; set => SetValue(ref field, value); }
    public int? Stars { get; set => SetValue(ref field, value); }
    public string? QuestType { get; set => SetValue(ref field, value); }
    public int Deaths { get; set => SetValue(ref field, value); }
    public int MaxDeaths { get; set => SetValue(ref field, value); }
    public TimeSpan? QuestTime { get; set => SetValue(ref field, value); }
    public GameType GameType { get; set => SetValue(ref field, value); }
    public DateTime UploadedAt { get; set => SetValue(ref field, value); }

    public ObservableCollectionRange<MonsterSummaryViewModel> Monsters { get; init; } = new();

    public static async Task<QuestStatisticsSummaryViewModel> CreateAsync(
        IDependencyRegistry registry,
        PoogieQuestSummaryModel model
    )
    {
        ILocalizationRepository localizationRepository = registry.Get<ILocalizationRepository>();
        GameType game = model.GameType.ToEntity();

        List<MonsterSummaryViewModel> monsterVms =
            model.Monsters.Select(async it =>
            {
                return await MonsterSummaryViewModel.CreateAsync(
                    iconResolver: registry.Get<IMonsterIconResolver>(),
                    game: game,
                    model: it
                );
            })
            .ToAsyncEnumerable()
            .Collect();

        var summary = new QuestStatisticsSummaryViewModel
        {
            UploadId = model.Id,
            GameType = model.GameType.ToEntity(),
            UploadedAt = model.CreatedAt.ToLocalTime(),
            QuestTime = model.ElapsedTime,
        };

        summary.Monsters.Replace(monsterVms);

        if (model.QuestDetails is not { } details)
            return summary;

        summary.QuestName = localizationRepository.GetQuestNameBy(game, details.Id);
        summary.Deaths = details.Deaths;
        summary.MaxDeaths = details.MaxDeaths;
        summary.QuestType = localizationRepository.FindByEnum(details.Type).String;
        summary.QuestLevel = details.Level;
        summary.Stars = details.Stars;

        return summary;
    }
}