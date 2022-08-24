using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Rise.Definitions;
using System;

namespace HunterPie.Core.Game.Rise.Entities.Activities
{
    public class MHRMeowmasters : IEventDispatcher, IUpdatable<MHRMeowmasterData>
    {
        private int _step;
        private int _expectedOutcome;
        private bool _isDeployed;
        private int _buddyCount;

        public int Step
        {
            get => _step;
            private set
            {
                if (_step != value)
                {
                    _step = value;
                    this.Dispatch(OnStepChange, this);
                }
            }
        }

        public int MaxSteps { get; private set; }

        public int ExpectedOutcome
        {
            get => _expectedOutcome;
            private set
            {
                if (value != _expectedOutcome)
                {
                    _expectedOutcome = value;
                    this.Dispatch(OnExpectedOutcomeChange, this);
                }
            }
        }

        public bool IsDeployed
        {
            get => _isDeployed;
            private set
            {
                if (_isDeployed != value)
                {
                    _isDeployed = value;
                    this.Dispatch(OnDeployStateChange, this);
                }
            }
        }

        public int BuddyCount
        {
            get => _buddyCount;
            private set
            {
                if (_buddyCount != value)
                {
                    _buddyCount = value;
                    this.Dispatch(OnBuddyCountChange, this);
                }
            }
        }
        public int MaxBuddies => 4;
        public int MaxOutcome => 5;

        public bool HasLagniapple { get; private set; }

        public event EventHandler<MHRMeowmasters> OnStepChange;
        public event EventHandler<MHRMeowmasters> OnDeployStateChange;
        public event EventHandler<MHRMeowmasters> OnExpectedOutcomeChange;
        public event EventHandler<MHRMeowmasters> OnBuddyCountChange;

        void IUpdatable<MHRMeowmasterData>.Update(MHRMeowmasterData data)
        {
            MaxSteps = data.MaxStep;
            Step = data.CurrentStep + (data.IsDeployed ? 1 : 0);
            BuddyCount = data.BuddiesCount;
            HasLagniapple = data.IsLagniappleActive;
            ExpectedOutcome = BuddyCount + (HasLagniapple ? 1 : 0);
            IsDeployed = data.IsDeployed;
        }
    }
}
