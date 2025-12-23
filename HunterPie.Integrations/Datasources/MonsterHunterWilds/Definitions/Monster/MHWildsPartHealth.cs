using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPartHealth
{
    /// <summary>
    /// <see cref="MaxHealthPointer"/> is of type <see cref="MHWildsEncryptedFloat"/>
    /// </summary>
    [FieldOffset(0x10)] public nint MaxHealthPointer;

    /// <summary>
    /// <see cref="HealthPointer"/> is of type <see cref="MHWildsEncryptedFloat"/>
    /// </summary>
    [FieldOffset(0x28)] public nint HealthPointer;

    [FieldOffset(0x58)] public int Count;

    [FieldOffset(0x5C)]
    [MarshalAs(UnmanagedType.I1)] public bool IsEnabled;
}