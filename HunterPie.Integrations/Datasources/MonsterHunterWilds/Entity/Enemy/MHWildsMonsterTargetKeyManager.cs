using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;
using System.Collections.Frozen;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public class MHWildsMonsterTargetKeyManager
{
    private FrozenSet<int>? _questTargetKeys;
    private readonly HashSet<int> _monsterTargetKeys = new();

    public bool HasQuestTargets()
    {
        return _questTargetKeys is not null;
    }

    public void SetQuestTargets(MHWildsTargetKey[] keys)
    {
        lock (this)
            _questTargetKeys = keys.Select(it => it.Key).ToFrozenSet();
    }

    public void ClearQuestTargets()
    {
        lock (this)
            _questTargetKeys = null;
    }

    public void AddMonster(int key)
    {
        lock (this)
            _monsterTargetKeys.Add(key);
    }

    public void ClearMonsters()
    {
        lock (this)
            _monsterTargetKeys.Clear();
    }

    public bool IsMonster(int targetKey)
    {
        lock (this)
            return _monsterTargetKeys.Contains(targetKey);
    }

    public bool IsQuestTarget(int targetKey)
    {
        lock (this)
            return _questTargetKeys?.Contains(targetKey) == true;
    }
}