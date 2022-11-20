using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.NPC;

public class MHRBuddy : IEventDispatcher, IUpdatable<MHRBuddyData>
{
    public string _name;
    public int _level;

    public string Name
    {
        get => _name;
        private set
        {
            if (value != _name)
            {
                _name = value;
                this.Dispatch(OnNameChange, this);
            }
        }
    }

    public int Level
    {
        get => _level;
        private set
        {
            if (value != _level)
            {
                _level = value;
                this.Dispatch(OnLevelChange, this);
            }
        }
    }

    public event EventHandler<MHRBuddy> OnNameChange;
    public event EventHandler<MHRBuddy> OnLevelChange;

    void IUpdatable<MHRBuddyData>.Update(MHRBuddyData data)
    {
        Name = data.Name;
        Level = data.Level;
    }
}
