using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
public struct MHRConsumableStructure
{
    public float Timer;
}

public struct MHRDebuffStructure
{
    public float Timer;
}

[StructLayout(LayoutKind.Explicit)]
public struct MHRAbnormalityStructure
{
    [FieldOffset(0)] public int Timer;
    [FieldOffset(0)] public MHRConsumableStructure Consumable;
    [FieldOffset(0)] public MHRDebuffStructure Debuff;
}