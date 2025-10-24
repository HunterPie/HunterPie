using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.World;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMoonControllers
{
    [FieldOffset(0x10)] public nint Main;
    [FieldOffset(0x18)] public nint Story;
    [FieldOffset(0x20)] public nint Quest;
}