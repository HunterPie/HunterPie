using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Monster;

[StructLayout(LayoutKind.Explicit)]
internal struct MHRiseMonsterMoveContext
{
    [FieldOffset(0x40)] public MHRiseVector3 Position;
}