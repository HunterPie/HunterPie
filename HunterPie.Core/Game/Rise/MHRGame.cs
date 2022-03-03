using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise
{
    public class MHRGame : Scannable, IGame, IEventDispatcher
    {
        const uint MAXIMUM_MONSTER_ARRAY_SIZE = 5;

        // TODO: Could probably turn this into a bit mask with 256 bits
        private HashSet<int> MonsterAreas = new() { 5, 201, 202, 203, 204, 205, 207, 209, 210, 211};

        public IPlayer Player { get; }
        public List<IMonster> Monsters { get; } = new();

        Dictionary<long, IMonster> monsters = new();

        public event EventHandler<IMonster> OnMonsterSpawn;
        public event EventHandler<IMonster> OnMonsterDespawn;

        public MHRGame(IProcessManager process) : base(process)
        {
            Player = new MHRPlayer(process);

            ScanManager.Add(
                this,
                Player as Scannable
            );
        }

        [ScannableMethod]
        private void ScanMonstersArray()
        {
            // Only scans for monsters in hunting areas
            if (!MonsterAreas.Contains(Player.StageId))
            {
                if (monsters.Keys.Count > 0)
                    foreach (long mAddress in monsters.Keys)
                        HandleMonsterDespawn(mAddress);

                return;
            }

            long address = _process.Memory.Read(
                AddressMap.GetAbsolute("MONSTERS_ADDRESS"),
                AddressMap.Get<int[]>("MONSTER_LIST_OFFSETS")
            );

            uint monsterArraySize = _process.Memory.Read<uint>(address - 0x8);
            HashSet<long> monsterAddresses = _process.Memory.Read<long>(address + 0x20, Math.Max(MAXIMUM_MONSTER_ARRAY_SIZE, monsterArraySize))
                .ToHashSet();

            long[] toDespawn = monsters.Keys.Where(address => !monsterAddresses.Contains(address))
                .ToArray();

            foreach (long mAddress in toDespawn)
                HandleMonsterDespawn(mAddress);

            long[] toSpawn = monsterAddresses.Where(address => !monsters.ContainsKey(address))
                .ToArray();

            foreach (long mAddress in toSpawn)
                HandleMonsterSpawn(mAddress);

        }

        private void HandleMonsterSpawn(long monsterAddress)
        {
            if (monsterAddress == 0 || monsters.ContainsKey(monsterAddress))
                return;

            IMonster monster = new MHRMonster(_process, monsterAddress);
            monsters.Add(monsterAddress, monster);
            Monsters.Add(monster);
            ScanManager.Add(monster as Scannable);

            this.Dispatch(OnMonsterSpawn, monster);
        }

        private void HandleMonsterDespawn(long address)
        {
            IMonster monster = monsters[address]; 
            monsters.Remove(address);
            Monsters.Remove(monster);
            ScanManager.Remove(monster as Scannable);

            this.Dispatch(OnMonsterDespawn, monster);
        }
    }
}
