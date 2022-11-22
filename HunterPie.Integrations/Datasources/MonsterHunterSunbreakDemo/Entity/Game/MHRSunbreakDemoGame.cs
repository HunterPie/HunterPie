using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Player;

namespace HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Game;

public class MHRSunbreakDemoGame : Scannable, IGame, IEventDispatcher
{
    public const uint MaximumMonsterArraySize = 5;

    private readonly HashSet<int> _monsterAreas = new() { 5, 201, 202, 203, 204, 205, 207, 209, 210, 211, 212 };
    private readonly Dictionary<long, IMonster> _monsters = new();
    private readonly MHRSunbreakDemoPlayer _player;

    public IPlayer Player => _player;
    public List<IMonster> Monsters { get; } = new();
    public bool IsHudOpen { get; set; }

    public IChat Chat => throw new NotImplementedException();
    public float TimeElapsed => throw new NotImplementedException();
    public int MaxDeaths => throw new NotImplementedException();
    public int Deaths => throw new NotImplementedException();
    public IAbnormalityCategorizationService AbnormalityCategorizationService => throw new NotImplementedException();

    public event EventHandler<IMonster>? OnMonsterSpawn;
    public event EventHandler<IMonster>? OnMonsterDespawn;
    public event EventHandler<IGame>? OnHudStateChange;
    public event EventHandler<TimeElapsedChangeEventArgs>? OnTimeElapsedChange;
    public event EventHandler<IGame>? OnDeathCountChange;

    public MHRSunbreakDemoGame(IProcessManager process) : base(process)
    {
        _player = new(process);

        ScanManager.Add(_player, this);
    }

    [ScannableMethod]
    private void ScanMonstersArray()
    {
        // Only scans for monsters in hunting areas
        if (!_monsterAreas.Contains(Player.StageId))
        {
            if (_monsters.Keys.Count > 0)
                foreach (long mAddress in _monsters.Keys)
                    HandleMonsterDespawn(mAddress);

            return;
        }

        long address = Process.Memory.Read(
            AddressMap.GetAbsolute("MONSTERS_ADDRESS"),
            AddressMap.Get<int[]>("MONSTER_LIST_OFFSETS")
        );

        uint monsterArraySize = Process.Memory.Read<uint>(address - 0x8);
        var monsterAddresses = Process.Memory.Read<long>(address + 0x20, Math.Min(MaximumMonsterArraySize, monsterArraySize))
            .ToHashSet();

        long[] toDespawn = _monsters.Keys.Where(address => !monsterAddresses.Contains(address))
            .ToArray();

        foreach (long mAddress in toDespawn)
            HandleMonsterDespawn(mAddress);

        long[] toSpawn = monsterAddresses.Where(address => !_monsters.ContainsKey(address))
            .ToArray();

        foreach (long mAddress in toSpawn)
            HandleMonsterSpawn(mAddress);

    }

    private void HandleMonsterSpawn(long monsterAddress)
    {
        if (monsterAddress == 0 || _monsters.ContainsKey(monsterAddress))
            return;

        IMonster monster = new MHRSunbreakDemoMonster(Process, monsterAddress);
        _monsters.Add(monsterAddress, monster);
        Monsters.Add(monster);
        ScanManager.Add(monster as Scannable);

        this.Dispatch(OnMonsterSpawn, monster);
    }

    private void HandleMonsterDespawn(long address)
    {
        IMonster monster = _monsters[address];
        _ = _monsters.Remove(address);
        _ = Monsters.Remove(monster);
        ScanManager.Remove(monster as Scannable);

        this.Dispatch(OnMonsterDespawn, monster);
    }

    public void Dispose() { }
}
