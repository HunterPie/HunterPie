using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHRCharacterData
{
    [FieldOffset(0x18)]
    public long NamePointer;

    [FieldOffset(0x40)]
    public int HighRank;

    [FieldOffset(0x98)]
    public int MasterRank;
}
