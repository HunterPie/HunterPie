using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;

public class MHWTailraiders : IEventDispatcher, IDisposable, IUpdatable<MHWTailraidersData>
{
    public int QuestsLeft
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onQuestsLeftChange, this);
            }
        }
    }

    public bool IsDeployed { get; private set; }

    public int MaxDays => 5;

    private readonly SmartEvent<MHWTailraiders> _onQuestsLeftChange = new();
    public event EventHandler<MHWTailraiders> OnQuestsLeftChange
    {
        add => _onQuestsLeftChange.Hook(value);
        remove => _onQuestsLeftChange.Unhook(value);
    }


    public void Dispose()
    {
        _onQuestsLeftChange.Dispose();
    }

    public void Update(MHWTailraidersData data)
    {
        IsDeployed = data.IsDeployed;
        QuestsLeft = data.QuestsLeft;
    }
}