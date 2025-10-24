using HunterPie.Core.Domain.Memory.Types;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsSpecializedToolContext
{
    [FieldOffset(0x10)] public Ref<MHWildsEncryptedFloat> Timer;
    [FieldOffset(0x28)] public Ref<MHWildsEncryptedFloat> MaxTimer;

    [FieldOffset(0x18)] public Ref<MHWildsEncryptedFloat> Cooldown;
    [FieldOffset(0x20)] public Ref<MHWildsEncryptedFloat> MaxCooldown;

    [FieldOffset(0x33)]
    [MarshalAs(UnmanagedType.I1)] public bool InUse;
}