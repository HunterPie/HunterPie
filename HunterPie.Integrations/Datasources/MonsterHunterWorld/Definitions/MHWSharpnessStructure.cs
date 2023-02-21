using HunterPie.Core.Game.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;


[StructLayout(LayoutKind.Sequential)]
public struct MHWSharpnessStructure
{
    public int Sharpness;
    public Sharpness Level;
}
