using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.World.Crypto;
using HunterPie.Core.Game.World.Entities;
using HunterPie.Core.Game.World.Utils;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HunterPie.Core.Game.World
{
    public class MHWGame : Scannable, IGame, IEventDispatcher
    {
        public const long ALL_TARGETS = 0;

        private readonly MHWPlayer _player;
        private readonly Dictionary<long, IMonster> _monsters = new();
        private readonly Dictionary<long, EntityDamageData[]> _damageDone = new();
        private bool _isMouseVisible;
        private float _timeElapsed;
        private int _deaths;
        private Stopwatch _damageUpdateStopwatch = new();

        public event EventHandler<IMonster> OnMonsterSpawn;
        public event EventHandler<IMonster> OnMonsterDespawn;
        public event EventHandler<IGame> OnHudStateChange;
        public event EventHandler<IGame> OnTimeElapsedChange;
        public event EventHandler<IGame> OnDeathCountChange;

        public IPlayer Player => _player;
        public List<IMonster> Monsters { get; } = new();

        public IChat Chat => throw new NotImplementedException();

        public bool IsHudOpen
        {
            get => _isMouseVisible;
            private set
            {
                if (value != _isMouseVisible)
                {
                    _isMouseVisible = value;
                    this.Dispatch(OnHudStateChange, this);
                }
            }
        }

        public float TimeElapsed
        {
            get => _timeElapsed;
            private set
            {
                if (value != _timeElapsed)
                {
                    _timeElapsed = value;
                    this.Dispatch(OnTimeElapsedChange, this);
                }
            }
        }

        public int Deaths
        {
            get => _deaths;
            private set
            {
                if (value != _deaths)
                {
                    _deaths = value;
                    this.Dispatch(OnDeathCountChange, this);
                }
            }
        }

        public MHWGame(IProcessManager process) : base(process)
        {
            _player = new(process);

            DamageMessageHandler.OnReceived += OnReceivePlayersDamage;
            _player.OnStageUpdate += OnPlayerStageUpdate;

            ScanManager.Add(_player, this);
        }

        [ScannableMethod]
        private void GetMouseVisibilityState()
        {
            bool isMouseVisible = _process.Memory.Deref<int>(
                AddressMap.GetAbsolute("GAME_MOUSE_INFO_ADDRESS"),
                AddressMap.Get<int[]>("MOUSE_VISIBILITY_OFFSETS")
            ) == 1;

            IsHudOpen = isMouseVisible;
        }

        [ScannableMethod]
        private void GetTimeElapsed()
        {
            long questEndTimerPtrs = _process.Memory.Read(
                AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
                AddressMap.Get<int[]>("QUEST_TIMER_OFFSETS")
            );
            ulong[] timers = _process.Memory.Read<ulong>(questEndTimerPtrs, 2);
            ulong encryptKey = timers[0];
            ulong encryptedValue = timers[1];

            float questMaxTimer = _process.Memory.Read<uint>(questEndTimerPtrs + 0x1C)
                                                 .ApproximateHigh(MHWGameUtils.MaxQuestTimers)
                                                 .ToSeconds();

            float elapsed = MHWCrypto.DecryptQuestTimer(encryptedValue, encryptKey);

            TimeElapsed = Math.Max(0, questMaxTimer - elapsed);
        }

        [ScannableMethod]
        private void GetPartyMembersDamage()
        {
            if (_damageUpdateStopwatch.IsRunning && _damageUpdateStopwatch.ElapsedMilliseconds < 100)
                return;

            _damageUpdateStopwatch.Restart();

            if (Player.InHuntingZone)
                DamageMessageHandler.RequestHuntStatistics(ALL_TARGETS);
        }

        [ScannableMethod]
        private void GetDeathCounter()
        {
            int deathCounter = _process.Memory.Deref<int>(
                AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
                AddressMap.Get<int[]>("QUEST_DEATH_COUNTER_OFFSETS")
            );

            Deaths = deathCounter;
        }

        [ScannableMethod]
        private void GetMonsterDoubleLinkedList()
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

        public void Dispose() {}

        #region Damage helpers

        private void OnPlayerStageUpdate(object sender, EventArgs e)
        {
            DamageMessageHandler.ClearAllHuntStatisticsExcept(Array.Empty<long>());
            DamageMessageHandler.RequestHuntStatistics(ALL_TARGETS);
        }

        private void OnReceivePlayersDamage(object sender, ResponseDamageMessage e)
        {
            long target = e.Target;

            _damageDone[target] = e.Entities;

            EntityDamageData[] damages = _damageDone.Values.SelectMany(entity => entity)
                .GroupBy(entity => entity.Entity.Index)
                .Select(group =>
                {
                    EntityDamageData entity = group.ElementAt(0);

                    return new EntityDamageData
                    {
                        Target = entity.Target,
                        Entity = entity.Entity,
                        RawDamage = group.Sum(e => e.RawDamage),
                        ElementalDamage = group.Sum(e => e.ElementalDamage)
                    };
                })
                .ToArray();

            _player.UpdatePartyMembersDamage(damages);
        }

        #endregion
    }
}
