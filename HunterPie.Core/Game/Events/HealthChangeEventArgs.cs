using HunterPie.Core.Game.Entity.Player.Vitals;

namespace HunterPie.Core.Game.Events;

public class HealthChangeEventArgs(IHealthComponent component)
{

    public double Health { get; } = component.Current;
    public double MaxHealth { get; } = component.Max;
    public double RecoverableHealth { get; } = component.RecoverableHealth;
    public double MaxPossibleHealth { get; } = component.MaxPossibleHealth;
    public double Heal { get; } = component.Heal;
}