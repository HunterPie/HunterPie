using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Runtime.InteropServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;

[StructLayout(LayoutKind.Explicit)]
public struct MHRQuestDataStructure
{
    /// <summary>
    /// Pointer to a normal quest structure <see cref="MHRNormalQuestDataStructure"/>
    /// </summary>
    [FieldOffset(0x10)] public long NormalQuestPointer;

    /// <summary>
    /// Pointer to an anomaly quest structure <see cref="MHRAnomalyQuestDataStructure"/>
    /// </summary>
    [FieldOffset(0x28)] public long AnomalyQuestPointer;

    public MHRQuestData? GetCurrentQuest(IProcessManager process)
    {
        if (!NormalQuestPointer.IsNullPointer())
        {
            MHRNormalQuestDataStructure normalQuest =
                process.Memory.Read<MHRNormalQuestDataStructure>(NormalQuestPointer);

            return new MHRQuestData
            {
                Id = normalQuest.Id,
                Level = normalQuest.Rank.ToQuestLevel(),
                Stars = normalQuest.Stars + 1
            };
        }

        if (!AnomalyQuestPointer.IsNullPointer())
        {
            MHRAnomalyQuestDataStructure anomalyQuest =
                process.Memory.Read<MHRAnomalyQuestDataStructure>(AnomalyQuestPointer);

            return new MHRQuestData
            {
                Id = anomalyQuest.Id,
                Level = QuestLevel.Anomaly,
                Stars = anomalyQuest.Level
            };
        }

        return null;
    }
}