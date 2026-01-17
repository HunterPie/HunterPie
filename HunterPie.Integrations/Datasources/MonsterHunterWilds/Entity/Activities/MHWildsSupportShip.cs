using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;

public class MHWildsSupportShip : IEventDispatcher, IDisposable, IUpdatable<MHWildsSupportShipContext>
{
    public bool InTown
    {
        get;
        private set
        {
            if (value == field)
                return;

            bool old = field;
            field = value;

            this.Dispatch(
                toDispatch: _inTownChanged,
                data: new SimpleValueChangeEventArgs<bool>(old, value)
            );
        }
    }

    public int Days
    {
        get;
        private set
        {
            if (value == field)
                return;

            int old = field;
            field = value;

            this.Dispatch(
                toDispatch: _daysChanged,
                data: new SimpleValueChangeEventArgs<int>(old, value)
            );
        }
    }

    private readonly SmartEvent<SimpleValueChangeEventArgs<bool>> _inTownChanged = new();
    public event EventHandler<SimpleValueChangeEventArgs<bool>> InTownChanged
    {
        add => _inTownChanged.Hook(value);
        remove => _inTownChanged.Unhook(value);
    }

    private readonly SmartEvent<SimpleValueChangeEventArgs<int>> _daysChanged = new();
    public event EventHandler<SimpleValueChangeEventArgs<int>> DaysChanged
    {
        add => _daysChanged.Hook(value);
        remove => _daysChanged.Unhook(value);
    }

    public void Update(MHWildsSupportShipContext data)
    {
        Days = data.Days;
        InTown = data.InTown == 1;
    }

    public void Dispose()
    {
        _inTownChanged.Dispose();
        _daysChanged.Dispose();
    }
}