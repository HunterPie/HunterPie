using HunterPie.Core.Domain.Memory.Types;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsDamageStatus
{
    [FieldOffset(0x18)] public Ref<MHWildsEncryptedFloat> CurrentRawAttack;
    [FieldOffset(0x40)] public Ref<MHWildsEncryptedFloat> CurrentElementalAttack;
}