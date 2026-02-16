using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment;

public class MHWFertilizer : IEventDispatcher, IUpdatable<MHWFertilizerStructure>, IDisposable
{
    public Fertilizer Id
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onIdChange, this);
            }
        }
    }

    public int Count
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onCountChange, this);
            }
        }
    }

    private readonly SmartEvent<MHWFertilizer> _onIdChange = new();
    public event EventHandler<MHWFertilizer> OnIdChange
    {
        add => _onIdChange.Hook(value);
        remove => _onIdChange.Unhook(value);
    }

    private readonly SmartEvent<MHWFertilizer> _onCountChange = new();
    public event EventHandler<MHWFertilizer> OnCountChange
    {
        add => _onCountChange.Hook(value);
        remove => _onCountChange.Unhook(value);
    }

    public void Update(MHWFertilizerStructure data)
    {
        Id = (Fertilizer)data.Id;
        Count = data.Amount;
    }

    public void Dispose()
    {
        _onIdChange.Dispose();
        _onCountChange.Dispose();
    }
}