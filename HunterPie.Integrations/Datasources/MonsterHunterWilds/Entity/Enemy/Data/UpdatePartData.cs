namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy.Data;

public struct UpdatePartData
{
    public required bool IsBreakable;
    public required bool IsSeverable;
    public required int BreakMultiplier;
    public required int MaxBreaks;
    public required int Breaks;
    public required float Health;
    public required float MaxHealth;
    public required int HealthResetCount;
}