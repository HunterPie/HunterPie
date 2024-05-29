using System.Collections.ObjectModel;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MonsterConfiguration
{
    public required string Id { get; init; }
    public required ObservableCollection<MonsterPartConfiguration> Parts { get; init; }
    public required ObservableCollection<MonsterAilmentConfiguration> Ailments { get; init; }
}