using HunterPie.Core.Domain.Memory.Types;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsArenaData
{
    [FieldOffset(0x20)] public Ref<MHWildsArenaMissionData> Mission;
    [FieldOffset(0x40)] public MHWildsArrayRef<MHWildsArenaLoadoutData> Loadouts;
}