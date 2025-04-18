using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Game;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsObjectContext
{
    [FieldOffset(0x31)]
    [MarshalAs(UnmanagedType.I1)]
    public bool IsReady;

    [FieldOffset(0x33)]
    [MarshalAs(UnmanagedType.I1)]
    public bool IsDestroyed;
}