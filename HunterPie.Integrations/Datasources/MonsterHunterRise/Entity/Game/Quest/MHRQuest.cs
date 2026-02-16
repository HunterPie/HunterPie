using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Scan.Service;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game.Quest;

public class MHRQuest(
    IGameProcess process,
    IScanService scanService,
    int id,
    QuestType type,
    QuestLevel level,
    int stars
    ) : Scannable(process, scanService), IQuest, IDisposable, IEventDispatcher
{
    /// <inheritdoc />
    public int Id { get; } = id;

    /// <inheritdoc />
    public string Name => string.Empty;

    /// <inheritdoc />
    public QuestType Type { get; } = type;

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
    public int MaxDeaths { get; private set; }

    /// <inheritdoc />
    public QuestLevel Level { get; } = level;

    /// <inheritdoc />
    public int Stars { get; } = stars;

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
    private async Task GetData()
    {
        MHRQuestStructure questStructure = await Memory.DerefAsync<MHRQuestStructure>(
            address: AddressMap.GetAbsolute("QUEST_ADDRESS"),
            offsets: AddressMap.GetOffsets("QUEST_OFFSETS")
        );

        Status = questStructure.State.ToQuestStatus();
        MaxDeaths = questStructure.MaxDeaths;
        Deaths = questStructure.Deaths;
        TimeLeft = TimeSpan.FromSeconds(questStructure.TimeLimit - questStructure.TimeElapsed);
    }

    public override void Dispose()
    {
        base.Dispose();
        IDisposableExtensions.DisposeAll(
            _onQuestStatusChange,
            _onDeathCounterChange,
            _onTimeLeftChange
        );
    }
}