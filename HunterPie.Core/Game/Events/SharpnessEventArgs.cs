using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;

public class SharpnessEventArgs(IMeleeWeapon weapon) : EventArgs
{
    /// <summary>
    /// Current sharpness level
    /// </summary>
    public Sharpness Sharpness { get; } = weapon.Sharpness;

    /// <summary>
    /// Current sharpness value
    /// </summary>
    public int CurrentSharpness { get; } = weapon.CurrentSharpness;

    /// <summary>
    /// Current weapon sharpness thresholds
    /// </summary>
    public int[] SharpnessThresholds { get; } = weapon.SharpnessThresholds;

    /// <summary>
    /// Thresholds for the current level
    /// </summary>
    public int Threshold { get; } = weapon.Threshold;

    /// <summary>
    /// Maximum sharpness for the current level
    /// </summary>
    public int MaxSharpness { get; } = weapon.MaxSharpness;
}