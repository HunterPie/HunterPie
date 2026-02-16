using System.Numerics;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions.Types;

[StructLayout(LayoutKind.Sequential)]
public struct MHWVector3
{
    public float X;
    public float Y;
    public float Z;

    public Vector3 ToVector3() => new Vector3(X, Y, Z);
}