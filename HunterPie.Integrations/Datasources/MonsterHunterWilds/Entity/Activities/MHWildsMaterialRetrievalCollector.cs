using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;

public class MHWildsMaterialRetrievalCollector : IEventDispatcher, IUpdatable<UpdateMaterialCollectorData>, IDisposable
{
    public required MaterialRetrievalCollector Collector { get; init; }

    private int _count;
    public int Count
    {
        get => _count;
        private set
        {
            if (value == _count)
                return;

            int temp = _count;
            _count = value;
            this.Dispatch(
                toDispatch: _countChanged,
                data: new SimpleValueChangeEventArgs<int>(
                    oldValue: temp,
                    newValue: value
                )
            );
        }
    }

    public int MaxCount { get; private set; }

    private readonly SmartEvent<SimpleValueChangeEventArgs<int>> _countChanged = new();
    public event EventHandler<SimpleValueChangeEventArgs<int>> CountChanged
    {
        add => _countChanged.Hook(value);
        remove => _countChanged.Unhook(value);
    }

    public void Update(UpdateMaterialCollectorData data)
    {
        MaxCount = data.MaxCount;
        Count = data.Count;
    }

    public void Dispose() => _countChanged.Dispose();
}