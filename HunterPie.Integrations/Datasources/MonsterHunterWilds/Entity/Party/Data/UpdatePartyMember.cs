using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party.Data;

public struct UpdatePartyMember
{
    public required nint Id;
    public required string Name;
    public required Weapon Weapon;
    public required bool IsMyself;
    public required int Index;
    public required float Damage;
}