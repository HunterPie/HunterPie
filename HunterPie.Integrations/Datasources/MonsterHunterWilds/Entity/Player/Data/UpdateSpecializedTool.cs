using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Data;

public struct UpdateSpecializedTool
{
    public required SpecializedToolType Type;
    public required float Timer;
    public required float MaxTimer;
    public required float Cooldown;
    public required float MaxCooldown;
    public required bool IsTimerActive;
}