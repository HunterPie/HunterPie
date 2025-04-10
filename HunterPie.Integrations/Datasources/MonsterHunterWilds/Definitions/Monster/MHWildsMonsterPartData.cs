using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsMonsterPartData
{
    [FieldOffset(0x20)] public nint SeverablePartsPointer;
    [FieldOffset(0x28)] public nint BreakablePartsAssociationPointer;
    [FieldOffset(0x30)] public nint SeverablePartsAssociationPointer;
    [FieldOffset(0x50)] public nint PartsPointer;
}