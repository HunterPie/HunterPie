using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using System;
using System.Text;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRPlayer : Scannable, IPlayer, IEventDispatcher
    {
        #region Private
        private string _name;
        private int _stageId;
        #endregion 

        public string Name
        {
            get => _name;
            private set
            {
                if (value != _name)
                {
                    _name = value;
                    this.Dispatch(value is ""
                        ? OnLogout
                        : OnLogin);

                }
            }
        }

        public int HighRank { get; private set; }

        public int StageId
        {
            get => _stageId;
            private set
            {
                if (value != _stageId)
                {
                    _stageId = value;
                    
                    this.Dispatch(OnStageUpdate);
                }
            }
        }

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

        public MHRPlayer(IProcessManager process) : base(process) { }

        // TODO: Add DTOs for middlewares


        [ScannableMethod]
        private void ScanStageData()
        {
            long stageAddress = _process.Memory.Read(
                AddressMap.GetAbsolute("STAGE_ADDRESS"), 
                AddressMap.Get<int[]>("STAGE_OFFSETS")
            );

            // TODO: Transform this into a structure instead of an array
            int[] stageIds = _process.Memory.Read<int>(stageAddress + 0x64, 4);

            int villageId = stageIds[0];
            int huntId = stageIds[3];
            int zoneId = huntId != -1
                ? huntId + 200
                : villageId;

            StageId = zoneId;
        }

        [ScannableMethod]
        private void ScanPlayerSaveData()
        {
            if (StageId == -1)
            {
                Name = "";
                return;
            }

            long currentPlayerSaveAddress = _process.Memory.Read(
                AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
                AddressMap.Get<int[]>("CHARACTER_OFFSETS")
            );

            long namePtr = _process.Memory.Read<long>(currentPlayerSaveAddress);
            int nameLength = _process.Memory.Read<int>(namePtr + 0x10);
            string name = _process.Memory.Read(namePtr + 0x14, (uint)(nameLength * 2), encoding: Encoding.Unicode);

            Name = name;
        }
    }
}
