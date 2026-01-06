using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player;

public interface IPlayerStatus
{
    public double Affinity { get; }
    public double RawDamage { get; }
    public double ElementalDamage { get; }

    public event EventHandler<SimpleValueChangeEventArgs<double>> AffinityChanged;
    public event EventHandler<SimpleValueChangeEventArgs<double>> RawDamageChanged;
    public event EventHandler<SimpleValueChangeEventArgs<double>> ElementalDamageChanged;
}