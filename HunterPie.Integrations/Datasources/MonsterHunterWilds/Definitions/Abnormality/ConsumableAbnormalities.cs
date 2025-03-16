using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Abnormality;

public struct ConsumableAbnormalities
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xB0)]
    public byte[] Raw;
}