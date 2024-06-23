using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay.Monster;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.UI.Architecture.Adapter;
using HunterPie.UI.Controls.Settings.Monsters.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Controls.Settings.Monsters.Builders;

#nullable enable
public static class MonsterPartsViewModelBuilder
{
    private static readonly ObservableCollection<MonsterConfigurationViewModel> EmptyCached = new();

    public static ObservableCollection<MonsterConfigurationViewModel> Build(
        GameProcess game,
        ICollection<MonsterConfiguration> configurations
    )
    {
        GameType? gameType = MapFactory.Map<GameProcess, GameType?>(game);

        if (gameType is not { })
            return EmptyCached;

        var monsterConfigurations = configurations.ToDictionary(it => it.Id);

        IEnumerable<MonsterConfigurationViewModel> viewModels = MonsterRepository.FindAllBy(gameType.Value)
            .Select(it =>
            {
                MonsterConfiguration? configuration = monsterConfigurations.GetValueOrDefault(it.Id);
                var partConfigurations = configuration?.Parts.ToDictionary(part => part.Id);

                IEnumerable<MonsterPartConfiguration> parts = it.Parts.Select(part =>
                {
                    MonsterPartConfiguration? partConfiguration = partConfigurations?.GetValueOrDefault(part.Id);

                    return new MonsterPartConfiguration
                    {
                        Id = part.Id,
                        StringId = part.String,
                        IsEnabled = partConfiguration?.IsEnabled ?? new Observable<bool>(true)
                    };
                });

                return new MonsterConfigurationViewModel
                {
                    Name = MonsterNameAdapter.From(gameType.Value, it.Id),
                    Configuration = new MonsterConfiguration
                    {
                        Id = it.Id,
                        Parts = parts.ToObservableCollection(),
                        Ailments = new ObservableCollection<MonsterAilmentConfiguration>()
                    },
                    IsOverriding = configuration is not null
                };
            });

        return viewModels.ToObservableCollection();
    }
}