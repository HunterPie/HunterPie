namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Data;

public readonly struct UpdatePlayerStatus
{
    public required readonly double Affinity { get; init; }
    public required readonly double RawDamage { get; init; }
    public required readonly double ElementalDamage { get; init; }
}