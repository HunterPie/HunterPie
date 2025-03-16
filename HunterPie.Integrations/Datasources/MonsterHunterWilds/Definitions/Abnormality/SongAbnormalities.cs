using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Abnormality;

[StructLayout(LayoutKind.Explicit)]
public struct SongAbnormalities
{
    [FieldOffset(0x10)] public nint TimersPointer;
    [FieldOffset(0x18)] public nint MaxTimersPointer;
}