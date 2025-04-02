using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;
using System.Collections.Frozen;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public class MHWildsMonsterTargetKeyManager
{
    private FrozenSet<int>? _targetKeys;

    public void Set(MHWildsTargetKey[] keys)
    {
        lock (this)
            _targetKeys = keys.Select(it => it.Key).ToFrozenSet();
    }

    public void Clear()
    {
        lock (this)
            _targetKeys = null;
    }

    public bool Contains(int targetKey)
    {
        lock (this)
            return _targetKeys?.Contains(targetKey) == true;
    }
}