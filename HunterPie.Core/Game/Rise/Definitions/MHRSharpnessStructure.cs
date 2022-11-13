using HunterPie.Core.Game.Enums;
using System.Runtime.InteropServices;

namespace HunterPie.Core.Game.Rise.Definitions;

[StructLayout(LayoutKind.Sequential)]
public struct MHRSharpnessStructure
{
    public Sharpness Level;
    public int Hits;
    public int MaxHits;
}
