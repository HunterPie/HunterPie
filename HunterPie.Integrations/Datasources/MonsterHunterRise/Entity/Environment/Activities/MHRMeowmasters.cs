using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;

public class MHRMeowmasters : IEventDispatcher, IUpdatable<MHRMeowmasterData>, IDisposable
{
    public int Step
    {
        get;
        private set
        {
            if (field != value)
            {
                field = value;
                this.Dispatch(_onStepChange, this);
            }
        }
    }

    public int MaxSteps { get; private set; }

    public int ExpectedOutcome
    {
        get;
        private set
        {
            if (value != field)
            {
                field = value;
                this.Dispatch(_onExpectedOutcomeChange, this);
            }
        }
    }

    public bool IsDeployed
    {
        get;
        private set
        {
            if (field != value)
            {
                field = value;
                this.Dispatch(_onDeployStateChange, this);
            }
        }
    }

    public int BuddyCount
    {
        get;
        private set
        {
            if (field != value)
            {
                field = value;
                this.Dispatch(_onBuddyCountChange, this);
            }
        }
    }
    public int MaxBuddies => 4;
    public int MaxOutcome => 5;

    public bool HasLagniapple { get; private set; }

    private readonly SmartEvent<MHRMeowmasters> _onStepChange = new();
    public event EventHandler<MHRMeowmasters> OnStepChange
    {
        add => _onStepChange.Hook(value);
        remove => _onStepChange.Unhook(value);
    }

    private readonly SmartEvent<MHRMeowmasters> _onDeployStateChange = new();
    public event EventHandler<MHRMeowmasters> OnDeployStateChange
    {
        add => _onDeployStateChange.Hook(value);
        remove => _onDeployStateChange.Unhook(value);
    }

    private readonly SmartEvent<MHRMeowmasters> _onExpectedOutcomeChange = new();
    public event EventHandler<MHRMeowmasters> OnExpectedOutcomeChange
    {
        add => _onExpectedOutcomeChange.Hook(value);
        remove => _onExpectedOutcomeChange.Unhook(value);
    }

    private readonly SmartEvent<MHRMeowmasters> _onBuddyCountChange = new();
    public event EventHandler<MHRMeowmasters> OnBuddyCountChange
    {
        add => _onBuddyCountChange.Hook(value);
        remove => _onBuddyCountChange.Unhook(value);
    }

    public void Update(MHRMeowmasterData data)
    {
        MaxSteps = data.MaxStep;
        Step = data.CurrentStep + (data.IsDeployed ? data.CurrentStep == 0 ? 1 : 0 : 0);
        BuddyCount = data.BuddiesCount;
        HasLagniapple = data.IsLagniappleActive;
        ExpectedOutcome = data.BuddiesCount + (HasLagniapple ? 1 : 0);
        IsDeployed = data.IsDeployed;
    }

    public void Dispose()
    {
        _onStepChange.Dispose();
        _onDeployStateChange.Dispose();
        _onExpectedOutcomeChange.Dispose();
        _onBuddyCountChange.Dispose();
    }
}