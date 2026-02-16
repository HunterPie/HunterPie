using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;

public class MHWSteamworks : IEventDispatcher, IDisposable, IUpdatable<MHWSteamFuelData>
{
    public int NaturalFuel
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onNaturalFuelChange, this);
            }
        }
    }

    public int MaxNaturalFuel => 700;

    public int StoredFuel
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onStoredFuelChange, this);
            }
        }
    }

    private readonly SmartEvent<MHWSteamworks> _onNaturalFuelChange = new();
    public event EventHandler<MHWSteamworks> OnNaturalFuelChange
    {
        add => _onNaturalFuelChange.Hook(value);
        remove => _onNaturalFuelChange.Unhook(value);
    }

    private readonly SmartEvent<MHWSteamworks> _onStoredFuelChange = new();
    public event EventHandler<MHWSteamworks> OnStoredFuelChange
    {
        add => _onStoredFuelChange.Hook(value);
        remove => _onStoredFuelChange.Unhook(value);
    }

    public void Dispose()
    {
        _onStoredFuelChange.Dispose();
        _onNaturalFuelChange.Dispose();
    }

    public void Update(MHWSteamFuelData data)
    {
        NaturalFuel = data.NaturalFuel;
        StoredFuel = data.StoredFuel;
    }
}