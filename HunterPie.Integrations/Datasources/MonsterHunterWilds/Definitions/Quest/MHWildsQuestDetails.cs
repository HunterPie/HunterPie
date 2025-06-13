using HunterPie.Core.Game.Entity.Game.Quest;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHWildsQuestDetails
{
    [FieldOffset(0x98)] public int Type;
    [FieldOffset(0xA6)] public byte Level;

    public QuestType ToQuestType()
    {
        return Type switch
        {
            0 or 5 => QuestType.Hunt,
            1 => QuestType.Slay,
            2 => QuestType.Capture,
            6 or 7 => QuestType.Special,
            _ => QuestType.Delivery
        };
    }

    public QuestLevel ToQuestLevel()
    {
        return Level switch
        {
            >= 4 => QuestLevel.HighRank,
            _ => QuestLevel.LowRank
        };
    }
}