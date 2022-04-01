using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using System;

namespace HunterPie.Core.Game.Rise.Entities.Activities
{
    public class MHRMeowmasters : IEventDispatcher
    {
        private int _daysLeft;
        private int _expectedOutcome;
        private bool _isDeployed;

        public int DaysLeft
        {
            get => _daysLeft;
            private set
            {
                if (_daysLeft != value)
                {
                    _daysLeft = value;
                    this.Dispatch(OnDaysLeftChange, this);
                }
            }
        }

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

        public event EventHandler<MHRMeowmasters> OnDaysLeftChange;
        public event EventHandler<MHRMeowmasters> OnDeployStateChange;
        public event EventHandler<MHRMeowmasters> OnExpectedOutcomeChange;
    }
}
