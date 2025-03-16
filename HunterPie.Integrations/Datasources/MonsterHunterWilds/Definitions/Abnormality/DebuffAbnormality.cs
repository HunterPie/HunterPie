using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Abnormality;

[StructLayout(LayoutKind.Explicit)]
public struct DebuffAbnormality
{
    [FieldOffset(0x18)] public float BuildUp;
}