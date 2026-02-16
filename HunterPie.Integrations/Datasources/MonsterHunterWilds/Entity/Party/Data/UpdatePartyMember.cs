using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party.Data;

public struct UpdatePartyMember
{
    public required bool IsValid;
    public required nint Id;
    public required string Name;
    public required Weapon Weapon;
    public required bool IsMyself;
    public required int Index;
    public required float Damage;
    public required int HunterRank;
    public bool IsNpc;
    public UpdatePlayerStatus Status;
}