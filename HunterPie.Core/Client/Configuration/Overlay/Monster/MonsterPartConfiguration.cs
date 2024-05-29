using HunterPie.Core.Architecture;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MonsterPartConfiguration
{
    public required string Id { get; init; }
    public Observable<bool> IsEnabled = true;
}