using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.Features.Statistics.Models;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Statistics.Details.Builders;

internal class QuestDetailsViewModelBuilder(MonsterDetailsViewModelBuilder monsterDetailsViewModelBuilder)
{
    private readonly MonsterDetailsViewModelBuilder _monsterDetailsViewModelBuilder = monsterDetailsViewModelBuilder;

    public async Task<QuestDetailsViewModel> From(HuntStatisticsModel model)
    {
        var quest = new QuestDetailsViewModel();

        Task<MonsterDetailsViewModel>[] monsterTasks = model.Monsters
            .OrderByDescending(it => it.Enrage.Activations.Length)
            .Select(it => _monsterDetailsViewModelBuilder.Build(model, it))
            .ToArray();

        MonsterDetailsViewModel[] monsters = await Task.WhenAll(monsterTasks);

        foreach (MonsterDetailsViewModel monster in monsters)
            quest.Monsters.Add(monster);

        return quest;
    }
}