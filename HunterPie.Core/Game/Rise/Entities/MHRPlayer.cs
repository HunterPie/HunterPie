using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Rise.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace HunterPie.Core.Game.Rise.Entities
{
    public class MHRPlayer : Scannable, IPlayer, IEventDispatcher
    {
        #region Private
        private int SaveSlotId;
        private string _name;
        private int _stageId;
        private Weapon _weaponId;
        #endregion 

        public string Name
        {
            get => _name;
            private set
            {
                if (value != _name)
                {
                    _name = value;
                    FindPlayerSaveSlot();
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

        public Weapon WeaponId
        {
            get => _weaponId;
            private set
            {
                if (value != _weaponId)
                {
                    _weaponId = value;
                    this.Dispatch(OnWeaponChange);
                }
            }
        }

        public List<IPartyMember> Party { get; } = new();

        private Dictionary<int, IAbnormality> abnormalities = new();
        public IReadOnlyCollection<IAbnormality> Abnormalities => abnormalities.Values;

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
        public event EventHandler<EventArgs> OnWeaponChange;
        public event EventHandler<IAbnormality> OnAbnormalityStart;
        public event EventHandler<IAbnormality> OnAbnormalityEnd;

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
            int[] stageIds = _process.Memory.Read<int>(stageAddress + 0x60, 5);

            bool isVillage = stageIds[0] == 4;
            bool isMainMenu = stageIds[0] == 0;

            int villageId = stageIds[1];
            int huntId = stageIds[4];

            int zoneId = isMainMenu switch
            {
                true => -1,
                false => isVillage
                ? villageId
                : huntId + 200
            };

            StageId = zoneId;
        }

        [ScannableMethod]
        private void ScanPlayerSaveData()
        {
            if (StageId == -1 || StageId == 199)
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

        private void FindPlayerSaveSlot()
        {
            if (StageId == -1 || StageId == 199)
            {
                Name = "";
                SaveSlotId = -1;
                return;
            }

            long currentPlayerSaveAddress = _process.Memory.Read(
                AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
                AddressMap.Get<int[]>("CHARACTER_OFFSETS")
            );
            long namePtr = _process.Memory.Read<long>(currentPlayerSaveAddress);

            long saveAddress = _process.Memory.Read(
                AddressMap.GetAbsolute("SAVE_ADDRESS"), 
                AddressMap.Get<int[]>("SAVE_OFFSETS")
            );

            for (int i = 0; i < 3; i++)
            {
                int[] nameOffsets = { i * 8 + 0x20, 0x10 };

                long saveNamePtr = _process.Memory.Deref<long>(saveAddress, nameOffsets);

                if (saveNamePtr == namePtr)
                {
                    SaveSlotId = i;
                    return;
                }
            }
        }

        [ScannableMethod]
        private void ScanPlayerLevel()
        {
            if (SaveSlotId < 0)
                return;

            long saveAddress = _process.Memory.Read(
                AddressMap.GetAbsolute("SAVE_ADDRESS"),
                AddressMap.Get<int[]>("SAVE_OFFSETS")
            );

            int[] levelOffsets = { SaveSlotId * 8 + 0x20, 0x18 };

            int level = _process.Memory.Deref<int>(saveAddress, levelOffsets);

            HighRank = level;
        }

        [ScannableMethod]
        private void ScanPlayerWeaponData()
        {
            long weaponIdPtr = _process.Memory.Read(
                AddressMap.GetAbsolute("WEAPON_ADDRESS"),
                AddressMap.Get<int[]>("WEAPON_OFFSETS")    
            );

            int weaponId = _process.Memory.Read<int>(weaponIdPtr + 0x8C);

            // Why can't capcom keep the same ids for weapons in all their games? :tired:
            WeaponId = weaponId switch
            {
                0 => Weapon.Greatsword,
                1 => Weapon.SwitchAxe,
                2 => Weapon.Longsword,
                3 => Weapon.LightBowgun,
                4 => Weapon.HeavyBowgun,
                5 => Weapon.Hammer,
                6 => Weapon.GunLance,
                7 => Weapon.Lance,
                8 => Weapon.SwordAndShield,
                9 => Weapon.DualBlades,
                10 => Weapon.HuntingHorn,
                11 => Weapon.ChargeBlade,
                12 => Weapon.InsectGlaive,
                13 => Weapon.Bow,
                _ => Weapon.None,
            };
        }

        [ScannableMethod]
        private void ScanPlayerConsumableAbnormalities()
        {
            long consumableBuffs = _process.Memory.Read(
                AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"), 
                AddressMap.Get<int[]>("CONS_ABNORMALITIES_OFFSETS")
            );

            AbnormalitySchema[] consumableSchemas = AbnormalityData.GetAllConsumableAbnormalities();

            foreach (AbnormalitySchema schema in consumableSchemas)
            {
                MHRConsumableStructure abnormality = _process.Memory.Read<MHRConsumableStructure>(consumableBuffs + schema.Offset);

                if (abnormality.Timer > 0 && abnormalities.ContainsKey(schema.Offset))
                {
                    MHRConsumableAbnormality abnorm = (MHRConsumableAbnormality)abnormalities[schema.Offset];
                    abnorm.Update(abnormality);
                } else if (abnormality.Timer > 0 && !abnormalities.ContainsKey(schema.Offset))
                {
                    MHRConsumableAbnormality abnorm = new(schema.Offset);

                    abnormalities.Add(schema.Offset, abnorm);

                    abnorm.Update(abnormality);
                    this.Dispatch(OnAbnormalityStart, abnorm);
                } else if (abnormality.Timer <= 0 && abnormalities.ContainsKey(schema.Offset))
                {
                    MHRConsumableAbnormality abnorm = (MHRConsumableAbnormality)abnormalities[schema.Offset];
                    abnorm.Update(abnormality);

                    abnormalities.Remove(schema.Offset);
                    this.Dispatch(OnAbnormalityEnd, abnorm);
                }

            }
        }

        [ScannableMethod]
        private void ScanPlayerSongAbnormalities()
        {
            long songBuffsPtr = _process.Memory.Read(
                AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"), 
                AddressMap.Get<int[]>("HH_ABNORMALITIES_OFFSETS")
            );

            uint songBuffsLength = _process.Memory.Read<uint>(songBuffsPtr + 0x1C);
            long[] songBuffPtrs = _process.Memory.Read<long>(songBuffsPtr + 0x20, songBuffsLength);

            DerefSongBuffs(songBuffPtrs);
        }

        private void DerefSongBuffs(long[] buffs)
        {
            int id = 0;
            foreach (long buffPtr in buffs)
            {
                MHRHHAbnormality abnormality = _process.Memory.Read<MHRHHAbnormality>(buffPtr);

                // Abnormality is over
                if (abnormalities.ContainsKey(id) && abnormality.Timer <= 0)
                {
                    MHRSongAbnormality abnorm = (MHRSongAbnormality)abnormalities[id];

                    abnorm.Update(abnormality);

                    abnormalities.Remove(id);
                    this.Dispatch(OnAbnormalityEnd, abnorm);
                } else if (abnormalities.ContainsKey(id) && abnormality.Timer > 0)
                {
                    MHRSongAbnormality abnorm = (MHRSongAbnormality)abnormalities[id];
                    abnorm.Update(abnormality);
                } else if (!abnormalities.ContainsKey(id) && abnormality.Timer > 0)
                {
                    MHRSongAbnormality abnorm = new MHRSongAbnormality(id); ;
                    
                    abnormalities.Add(id, abnorm);

                    abnorm.Update(abnormality);
                    this.Dispatch(OnAbnormalityStart, abnorm);
                }

                id++;
            }
        }
    }
}
