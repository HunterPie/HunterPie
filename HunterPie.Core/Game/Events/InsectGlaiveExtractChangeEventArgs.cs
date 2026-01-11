using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.Core.Game.Events;

public class InsectGlaiveExtractChangeEventArgs(KinsectBuff extract) : EventArgs
{
    public KinsectBuff Extract { get; } = extract;
}