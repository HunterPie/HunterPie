using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Rise.Definitions;
using System;
using System.Linq;

namespace HunterPie.Core.Game.Rise.Entities.Activities
{
    public class MHRSubmarine : IEventDispatcher, IUpdatable<MHRSubmarineData>
    {
        private int _count;
        private int _daysLeft;
        private bool _isUnlocked;

        public int Count
        {
            get => _count;
            private set
            {
                if (value != _count)
                {
                    _count = value;
                    this.Dispatch(OnItemCountChange, this);
                }
            }
        }

        public int MaxCount { get; private set; } = 20;

        public int DaysLeft
        {
            get => _daysLeft;
            private set
            {
                if (value != _daysLeft)
                {
                    _daysLeft = value;
                    this.Dispatch(OnDaysLeftChange, this);
                }
            }
        }

        public bool IsUnlocked
        {
            get => _isUnlocked;
            private set
            {
                if (value != _isUnlocked)
                {
                    _isUnlocked = value;
                    this.Dispatch(OnLockStateChange, this);
                }
            }
        }

        public event EventHandler<MHRSubmarine> OnItemCountChange;
        public event EventHandler<MHRSubmarine> OnDaysLeftChange;
        public event EventHandler<MHRSubmarine> OnLockStateChange;

        void IUpdatable<MHRSubmarineData>.Update(MHRSubmarineData data)
        {
            DaysLeft = data.Data.DaysLeft;
            MaxCount = data.Items.Length;
            Count = data.Items.Count(item => item.IsNotEmpty());
            IsUnlocked = data.Data.Buddy != 0;
        }
    }
}
