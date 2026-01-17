using HunterPie.Core.Game.Entity.Player.Classes;
using System;

namespace HunterPie.Core.Game.Events;

public class WeaponChangeEventArgs(IWeapon oldWeapon, IWeapon newWeapon) : EventArgs
{
    public IWeapon OldWeapon { get; } = oldWeapon;
    public IWeapon NewWeapon { get; } = newWeapon;
}