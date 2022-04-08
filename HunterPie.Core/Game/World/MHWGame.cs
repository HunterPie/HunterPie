using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.World.Entities;
using System;
using System.Collections.Generic;

namespace HunterPie.Core.Game.World
{
    public class MHWGame : Scannable, IGame, IEventDispatcher
    {
        private readonly MHWPlayer _player;
        private readonly Dictionary<long, IMonster> _monsters;
        private readonly IProcessManager _process;

        public event EventHandler<IMonster> OnMonsterSpawn;
        public event EventHandler<IMonster> OnMonsterDespawn;
        public event EventHandler<IGame> OnHudStateChange;

        public IPlayer Player => _player;
        public List<IMonster> Monsters { get; } = new();

        public IChat Chat => throw new NotImplementedException();

        public bool IsHudOpen => throw new NotImplementedException();

        public MHWGame(IProcessManager process)
        {
            _process = process;
            _player = new(process);

            ScanManager.Add(_player, this);
        }

        [ScannableMethod]
        private void ScanMonsterDoubleLinkedList()
        {
            long doubleLinkedListHead = _process.Memory.Read(
                AddressMap.GetAbsolute("MONSTER_ADDRESS"),
                AddressMap.Get<int[]>("MONSTER_OFFSETS")
            );

            long next = doubleLinkedListHead;
            bool isBigMonster;
            HashSet<long> monsterAddresses = new();
            do
            {
                isBigMonster = _process.Memory.Read<int>(next + 0x12284) == 0;

                if (!isBigMonster)
                    break;

                monsterAddresses.Add(next);

                next = _process.Memory.Read<long>(next - 0x30) + 0x40;
            } while (isBigMonster);
        }

        private void HandleMonsterSpawn(long address)
        {
            if (_monsters.ContainsKey(address))
                return;

            IMonster monster = new MHWMonster(address);
            _monsters.Add(address, monster);
            Monsters.Add(monster);
            ScanManager.Add(monster as Scannable);

            this.Dispatch(OnMonsterSpawn, monster);
        }

        private void HandleMonsterDespawn(long address)
        {
            IMonster monster = _monsters[address];
            _monsters.Remove(address);
            Monsters.Remove(monster);
            ScanManager.Remove(monster as Scannable);

            this.Dispatch(OnMonsterDespawn, monster);
        }
    }
}
