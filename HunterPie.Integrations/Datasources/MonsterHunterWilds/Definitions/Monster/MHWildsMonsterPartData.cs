using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterPartData
{
    [FieldOffset(0x18)] public nint SeverablePartsPointer;
    [FieldOffset(0x20)] public nint BreakablePartsAssociationPointer;
    [FieldOffset(0x28)] public nint SeverablePartsAssociationPointer;
    [FieldOffset(0x78)] public nint PartsPointer;
}