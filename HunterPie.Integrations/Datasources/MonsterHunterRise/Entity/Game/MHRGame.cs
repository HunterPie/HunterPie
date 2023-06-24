
using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage;
using HunterPie.Core.Native.IPC.Handlers.Internal.Damage.Models;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common;
using HunterPie.Integrations.Datasources.Common.Entity.Game;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Chat;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Services;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Text;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game;

#pragma warning disable IDE0051 // Remove unused private members
public sealed class MHRGame : CommonGame
{
    public const uint MAXIMUM_MONSTER_ARRAY_SIZE = 5;
    public const int TRAINING_ROOM_ID = 5;

    private readonly MHRChat _chat = new();
    private readonly MHRPlayer _player;
    private float _timeElapsed;
    private (int, DateTime) _lastTeleport = (0, DateTime.Now);
    private int _maxDeaths;
    private int _deaths;
    private bool _isHudOpen;
    private bool _isInQuest;
    private DateTime _lastDamageUpdate = DateTime.MinValue;
    private readonly Dictionary<long, IMonster> _monsters = new();
    private readonly Dictionary<long, EntityDamageData[]> _damageDone = new();

    public override IPlayer Player => _player;
    public override List<IMonster> Monsters { get; } = new();

    public override IChat Chat => _chat;

    public override bool IsHudOpen
    {
        get => _isHudOpen;
        protected set
        {
            if (value != _isHudOpen)
            {
                _isHudOpen = value;
                this.Dispatch(_onHudStateChange, this);
            }
        }
    }

    public override float TimeElapsed
    {
        get => _timeElapsed;
        protected set
        {
            if (value != _timeElapsed)
            {
                bool hasReset = value - _timeElapsed > 5;

                _timeElapsed = value;
                this.Dispatch(_onTimeElapsedChange, new TimeElapsedChangeEventArgs(hasReset, value));
            }
        }
    }

    public override int MaxDeaths
    {
        get => _maxDeaths;
        protected set
        {
            if (value != _maxDeaths)
            {
                _maxDeaths = value;
                this.Dispatch(_onDeathCountChange, this);
            }
        }
    }

    public override int Deaths
    {
        get => _deaths;
        protected set
        {
            if (value != _deaths)
            {
                _deaths = value;
                this.Dispatch(_onDeathCountChange, this);
            }
        }
    }

    public override bool IsInQuest
    {
        get => _isInQuest;
        protected set
        {
            if (value != _isInQuest)
            {
                _isInQuest = value;
                this.Dispatch(value ? _onQuestStart : _onQuestEnd, new QuestStateChangeEventArgs(this));
            }
        }
    }

    public override IAbnormalityCategorizationService AbnormalityCategorizationService { get; } = new MHRAbnormalityCategorizationService();

    public MHRGame(IProcessManager process) : base(process)
    {
        _player = new MHRPlayer(process);

        ScanManager.Add(
            this,
            Player as Scannable
        );

        HookEvents();
    }

    private void HookEvents()
    {
        DamageMessageHandler.OnReceived += OnReceivePlayersDamage;
        _player.OnStageUpdate += OnPlayerStageUpdate;
    }

    public override void Dispose()
    {
        DamageMessageHandler.OnReceived -= OnReceivePlayersDamage;
        _player.OnStageUpdate -= OnPlayerStageUpdate;
        base.Dispose();
    }

    [ScannableMethod]
    private void ScanChat()
    {
        long chatArrayPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("CHAT_ADDRESS"),
            AddressMap.Get<int[]>("CHAT_OFFSETS")
        );
        long chatArray = Process.Memory.Read<long>(chatArrayPtr);
        int chatCount = Process.Memory.Read<int>(chatArrayPtr + 0x8);

        if (chatCount <= 0)
            return;

        long[] chatMessagePtrs = Process.Memory.Read<long>(chatArray + 0x20, (uint)chatCount);

        bool isChatOpen = false;

        for (int i = 0; i < chatCount; i++)
        {
            long messagePtr = chatMessagePtrs[i];

            MHRChatMessageStructure message = Process.Memory.Read<MHRChatMessageStructure>(messagePtr);

            if (message.Type is not 0 and not 1)
                continue;

            if (!isChatOpen)
                isChatOpen |= message.Visibility == 2;

            if (_chat.ContainsMessage(messagePtr))
                continue;

            MHRChatMessage messageData = DerefChatMessage(message);

            _chat.AddMessage(messagePtr, messageData);
        }

