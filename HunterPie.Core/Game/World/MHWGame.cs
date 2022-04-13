using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.World
{
    public class MHWGame : Scannable, IGame, IEventDispatcher
    {
        private readonly MHWPlayer _player;
        private readonly Dictionary<long, IMonster> _monsters = new();
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
                long monsterEmPtr = _process.Memory.Read<long>(next + 0x2A0);
                string monsterEm = _process.Memory.Read(monsterEmPtr + 0x0C, 64);

                isBigMonster = monsterEmPtr != 0 
                    && monsterEm.StartsWith("em\\em")
                    && !monsterEm.StartsWith("em\\ems");

                if (!isBigMonster)
                    break;

                monsterAddresses.Add(next);

                string em = monsterEm.Split('\\')
                    .ElementAtOrDefault(1);

                HandleMonsterSpawn(next, em);

                next = _process.Memory.Read<long>(next - 0x30) + 0x40;
            } while (isBigMonster);

            long[] toDespawn = _monsters.Keys.Where(address => !monsterAddresses.Contains(address))
                .ToArray();

            foreach (long monsterAddress in toDespawn)
                HandleMonsterDespawn(monsterAddress);
        }

        private void HandleMonsterSpawn(long address, string em)
        {
            if (_monsters.ContainsKey(address))
                return;

            IMonster monster = new MHWMonster(_process, address, em);
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
