namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Data;

public record struct UpdatePlayerStatus(
    float RawDamage,
    float ElementalDamage,
    int Affinity
);