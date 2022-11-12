using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using System;

namespace HunterPie.Core.Game.World.Entities.Weapons;
public class MHWMeleeWeapon : IWeapon, IMeleeWeapon
{
    public Sharpness Sharpness => throw new NotImplementedException();

    public int CurrentSharpness => throw new NotImplementedException();

    public int[] SharpnessThresholds => throw new NotImplementedException();

    public Weapon Id { get; }

    public int MaxSharpness => throw new NotImplementedException();

    public int Threshold => throw new NotImplementedException();

    public event EventHandler<SharpnessEventArgs> OnSharpnessChange;
    public event EventHandler<SharpnessEventArgs> OnSharpnessLevelChange;

    public MHWMeleeWeapon(Weapon id)
    {
        Id = id;
    }
}
