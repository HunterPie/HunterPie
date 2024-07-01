using HunterPie.Core.Architecture;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MonsterAilmentConfiguration
{
    public required string Id { get; init; }
    public Observable<bool> IsEnabled = true;
}