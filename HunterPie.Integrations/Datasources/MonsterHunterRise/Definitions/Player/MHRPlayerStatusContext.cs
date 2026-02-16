using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHRPlayerStatusContext
{
    [FieldOffset(0x44)] public float RawDamage;
    [FieldOffset(0x50)] public float PrimaryElementalDamage;
    [FieldOffset(0x54)] public float SecondaryElementalDamage;
    [FieldOffset(0x68)] public int Affinity;
}