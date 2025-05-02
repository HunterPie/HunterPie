using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.World;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMoon
{
    [FieldOffset(0x10)] public MHWildsEncryptedInteger Phase;
    [FieldOffset(0x29)]
    [MarshalAs(UnmanagedType.I1)] public bool IsValid;
}