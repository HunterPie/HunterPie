using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Quest;

public sealed class MHWildsQuest : IQuest, IEventDispatcher, IUpdatable<CurrentQuestInformation>, IDisposable
{
    /// <inheritdoc />
    public int Id { get; }

    /// <inheritdoc />
    public string Name => string.Empty;

    /// <inheritdoc />
    public QuestType Type { get; }

    private QuestStatus _status;

    /// <inheritdoc />
    public QuestStatus Status
    {
        get => _status;
        private set
        {
            if (value == _status)
                return;

            QuestStatus temp = _status;
            _status = value;
            this.Dispatch(
                toDispatch: _onQuestStatusChange,
                data: new SimpleValueChangeEventArgs<QuestStatus>(temp, value));
        }
    }

    private int _deaths;

    /// <inheritdoc />
    public int Deaths
    {
        get => _deaths;
        private set
        {
            if (value == _deaths)
                return;

            _deaths = value;
            this.Dispatch(_onDeathCounterChange, new CounterChangeEventArgs(value, MaxDeaths));
        }
    }

    /// <inheritdoc />
    public int MaxDeaths { get; private set; }

    /// <inheritdoc />
    public QuestLevel Level { get; }

    /// <inheritdoc />
    public int Stars { get; }

    private TimeSpan _timeLeft = TimeSpan.Zero;

    /// <inheritdoc />
    public TimeSpan TimeLeft
    {
        get => _timeLeft;
        private set
        {
            if (value == _timeLeft)
                return;

            TimeSpan oldValue = _timeLeft;
            _timeLeft = value;
            this.Dispatch(_onTimeLeftChange, new SimpleValueChangeEventArgs<TimeSpan>(oldValue, value));
        }
    }

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

    public MHWildsQuest(
        QuestInformation information,
        QuestDetails? details)
    {
        Id = information.Id;
        Type = details?.ToQuestType() ?? QuestType.Hunt;
        Level = details?.ToQuestLevel() ?? QuestLevel.HighRank;
        Stars = details?.Level ?? 0;
    }

    public void Update(CurrentQuestInformation data)
    {
        Status = data.ToQuestStatus();
        TimeLeft = TimeSpan.FromSeconds((data.MaxTimer - data.Timer) / 100.0);
    }

    public void Dispose()
    {
        IDisposableExtensions.DisposeAll(
            _onDeathCounterChange,
            _onQuestStatusChange,
            _onTimeLeftChange
        );
    }
}