using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.Entity.Player.Classes;

public interface IInsectGlaive : IMeleeWeapon
{
    public KinsectBuff PrimaryExtract { get; }
    public KinsectBuff SecondaryExtract { get; }
    public float AttackTimer { get; }
    public float SpeedTimer { get; }
    public float DefenseTimer { get; }

    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnPrimaryExtractChange;
    public event EventHandler<InsectGlaiveExtractChangeEventArgs> OnSecondaryExtractChange;
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnAttackTimerChange;
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnSpeedTimerChange;
    public event EventHandler<InsectGlaiveBuffTimerChangeEventArgs> OnDefenseTimerChange;
}
