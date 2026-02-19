using HunterPie.Core.Observability.Logging;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Monster;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public class MHWildsMonsterTargetKeyManager
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly Lock _lock = new();
    private int[]? _questTargetKeys;
    private readonly HashSet<int> _monsterTargetKeys = new();

    public bool HasQuestTargets()
    {
        return _questTargetKeys is not null;
    }

    public void SetQuestTargets(MHWildsTargetKey[] keys)
    {
        bool shouldUpdate = _questTargetKeys is null || !_questTargetKeys.SequenceEqual(keys.Select(it => it.Key));

        if (!shouldUpdate)
            return;

        lock (_lock)
            _questTargetKeys = keys.Select(it => it.Key).ToArray();

        _logger.Debug($"updated quest with target keys ({string.Join(',', _questTargetKeys)})");
    }

    public void ClearQuestTargets()
    {
        lock (_lock)
            _questTargetKeys = null;

        _logger.Debug("cleared quest target keys");
    }

    public void AddMonster(int key)
    {
        lock (_lock)
            _monsterTargetKeys.Add(key);
    }

    public void ClearMonsters()
    {
        lock (_lock)
            _monsterTargetKeys.Clear();
    }

    public bool IsMonster(int targetKey)
    {
        lock (_lock)
            return _monsterTargetKeys.Contains(targetKey);
    }

    public bool IsQuestTarget(int targetKey)
    {
        lock (_lock)
            return _questTargetKeys?.Contains(targetKey) == true;
    }
}