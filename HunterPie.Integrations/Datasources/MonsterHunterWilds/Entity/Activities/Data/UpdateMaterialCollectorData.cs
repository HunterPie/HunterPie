namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities.Data;

public struct UpdateMaterialCollectorData
{
    public required MaterialRetrievalCollector Collector { get; init; }
    public required int Count { get; init; }
    public required int MaxCount { get; init; }
}