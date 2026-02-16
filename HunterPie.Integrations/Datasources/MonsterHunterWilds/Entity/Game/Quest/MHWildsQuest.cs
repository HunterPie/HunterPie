using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Quest.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Game.Quest;

public sealed class MHWildsQuest(
    MHWildsQuestInformation information,
    MHWildsQuestDetails? details) : IQuest, IEventDispatcher, IUpdatable<UpdateQuest>, IDisposable
{
    /// <inheritdoc />
    public int Id { get; } = information.Id;

    /// <inheritdoc />
    public string Name => string.Empty;

    /// <inheritdoc />
    public QuestType Type { get; } = details?.ToQuestType() ?? QuestType.Hunt;

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
            this.Dispatch(
                toDispatch: _onQuestStatusChange,
                data: new SimpleValueChangeEventArgs<QuestStatus>(temp, value));
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
    public QuestLevel Level { get; } = details?.ToQuestLevel() ?? QuestLevel.HighRank;

    /// <inheritdoc />
    public int Stars { get; } = details?.Level ?? 0;

    /// <inheritdoc />
    public TimeSpan TimeLeft
    {
        get;
        private set
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

    public void Update(UpdateQuest data)
    {
        MaxDeaths = (int)data.Information.MaxDeaths.Decode();
        Deaths = (int)data.Deaths.Decode();
        Status = data.Information.ToQuestStatus();
        TimeLeft = TimeSpan.FromMilliseconds(data.Information.MaxTimer - data.Information.Timer);
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