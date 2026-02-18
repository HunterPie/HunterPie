using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsCurrentQuestInformation
{
    [FieldOffset(0x20)] public nint TargetKeysPointer;
    [FieldOffset(0xB8)] public MHWildsEncryptedInteger MaxDeaths;
    [FieldOffset(0xE0)] public float Timer;
    [FieldOffset(0xE4)] public float MaxTimer;
    [FieldOffset(0x108)] public int SuccessState; // 0: none, 1: leave, 2: clear, 3: failed, 4: failed
    [FieldOffset(0x114)] public int FailureState; // 0: none, 1: time up, 2: wipe, 3: killed target

    public QuestStatus ToQuestStatus()
    {
        return (SuccessState, Timer) switch
        {
            (1, _) => QuestStatus.Quit,
            (2, _) => QuestStatus.Success,
            ( >= 3, _) => QuestStatus.Fail,
            (_, > 0) => QuestStatus.InProgress,
            _ => QuestStatus.None
        };
    }
}