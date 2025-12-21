using HunterPie.Core.Domain.Memory.Types;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Game;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsAilment
{
    [FieldOffset(0x20)] public Ref<MHWildsBuildUp> BuildUpPointer;
    [FieldOffset(0x48)] public MHWildsTimer Timer;
    [FieldOffset(0x58)] public int IsActive;
    [FieldOffset(0xB4)] public int Id;
}