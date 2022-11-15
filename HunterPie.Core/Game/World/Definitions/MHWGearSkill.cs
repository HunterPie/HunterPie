using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.World.Definitions;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct MHWGearSkill
{
    public long Ref;
    public byte LevelGear;
    public byte LevelMantle;
}
