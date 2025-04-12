using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsTargetDamage
{
    [FieldOffset(0x10)] public float Damage;
    [FieldOffset(0x18)] public int TargetKey;
}