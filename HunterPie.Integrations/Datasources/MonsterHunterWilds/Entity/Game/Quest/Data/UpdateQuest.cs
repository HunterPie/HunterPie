using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Quest.Data;

public struct UpdateQuest
{
    public required MHWildsCurrentQuestInformation Information;
    public required MHWildsEncryptedInteger Deaths;
}