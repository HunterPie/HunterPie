using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HunterPie.Core.Extensions;
using HunterPie.Core.Address.Map;
using HunterPie.Core.Game.Client.Data.Definitions;

namespace HunterPie.Core.Game.Client
{
    public class Health : Scannable, IEventDispatcher
    {
        public static readonly Dictionary<int, float> MaxHealthIncItems = new()
        {
            // Max potion
            { 3, 100 },
            { 4, 100 },
            { 14, 10 },
            { 15, 20 },
            { 185, 100 }
        };

        private float _current;
        private float _healHealth;

        public float Current
        {
            get => _current;
            set
            {
                if (value != _current)
                {
                    _current= value;
                    this.Dispatch(OnHealthUpdate);
                }
            }
        }

        public float MaxHealth { get; private set; }
        public float HealHealth
        {
            get => _healHealth;
            set
            {
                if (value != _healHealth)
                {
                    _healHealth = value;
                    this.Dispatch(OnHeal);
                }
            }
        }

        public event EventHandler<EventArgs> OnHealthUpdate;
        public event EventHandler<EventArgs> OnMaxHealthUpdate;
        public event EventHandler<EventArgs> OnCriticalHealthUpdate;
        public event EventHandler<EventArgs> OnHeal;
        public event EventHandler<EventArgs> OnHealthExtStateUpdate;

        [ScannableMethod(typeof(HealthData))]
        private void GetHealthData()
        {
            long address = _process.Memory.Read(
                AddressMap.GetAbsolute("EQUIPMENT_OFFSET"),
                AddressMap.Get<int[]>("PlayerBasicInformationOffsets")
            );

            long cGuiHealthAddress = _process.Memory.Read(
                AddressMap.GetAbsolute("HUD_DATA_OFFSET"),
                AddressMap.Get<int[]>("gHudHealthBarOffsets")
            );

            SHealingData[] healingDataArray = _process.Memory.Read<SHealingData>(
                _process.Memory.Read<long>(address + 0x30) + 0xEBB0,
                4
            );

            float[] health = _process.Memory.Read<float>(address + 0x60, 2);

            
        }
    }
}
