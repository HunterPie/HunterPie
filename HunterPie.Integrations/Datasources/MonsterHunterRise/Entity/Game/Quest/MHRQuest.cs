using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Quest;
using HunterPie.Core.Game.Events;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Game.Quest;

public class MHRQuest : IQuest, IDisposable, IEventDispatcher
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
            this.Dispatch(_onQuestStatusChange, new SimpleValueChangeEventArgs<QuestStatus>(temp, value));
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



    public void Dispose()
    {
        IDisposableExtensions.DisposeAll(
            _onQuestStatusChange,
            _onDeathCounterChange
        );
    }
}