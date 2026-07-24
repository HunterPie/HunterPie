using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Statistics.Details.Builders;
using HunterPie.Features.Statistics.Navigation;
using HunterPie.Features.Statistics.Services;
using HunterPie.Features.Statistics.ViewModels;

namespace HunterPie.Features.Statistics;

internal class StatisticsModule : IDependencyModule, IScopedModule
{
    void IDependencyModule.Register(IDependencyRegistry registry)
    {
        registry

            .WithFactory<QuestStatisticsSummariesViewModel>()
            .WithSingle<QuestDetailsViewModelBuilder>()
            .WithSingle<MonsterDetailsViewModelBuilder>()
            .WithSingle<QuestStatisticsNavigationHandler>();
    }

    void IScopedModule.Register(IScopedDependencyRegistry registry)
    {
        registry
            .WithSingle<QuestTrackerService>();
    }
}