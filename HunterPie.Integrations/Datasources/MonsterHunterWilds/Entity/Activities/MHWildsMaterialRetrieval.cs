using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities.Data;
using System.Collections.Concurrent;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;

public class MHWildsMaterialRetrieval : IEventDispatcher, IUpdatable<UpdateMaterialCollectorData>
{
    private readonly ConcurrentDictionary<MaterialRetrievalSourceType, MHWildsMaterialRetrievalSource> _sources = new();
    public IReadOnlyCollection<MHWildsMaterialRetrievalSource> Sources => _sources.Values.ToArray();

    private readonly SmartEvent<ValueCreationEventArgs<MHWildsMaterialRetrievalSource>> _addSource = new();
    public event EventHandler<ValueCreationEventArgs<MHWildsMaterialRetrievalSource>> AddSource
    {
        add => _addSource.Hook(value);
        remove => _addSource.Unhook(value);
    }

    private readonly SmartEvent<ValueCreationEventArgs<MHWildsMaterialRetrievalSource>> _removeSource = new();
    public event EventHandler<ValueCreationEventArgs<MHWildsMaterialRetrievalSource>> RemoveSource
    {
        add => _removeSource.Hook(value);
        remove => _removeSource.Unhook(value);
    }

    public void Update(UpdateMaterialCollectorData data)
    {
        MHWildsMaterialRetrievalSource collector = _sources.GetOrAdd(
            key: data.Type,
            valueFactory: static (type) => new MHWildsMaterialRetrievalSource { SourceType = type }
        );

        collector.Update(data);
    }
}