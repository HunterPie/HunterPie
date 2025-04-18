using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterHealth
{
    /// <summary>
    /// <see cref="HealthPointer"/> is of type <see cref="MHWildsEncryptedFloat"/>
    /// </summary>
    [FieldOffset(0x10)] public nint HealthPointer;

    /// <summary>
    /// <see cref="MaxHealthPointer"/> is of type <see cref="MHWildsEncryptedFloat"/>
    /// </summary>
    [FieldOffset(0x18)] public nint MaxHealthPointer;
}