using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

// TODO: Make this a generic Abnormality
public class MHRDebuffAbnormality : IAbnormality, IUpdatable<MHRDebuffStructure>, IEventDispatcher
{
    private float _timer;

    public string Id { get; }
    public string Icon { get; }
    public AbnormalityType Type { get; }
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
    public bool IsInfinite => false;
    public int Level => 0;

    public bool IsBuildup { get; }

    private readonly SmartEvent<IAbnormality> _onTimerUpdate = new();
    public event EventHandler<IAbnormality> OnTimerUpdate
    {
        add => _onTimerUpdate.Hook(value);
        remove => _onTimerUpdate.Unhook(value);
    }

    public MHRDebuffAbnormality(AbnormalitySchema data)
    {
        Id = data.Id;
        Icon = data.Icon;
        Type = data.Group.StartsWith("Debuffs")
            ? AbnormalityType.Debuff
            : AbnormalityType.Skill;
        IsBuildup = data.IsBuildup;

        if (IsBuildup)
            MaxTimer = data.MaxBuildup;
    }

    void IUpdatable<MHRDebuffStructure>.Update(MHRDebuffStructure structure)
    {
        MaxTimer = Math.Max(MaxTimer, structure.Timer);
        Timer = structure.Timer;
    }
}
