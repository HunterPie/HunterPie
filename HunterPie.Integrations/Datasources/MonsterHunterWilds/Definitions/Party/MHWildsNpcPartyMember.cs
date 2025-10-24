using HunterPie.Core.Domain.Memory.Types;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsNpcPartyMember
{
    [FieldOffset(0x10)] public Ref<MHWildsNpcCreation> CreationParams;
    [FieldOffset(0x28)] public nint ContextPointer;
    [FieldOffset(0x38)] public int Type;
    [FieldOffset(0x44)] public int State;

    public bool IsInParty()
    {
        return State == 2;
    }

    public bool IsHandler()
    {
        return Type == 3;
    }
}