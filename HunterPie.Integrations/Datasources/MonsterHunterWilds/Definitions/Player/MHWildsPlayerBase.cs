using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPlayerBase
{
    [FieldOffset(0x10)] public nint BasePointer;
    [FieldOffset(0x4C)] public int State;

    public bool IsReady()
    {
        return State == 6;
    }
}