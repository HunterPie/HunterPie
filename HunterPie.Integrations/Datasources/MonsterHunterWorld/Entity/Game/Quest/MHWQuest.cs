using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Game.Quest;

public class MHWQuest(
    IGameProcess process,
    IScanService scanService,
    int id,
    int stars,
    QuestType questType
    ) : Scannable(process, scanService), IQuest, IDisposable, IEventDispatcher
{
    /// <inheritdoc />
    public int Id { get; } = id;

    /// <inheritdoc />
    public string Name => string.Empty;

    /// <inheritdoc />
    public QuestType Type { get; } = questType;

    /// <inheritdoc />
    public QuestStatus Status
    {
        get;
        private set
        {
            if (value == field)
                return;

            QuestStatus temp = field;
            field = value;
            this.Dispatch(_onQuestStatusChange, new SimpleValueChangeEventArgs<QuestStatus>(temp, value));
        }
    }

    /// <inheritdoc />
    public int Deaths
    {
        get;
        private set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(_onDeathCounterChange, new CounterChangeEventArgs(value, MaxDeaths));
        }
    }

    /// <inheritdoc />
    public int MaxDeaths
    {
        get;
        private set
        {
            if (value == field)
                return;

            field = value;
            this.Dispatch(_onDeathCounterChange, new CounterChangeEventArgs(Deaths, value));
        }
    }

    /// <inheritdoc />
    public QuestLevel Level { get; } = GetLevelByStars(stars);

    /// <inheritdoc />
    public int Stars { get; } = stars % 10;

    /// <inheritdoc />
    public TimeSpan TimeLeft
    {
        get;
        protected set
        {
            if (value == field)
                return;

            TimeSpan oldValue = field;
            field = value;
            this.Dispatch(_onTimeLeftChange, new SimpleValueChangeEventArgs<TimeSpan>(oldValue, value));
        }
    } = TimeSpan.Zero;

    private readonly SmartEvent<SimpleValueChangeEventArgs<QuestStatus>> _onQuestStatusChange = new();

    /// <inheritdoc />
    public event EventHandler<SimpleValueChangeEventArgs<QuestStatus>> OnQuestStatusChange
    {
        add => _onQuestStatusChange.Hook(value);
        remove => _onQuestStatusChange.Unhook(value);
    }

    private readonly SmartEvent<CounterChangeEventArgs> _onDeathCounterChange = new();

    /// <inheritdoc />
    public event EventHandler<CounterChangeEventArgs> OnDeathCounterChange
    {
        add => _onDeathCounterChange.Hook(value);
        remove => _onDeathCounterChange.Unhook(value);
    }

    private readonly SmartEvent<SimpleValueChangeEventArgs<TimeSpan>> _onTimeLeftChange = new();

    /// <inheritdoc />
    public event EventHandler<SimpleValueChangeEventArgs<TimeSpan>> OnTimeLeftChange
    {
        add => _onTimeLeftChange.Hook(value);
        remove => _onTimeLeftChange.Unhook(value);
    }

    [ScannableMethod]
    private async Task GetQuestData()
    {
        MHWQuestStructure quest = await Memory.DerefAsync<MHWQuestStructure>(
            address: AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_DATA_OFFSETS")
        );
        MHWQuestDataStructure questData = await Memory.DerefAsync<MHWQuestDataStructure>(
            address: AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_EXTRA_DATA_OFFSETS")
        );

        Status = quest.State.ToStatus();
        MaxDeaths = questData.MaxDeaths;
        Deaths = questData.Deaths;
    }

    [ScannableMethod]
    private async Task GetTimer()
    {
        ulong timeLeft = await Memory.DerefAsync<ulong>(
            address: AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_TIMER_OFFSETS")
        );

        TimeLeft = TimeSpan.FromSeconds(
            value: MHWCrypto.LiterallyWhyCapcom(timeLeft)
        );
    }

    public void Dispose()
    {
        IDisposableExtensions.DisposeAll(
            _onQuestStatusChange,
            _onDeathCounterChange,
            _onTimeLeftChange
        );
    }

    private static QuestLevel GetLevelByStars(int stars)
    {
        return stars switch
        {
            >= 1 and < 6 => QuestLevel.LowRank,
            >= 5 and < 10 => QuestLevel.HighRank,
            >= 10 => QuestLevel.MasterRank,
            _ => throw new ArgumentOutOfRangeException(nameof(stars), stars, null)
        };
    }
}