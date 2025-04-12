using System;

namespace HunterPie.Core.Game.Entity.Enemy;

[Flags]
public enum VariantType
{
    Normal = 1 << 0,
    Frenzy = 1 << 1,
    Tempered = 1 << 2,
    ArchTempered = 1 << 3,
}