using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using System;

namespace HunterPie.Core.Game.Client
{

    public class Player : Scannable, IEventDispatcher
    {
        #region Private fields

        private IProcessManager process;
        private long playerAddress;
        private int zoneId;

        #endregion

        #region Public fields

        public long PlayerAddress
        {
            get => playerAddress;
            private set
            {
                if (value != playerAddress)
                {
                    playerAddress = value;

                    this.Dispatch(
                        value != 0
                        ? OnLogin
                        : OnLogout,
                        EventArgs.Empty
                    );

                }
            }
        }
        public string Name { get; private set; }
        public short HighRank { get; private set; }
        public short MasterRank { get; private set; }
        public int PlayTime { get; private set; }
        public bool IsLoggedOn => playerAddress != 0;
        #endregion

        public event EventHandler<EventArgs> OnLogin;
        public event EventHandler<EventArgs> OnLogout;
        public event EventHandler<EventArgs> OnHealthUpdate;
        public event EventHandler<EventArgs> OnStaminaUpdate;
        public event EventHandler<EventArgs> OnDeath;
        public event EventHandler<EventArgs> OnActionUpdate;
        public event EventHandler<EventArgs> OnStageUpdate;
        public event EventHandler<EventArgs> OnVillageEnter;
        public event EventHandler<EventArgs> OnVillageLeave;
        public event EventHandler<EventArgs> OnAilmentUpdate;
        
        internal Player(IProcessManager process)
        {
            this.process = process;

            SetupScanners();
        }

        private void SetupScanners()
        {
            Add(GetBasicData);
        }

        public PlayerInformationData GetBasicData()
        {
            PlayerInformationData data = new();
            if (zoneId == 0)
            {
                PlayerAddress = 0;
                return data;
            }

            long firstSaveAddress = process.Memory.Read(
                AddressMap.GetAbsolute("LEVEL_OFFSET"),
                AddressMap.Get<int[]>("LevelOffsets")
            );

            uint currentSaveSlot = process.Memory.Read<uint>(firstSaveAddress + 0x44);
            long nextPlayerSave = 0x27E9F0;
            long currentPlayerSaveHeader = 
                process.Memory.Read<long>(firstSaveAddress) + nextPlayerSave * currentSaveSlot;

            if (currentPlayerSaveHeader != playerAddress)
            {
                data.Name = process.Memory.Read(currentPlayerSaveHeader + 0x50, 32);
                data.HighRank = process.Memory.Read<short>(currentPlayerSaveHeader + 0x90);
                data.MasterRank = process.Memory.Read<short>(currentPlayerSaveHeader + 0x44);
                data.PlayTime = process.Memory.Read<int>(currentPlayerSaveHeader + 0x10);

                Next(ref data);

                Name = data.Name;
                HighRank = data.HighRank;
                MasterRank = data.MasterRank;
                PlayTime = data.PlayTime;

            }

            return data;
        }

    }
}
