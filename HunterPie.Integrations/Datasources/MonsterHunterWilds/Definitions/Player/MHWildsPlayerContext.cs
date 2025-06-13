using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPlayerContext
{
    /// <summary>
    /// <see cref="NamePointer"/> is of type <see cref="MHWildsString"/>
    /// </summary>
    [FieldOffset(0x38)] public nint NamePointer;

    [FieldOffset(0x64)] public int StageId;
}