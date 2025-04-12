using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Abnormality;

[StructLayout(LayoutKind.Explicit)]
public struct SkillAbnormality
{
    [FieldOffset(0x10)] public float Timer;
    [FieldOffset(0x14)] public float MaxTimer;
}