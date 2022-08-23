using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Rise.Definitions;
using System;

namespace HunterPie.Core.Game.Rise.Entities.Activities
{
    public class MHRCohoot : IEventDispatcher, IUpdatable<MHRCohootStructure>
    {
        private int _count;

        public int Count
        {
            get => _count;
            private set
            {
                if (value != _count)
                {
                    _count = value;
                    this.Dispatch(OnCountChange, this);
                }
            }
        }
        public int MaxCount { get; private set; }

        public event EventHandler<MHRCohoot> OnCountChange;

        void IUpdatable<MHRCohootStructure>.Update(MHRCohootStructure data)
        {
            MaxCount = data.MaxCount;
            Count = data.Count;
        }
    }
}
