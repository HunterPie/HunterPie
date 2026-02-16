using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Statistics.Details.Builders;
using HunterPie.Features.Statistics.Services;
using HunterPie.Features.Statistics.ViewModels;

namespace HunterPie.Features.Statistics;

internal class StatisticsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<QuestTrackerService>()
            .WithFactory<QuestStatisticsSummariesViewModel>()
            .WithSingle<QuestDetailsViewModelBuilder>()
            .WithSingle<MonsterDetailsViewModelBuilder>();
    }
}