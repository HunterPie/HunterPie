using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;

public class ChargeBladePhialChangeEventArgs(IChargeBlade chargeBlade) : EventArgs
{
    public PhialChargeLevel Charge { get; } = chargeBlade.Charge;
    public int Phials { get; } = chargeBlade.Phials;
    public int MaxPhials { get; } = chargeBlade.MaxPhials;
}