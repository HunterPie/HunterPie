using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsQuestInformation
{
    [FieldOffset(0x18)] public nint DetailsPointer;
    [FieldOffset(0x38)] public int Id;
    [FieldOffset(0x3C)] public int Origin;
}