using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.World;

[StructLayout(LayoutKind.Explicit)]
public struct WorldTime
{
    [FieldOffset(0x78)] public float Current;
}