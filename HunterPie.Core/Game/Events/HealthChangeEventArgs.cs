using HunterPie.Core.Game.Client;

namespace HunterPie.Core.Game.Events;
public class HealthChangeEventArgs
{

    public double Health { get; }
    public double MaxHealth { get; }
    public double RecoverableHealth { get; }
    public double MaxPossibleHealth { get; }

    public HealthChangeEventArgs(IPlayer player)
    {
        Health = player.Health;
        MaxHealth = player.MaxHealth;
        RecoverableHealth = player.RecoverableHealth;
        MaxPossibleHealth = player.MaxPossibleHealth;
    }
}
