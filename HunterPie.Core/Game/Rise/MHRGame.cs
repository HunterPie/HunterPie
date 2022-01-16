using HunterPie.Core.Address.Map;
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
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise
{
    public class MHRGame : Scannable, IGame, IEventDispatcher
    {
        const uint MAXIMUM_MONSTER_ARRAY_SIZE = 5;

        public IPlayer Player { get; }

        public List<IMonster> Monsters { get; } = new();

        Dictionary<long, IMonster> monsters = new();

        public event EventHandler<IMonster> OnMonsterSpawn;
        public event EventHandler<IMonster> OnMonsterDespawn;

        public MHRGame(IProcessManager process) : base(process)
        {
            Player = new MHRPlayer(process);
            StartScanTask();
        }

        [ScannableMethod]
        private void ScanMonstersArray()
        {
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

            this.Dispatch(OnMonsterSpawn, monster);
        }

        private void HandleMonsterDespawn(long address)
        {
            IMonster monster = monsters[address]; 
            monsters.Remove(address);
            Monsters.Remove(monster);

            this.Dispatch(OnMonsterDespawn, monster);
        }

        private void StartScanTask()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    (Player as Scannable).Scan();

                    foreach (var m in Monsters)
                        if (m is Scannable ms)
                            ms.Scan();

                    Scan();

                    await Task.Delay(100);
                }
            });
        }
    }
}
