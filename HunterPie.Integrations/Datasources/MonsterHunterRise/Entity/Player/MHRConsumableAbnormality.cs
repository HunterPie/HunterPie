using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

public class MHRConsumableAbnormality : IAbnormality, IUpdatable<MHRConsumableStructure>, IEventDispatcher
{
    private float _timer;

    public string Id { get; }
    public string Icon { get; }
    public AbnormalityType Type => AbnormalityType.Consumable;

    public float Timer
    {
        get => _timer;
        private set
        {
            if (_timer != value)
            {
                _timer = value;
                this.Dispatch(_onTimerUpdate, this);
            }
        }
    }

    public float MaxTimer { get; private set; }
    public bool IsInfinite { get; }
    public int Level => 0;

    public bool IsBuildup => false;

    private readonly SmartEvent<IAbnormality> _onTimerUpdate = new();
    public event EventHandler<IAbnormality> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    public MHRConsumableAbnormality(AbnormalitySchema data)
    {
        Id = data.Id;
        Icon = data.Icon;
        IsInfinite = data.IsInfinite;

        if (IsBuildup)
            MaxTimer = data.MaxBuildup;
    }

    void IUpdatable<MHRConsumableStructure>.Update(MHRConsumableStructure data)
    {
        MaxTimer = Math.Max(MaxTimer, data.Timer);
        Timer = data.Timer;
    }
}
