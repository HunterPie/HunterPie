using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO.Monster;
using HunterPie.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE0051

namespace HunterPie.Core.Game.Environment
{
    public class Monster : Scannable
    {
        #region Private

        private long _monsterAddress;
        private int _index;

        #endregion

        public long MonsterAddress
        {
            get => _monsterAddress;
            private set
            {
                if (value != _monsterAddress)
                {
                    if (_monsterAddress != 0)
                    {
                        Model = null;
                    }
                    _monsterAddress = value;
                }
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            private set 
            { 
                if (value != _model)
                {
                    _model = value;
                }
            }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }


        public Monster(IProcessManager process, int index)
            : base(process)
        {
            _index = index;
        }

        [ScannableMethod(typeof(MonsterAddressData))]
        private void GetMonsterAddress()
        {
            MonsterAddressData dto = new();

            long thirdMonster = _process.Memory.Read(
                AddressMap.GetAbsolute("MONSTER_OFFSET"),
                AddressMap.Get<int[]>("MonsterOffsets")
            );

            switch (_index)
            {
                case 3:
                    dto.Address = thirdMonster;
                    break;
                case 2:
                    dto.Address = _process.Memory.Read<long>(thirdMonster - 0x30) + 0x40;
                    break;
                case 1:
                    dto.Address = _process.Memory.Read<long>(
                        _process.Memory.Read<long>(thirdMonster - 0x30) + 0x10
                    ) + 0x40;
                    break;
            }

            Next(ref dto);

            MonsterAddress = dto.Address;
        }

        [ScannableMethod(typeof(MonsterInformationData))]
        private void GetMonsterInformation()
        {
            MonsterInformationData dto = new();

            long namePtr = _process.Memory.Read<long>(MonsterAddress + 0x2A0);
            string monsterEm = _process.Memory.Read(namePtr + 0x0C, 64);

            if (!string.IsNullOrEmpty(monsterEm))
            {
                string[] monsterEmSplit = monsterEm.Split('\\');
                if (monsterEmSplit.ElementAtOrDefault(3) is null)
                {
                    Model = null;
                    return;
                }

                monsterEm = monsterEmSplit.LastOrDefault();
                
                if (!monsterEm.StartsWith("em"))
                    return;


            }

            Next(ref dto);

            return;
        }
    }
}
