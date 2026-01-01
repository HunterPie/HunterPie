using HunterPie.Core.Domain.Memory.Types;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsAffinityStatus
{
    [FieldOffset(0x10)] public Ref<MHWildsEncryptedFloat> WeaponAffinity;
    [FieldOffset(0x18)] public Ref<MHWildsEncryptedFloat> OriginalAffinity;
    [FieldOffset(0x20)] public Ref<MHWildsEncryptedFloat> CurrentCriticalRate;
}