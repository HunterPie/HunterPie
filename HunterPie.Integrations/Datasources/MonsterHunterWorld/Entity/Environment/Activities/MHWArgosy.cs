using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;

public class MHWArgosy : IEventDispatcher, IDisposable, IUpdatable<MHWArgosyData>
{
    public int DaysLeft
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onDaysLeftChange, this);
            }
        }
    }

    public bool IsInTown { get; private set; }

    private readonly SmartEvent<MHWArgosy> _onDaysLeftChange = new();
    public event EventHandler<MHWArgosy> OnDaysLeftChange
    {
        add => _onDaysLeftChange.Hook(value);
        remove => _onDaysLeftChange.Unhook(value);
    }

    public void Dispose()
    {
        _onDaysLeftChange.Dispose();
    }

    public void Update(MHWArgosyData data)
    {
        IsInTown = data.IsInTown;
        DaysLeft = data.DaysLeft;
    }
}