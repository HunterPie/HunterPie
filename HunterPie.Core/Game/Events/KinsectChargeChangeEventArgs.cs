using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;

public class KinsectChargeChangeEventArgs(IInsectGlaive source) : EventArgs
{
    public KinsectChargeType Type { get; } = source.ChargeType;
    public float Timer { get; } = source.Charge;
}