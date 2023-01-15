namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;

[Flags]
public enum WirebugConditions : ulong
{
    None = 0,
    IceL = 1 << 7,
    MarionetteTypeRuby = 1UL << 34,
    MarionetteTypeGold = 1UL << 36,
    WindMantle = 1UL << 57,
}