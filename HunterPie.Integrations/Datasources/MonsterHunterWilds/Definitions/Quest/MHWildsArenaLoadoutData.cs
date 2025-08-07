using HunterPie.Core.Domain.Memory.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsArenaLoadoutData
{
    [FieldOffset(0x10)] public Ref<MHWildsArenaLoadoutEquipmentData> Equipment;
}