        if (!isChatOpen)
            isChatOpen |= Process.Memory.Deref<byte>(
                AddressMap.GetAbsolute("CHAT_UI_ADDRESS"),
                AddressMap.Get<int[]>("CHAT_UI_OFFSETS")
            ) == 1;

        _chat.SetChatState(isChatOpen);
    }

    [ScannableMethod]
    private void GetElapsedTime()
    {
        float elapsedTime = Process.Memory.Deref<float>(
            AddressMap.GetAbsolute("QUEST_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_TIMER_OFFSETS")
        );

        TimeElapsed = elapsedTime > 0
            ? elapsedTime
            : (float)(DateTime.Now - _lastTeleport.Item2).TotalSeconds;

        if (Player.StageId != _lastTeleport.Item1)
            _lastTeleport = (Player.StageId, DateTime.Now);

    }

    [ScannableMethod]
    private void GetDeathCounter()
    {
        if (!Player.InHuntingZone)
            return;

        int maxDeathsCounter = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_MAX_DEATHS_OFFSETS")
        );

        int deathCounter = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_DEATH_COUNTER_OFFSETS")
        );

        MaxDeaths = maxDeathsCounter;
        Deaths = deathCounter;
    }

    [ScannableMethod]
    private void GetQuestState()
    {
        if (!Player.InHuntingZone)
        {
            IsInQuest = false;
            return;
        }

        var questState = (QuestState)Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_STATUS_OFFSETS")
        );

        var questType = (QuestType)Memory.Deref<uint>(
            AddressMap.GetAbsolute("QUEST_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_TYPE_OFFSETS")
        );

        bool isInQuest = questState == QuestState.InQuest;

        QuestStatus = questState switch
        {
            QuestState.InQuest => Core.Game.Enums.QuestStatus.InProgress,
            QuestState.Success => Core.Game.Enums.QuestStatus.Success,
            QuestState.Failed => Core.Game.Enums.QuestStatus.Fail,
            QuestState.Returning or QuestState.Reset => Core.Game.Enums.QuestStatus.Quit,
            _ => Core.Game.Enums.QuestStatus.None
        };

        IsInQuest = isInQuest && questType.IsHuntQuest();
    }

    [ScannableMethod]
    private void GetPartyMembersDamage()
    {
        if ((DateTime.Now - _lastDamageUpdate).TotalMilliseconds < 100)
            return;

        _lastDamageUpdate = DateTime.Now;

        if (Player.InHuntingZone)
            DamageMessageHandler.RequestHuntStatistics(CommonConstants.AllTargets);
    }

    [ScannableMethod]
    private void GetUIState()
    {
        byte isHudOpen = Process.Memory.Deref<byte>(
            AddressMap.GetAbsolute("MOUSE_ADDRESS"),
            AddressMap.Get<int[]>("MOUSE_OFFSETS")
        );

        byte isCutsceneActive = Process.Memory.Deref<byte>(
            AddressMap.GetAbsolute("EVENTCAMERA_ADDRESS"),
            AddressMap.Get<int[]>("CUTSCENE_STATE_OFFSETS")
        );

        IsHudOpen = isHudOpen == 1 || isCutsceneActive != 0;
    }

    [ScannableMethod]
    private void GetMonstersArray()
    {
        // Only scans for monsters in hunting areas
        if (!Player.InHuntingZone && Player.StageId != TRAINING_ROOM_ID)
        {
            if (_monsters.Keys.Count > 0)
                foreach (long mAddress in _monsters.Keys)
                    HandleMonsterDespawn(mAddress);

            return;
        }

        long address = Process.Memory.Read(
            AddressMap.GetAbsolute("MONSTERS_ADDRESS"),
            AddressMap.Get<int[]>("MONSTER_LIST_OFFSETS")
        );

        uint monsterArraySize = Process.Memory.Read<uint>(address + 0x1C);
        var monsterAddresses = Process.Memory.Read<long>(address + 0x20, Math.Min(MAXIMUM_MONSTER_ARRAY_SIZE, monsterArraySize))
            .Where(mAddress => mAddress != 0)
            .ToHashSet();

        long[] toDespawn = _monsters.Keys.Where(address => !monsterAddresses.Contains(address)
            || (MonsterDieCategory)Process.Memory.ReadPtr(address, AddressMap.Get<int[]>("MONSTER_DIE_CATEGORY_OFFSETS")) != MonsterDieCategory.None)
            .ToArray();

        foreach (long mAddress in toDespawn)
            HandleMonsterDespawn(mAddress);

        long[] toSpawn = monsterAddresses.Where(address => !_monsters.ContainsKey(address)
            && (MonsterDieCategory)Process.Memory.ReadPtr(address, AddressMap.Get<int[]>("MONSTER_DIE_CATEGORY_OFFSETS")) == MonsterDieCategory.None)
            .ToArray();

        foreach (long mAddress in toSpawn)
            HandleMonsterSpawn(mAddress);
    }

    private void HandleMonsterSpawn(long monsterAddress)
    {
        if (monsterAddress == 0 || _monsters.ContainsKey(monsterAddress))
            return;

        var monster = new MHRMonster(Process, monsterAddress);
        _monsters.Add(monsterAddress, monster);
        Monsters.Add(monster);
        ScanManager.Add(monster);

        this.Dispatch(_onMonsterSpawn, monster);
    }

    private void HandleMonsterDespawn(long address)
    {
        if (_monsters[address] is not MHRMonster monster)
            return;

        _ = _monsters.Remove(address);
        _ = _damageDone.Remove(address);
        _ = Monsters.Remove(monster);
        ScanManager.Remove(monster);


        this.Dispatch(_onMonsterDespawn, monster);

        monster.Dispose();
    }

    #region Damage helpers

    private static void OnPlayerStageUpdate(object? sender, EventArgs e)
    {
        DamageMessageHandler.ClearAllHuntStatisticsExcept(Array.Empty<long>());
        DamageMessageHandler.RequestHuntStatistics(CommonConstants.AllTargets);
    }

    private void OnReceivePlayersDamage(object? sender, ResponseDamageMessage e)
    {
        long target = e.Target;

        _damageDone[target] = e.Entities;

        EntityDamageData[] damages = _damageDone.Values.SelectMany(entity => entity)
            .GroupBy(entity => entity.Entity.Index)
            .Select(group =>
            {
                EntityDamageData entity = group.ElementAt(0);

                return entity with
                {
                    RawDamage = group.Sum(damage => damage.RawDamage),
                    ElementalDamage = group.Sum(damage => damage.ElementalDamage)
                };
            })
            .ToArray();

        _player.UpdatePartyMembersDamage(damages);
    }

    #endregion

    #region Chat helpers
    private MHRChatMessage DerefChatMessage(MHRChatMessageStructure message)
    {
        return message.Type switch
        {
            0x0 => DerefNormalChatMessage(message),
            _ => DerefUnknownTypeMessage()
        };
    }

    private MHRChatMessage DerefNormalChatMessage(MHRChatMessageStructure message)
    {
        int messageStringLength = Process.Memory.Read<int>(message.Message + 0x10);
        int messageAuthorLength = Process.Memory.Read<int>(message.Author + 0x10);

        string messageString = Process.Memory.Read(message.Message + 0x14, (uint)messageStringLength * 2, Encoding.Unicode);
        string messageAuthor = Process.Memory.Read(message.Author + 0x14, (uint)messageAuthorLength * 2, Encoding.Unicode);

        return new MHRChatMessage
        {
            Message = messageString,
            Author = messageAuthor,
            Type = AuthorType.Player,
            PlayerSlot = message.PlayerSlot,
        };
    }

    private MHRChatMessage DerefAutoChatMessage(MHRChatMessageStructure message)
    {
        int messageAuthorLength = Process.Memory.Read<int>(message.Author + 0x10);
        string messageAuthor = Process.Memory.Read(messageAuthorLength + 0x14, (uint)messageAuthorLength * 2, Encoding.Unicode);

        return new MHRChatMessage
        {
            Message = "<Auto message>",
            Author = messageAuthor,
            Type = AuthorType.Auto
        };
    }

    private MHRChatMessage DerefUnknownTypeMessage() => new() { Type = AuthorType.None };

    #endregion
}
#pragma warning restore IDE0051 // Remove unused private members
