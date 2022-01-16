using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Logger;
using System;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRPlayer : Scannable, IPlayer, IEventDispatcher
    {
        #region Private
        private int _stageId;
        #endregion 

        public string Name { get; private set; }

        public int HighRank { get; private set; }

        public int StageId
        {
            get => _stageId;
            private set
            {
                if (value != _stageId)
                {
                    if (value == -1 || _stageId == -1)
                        this.Dispatch(value == -1 
                            ? OnLogout
                            : OnLogin);

                    this.Dispatch(OnStageUpdate);
                    _stageId = value;
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
    }
}
