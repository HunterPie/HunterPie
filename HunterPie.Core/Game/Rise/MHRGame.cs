using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise
{
    public class MHRGame : Scannable, IGame
    {
        public IPlayer Player => throw new NotImplementedException();

        public List<IMonster> Monsters { get; } = new();

        Dictionary<long, IMonster> monsters = new();

        public event EventHandler<IMonster> OnMonsterSpawn;

        public MHRGame(IProcessManager process) : base(process)
        {
            
            Task.Factory.StartNew(async () =>
            {
                
                while (true)
                {
                    foreach (var m in Monsters)
                        if (m is Scannable ms)
                            ms.Scan();
                    
                    Scan();
                    
                    await Task.Delay(100);
                }
            });
            
        }

        [ScannableMethod(typeof(MHRGame))]
        private void ScanLockon()
        {
            long address = _process.Memory.Read(
                    AddressMap.GetAbsolute("LOCKON_ADDRESS"),
                    AddressMap.Get<int[]>("LOCKON_OFFSETS")
            );

            long monsterAddress = _process.Memory.Read<long>(address);

            if (monsterAddress == 0 || monsters.ContainsKey(monsterAddress))
                return;

            IMonster monster = new MHRMonster(_process, monsterAddress);
            monsters.Add(monsterAddress, monster);
            Monsters.Add(monster);

            OnMonsterSpawn?.Invoke(this, monster);
        }
    }
}
