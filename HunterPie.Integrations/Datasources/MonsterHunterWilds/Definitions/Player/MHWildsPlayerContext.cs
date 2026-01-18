using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsPlayerContext
{
    [FieldOffset(0x10)] public MHWildsVector3 Position;

    /// <summary>
    /// <see cref="NamePointer"/> is of type <see cref="MHWildsString"/>
    /// </summary>
    [FieldOffset(0x38)] public nint NamePointer;

    [FieldOffset(0x40)] public nint NetworkInfo;

    [FieldOffset(0x64)] public int StageId;

}