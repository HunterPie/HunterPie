using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities.Data;
using System.Collections.Concurrent;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;

public class MHWildsMaterialRetrieval : IEventDispatcher, IUpdatable<UpdateMaterialCollectorData>, IDisposable
{
    private readonly ConcurrentDictionary<MaterialRetrievalCollector, MHWildsMaterialRetrievalCollector> _collectors = new();
    public IReadOnlyCollection<MHWildsMaterialRetrievalCollector> Collectors => _collectors.Values.ToArray();

    private readonly SmartEvent<ValueCreationEventArgs<MHWildsMaterialRetrievalCollector>> _addSource = new();
    public event EventHandler<ValueCreationEventArgs<MHWildsMaterialRetrievalCollector>> AddSource
    {
        add => _addSource.Hook(value);
        remove => _addSource.Unhook(value);
    }

    private readonly SmartEvent<ValueCreationEventArgs<MHWildsMaterialRetrievalCollector>> _removeSource = new();
    public event EventHandler<ValueCreationEventArgs<MHWildsMaterialRetrievalCollector>> RemoveSource
    {
        add => _removeSource.Hook(value);
        remove => _removeSource.Unhook(value);
    }

    public void Update(UpdateMaterialCollectorData data)
    {
        bool doesCollectorExist = _collectors.ContainsKey(data.Collector);

        MHWildsMaterialRetrievalCollector collector = _collectors.GetOrAdd(
            key: data.Collector,
            valueFactory: static (type) => new MHWildsMaterialRetrievalCollector { Collector = type }
        );

        collector.Update(data);

        if (doesCollectorExist)
            return;

        this.Dispatch(
            toDispatch: _addSource,
            data: new ValueCreationEventArgs<MHWildsMaterialRetrievalCollector>(collector)
        );
    }

    public void Dispose()
    {
        _addSource.Dispose();
        _removeSource.Dispose();
        _collectors.Values.DisposeAll();
    }
}