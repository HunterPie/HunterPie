using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRMonsterAilmentStructure
{
    [FieldOffset(0x18)] public long CounterPtr;
    [FieldOffset(0x44)] public float MaxTimer;
    [FieldOffset(0x48)] public float Timer;
    [FieldOffset(0x68)] public long BuildUpPtr;
    [FieldOffset(0x78)] public long MaxBuildUpPtr;
}