using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Observability.Logging;
using HunterPie.Integrations.Datasources.Common.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party.Data;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party;

public class MHWildsParty : CommonParty, IUpdatable<UpdateParty>, IUpdatable<UpdatePartyMember>
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private const int MAX_PARTY_SIZE = 4;

    public override int Size { get; protected set; } = 1;

    public override int MaxSize { get; protected set; } = MAX_PARTY_SIZE;

    private readonly Dictionary<nint, MHWildsPartyMember> _members = new();
    public override IReadOnlyCollection<IPartyMember> Members
    {
        get
        {
            lock (_members)
                return _members.Values;
        }
    }

    public bool Contains(nint id)
    {
        lock (_members)
            return _members.ContainsKey(id);
    }

    public void Clear()
    {
        lock (_members)
            foreach (int id in _members.Keys.ToArray())
                Remove(id);
    }

    public void Update(UpdateParty data)
    {
        Size = data.Players.Length;

        lock (_members)
            foreach (nint member in _members.Keys.ToArray())
                if (!data.Players.Contains(member))
                    Remove(member);
    }

    public void Update(UpdatePartyMember data)
    {
        if (!data.IsValid)
            return;

        lock (_members)
        {
            if (!_members.TryGetValue(data.Id, out MHWildsPartyMember? member))
            {
                Add(data);
                return;
            }

            member.Update(data);
        }
    }

    private void Remove(nint id)
    {
        if (!_members.Remove(id, out MHWildsPartyMember? member))
            return;

        _logger.Debug($"Removed player from party id: {id:X08} {member.Name}");

        this.Dispatch(
            toDispatch: _onMemberLeave,
            data: member
        );

        member.Dispose();
    }

    private void Add(UpdatePartyMember data)
    {
        if (_members.ContainsKey(data.Id))
            return;

        MHWildsPartyMember member = new();

        _members.Add(data.Id, member);

        member.Update(data);

        _logger.Debug($"Added new player to party id: {data.Id:X08} {data.Name} {data.Weapon}");

        this.Dispatch(
            toDispatch: _onMemberJoin,
            data: member
        );
    }
}