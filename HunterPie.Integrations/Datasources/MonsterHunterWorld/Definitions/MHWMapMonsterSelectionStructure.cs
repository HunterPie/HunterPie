using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

[StructLayout(LayoutKind.Explicit)]
public struct MHWMapMonsterSelectionStructure
{
    [FieldOffset(0x128)] public long MapInsectsRef;
    [FieldOffset(0x148)] public long SelectedMonster;
    [FieldOffset(0x160)] public long GuiRadarRef;
}