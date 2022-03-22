using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Rise.Definitions;
using HunterPie.Core.Game.Rise.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Rise.Entities.Activities
{
    public class MHRTrainingDojo : IEventDispatcher, IUpdatable<MHRTrainingDojoData>
    {
        private int _rounds;
        private int _boosts;
        private int _buddiesCount;

        public int Rounds
        {
            get => _rounds;
            private set
            {
                if (value != _rounds)
                {
                    _rounds = value;
                    this.Dispatch(OnRoundsLeftChange, this);
                }
            }
        }
        public int MaxRounds { get; private set; }

        public int Boosts
        {
            get => _boosts;
            private set
            {
                if (value != _boosts)
                {
                    _boosts = value;
                    this.Dispatch(OnBoostsLeftChange, this);
                }
            }
        }
        public int MaxBoosts { get; private set; }

        public int BuddiesCount
        {
            get => _buddiesCount;
            private set
            {
                if (value != _buddiesCount)
                {
                    _buddiesCount = value;
                    this.Dispatch(OnBuddyCountChange, this);
                }
            }
        }

        public readonly MHRBuddy[] Buddies = { new(), new(), new(), new(), new(), new() };

        public event EventHandler<MHRTrainingDojo> OnRoundsLeftChange;
        public event EventHandler<MHRTrainingDojo> OnBoostsLeftChange;
        public event EventHandler<MHRTrainingDojo> OnBuddyCountChange;

        void IUpdatable<MHRTrainingDojoData>.Update(MHRTrainingDojoData data)
        {
            BuddiesCount = data.BuddiesCount;

            for (int i = 0; i < Buddies.Length; i++)
            {
                IUpdatable<MHRBuddyData> buddy = Buddies[i];
                buddy.Update(data.Buddies[i]);
            }

            MaxRounds = data.MaxRounds;
            Rounds = data.Rounds;
            MaxBoosts = data.MaxBoosts;
            Boosts = data.Boosts;

        }
    }
}
