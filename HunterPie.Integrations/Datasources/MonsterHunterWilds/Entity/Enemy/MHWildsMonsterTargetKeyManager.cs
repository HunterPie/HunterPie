namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;

public class MHWildsMonsterTargetKeyManager
{
    private readonly Dictionary<nint, int> _targetKeys = new();

    public void Add(nint address, int targetKey)
    {
        lock (_targetKeys)
            _targetKeys.TryAdd(address, targetKey);
    }

    public void Remove(nint address)
    {
        lock (_targetKeys)
            _targetKeys.Remove(address);
    }

    public bool Contains(int targetKey)
    {
        lock (_targetKeys)
            return _targetKeys.ContainsValue(targetKey);
    }
}