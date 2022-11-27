using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;

namespace HunterPie.Integrations.Datasources.Common.Entity.Player;
public abstract class CommonPlayer : Scannable, IPlayer, IEventDispatcher, IDisposable
{
    public abstract string Name { get; protected set; }
    public abstract int HighRank { get; protected set; }
    public abstract int MasterRank { get; protected set; }
    public abstract int StageId { get; protected set; }
    public abstract bool InHuntingZone { get; protected set; }
    public abstract IParty Party { get; protected set; }
    public abstract IReadOnlyCollection<IAbnormality> Abnormalities { get; protected set; }
    public abstract IHealthComponent Health { get; protected set; }
    public abstract IStaminaComponent Stamina { get; protected set; }
    public abstract IWeapon Weapon { get; protected set; }

    protected readonly SmartEvent<EventArgs> _onLogin = new();
    public event EventHandler<EventArgs> OnLogin
    {
        add => _onLogin.Hook(value);
        remove => _onLogin.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onLogout = new();
    public event EventHandler<EventArgs> OnLogout
    {
        add => _onLogout.Hook(value);
        remove => _onLogout.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onDeath = new();
    public event EventHandler<EventArgs> OnDeath
    {
        add => _onDeath.Hook(value);
        remove => _onDeath.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onActionUpdate = new();
    public event EventHandler<EventArgs> OnActionUpdate
    {
        add => _onActionUpdate.Hook(value);
        remove => _onActionUpdate.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onStageUpdate = new();
    public event EventHandler<EventArgs> OnStageUpdate
    {
        add => _onStageUpdate.Hook(value);
        remove => _onStageUpdate.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onVillageEnter = new();
    public event EventHandler<EventArgs> OnVillageEnter
    {
        add => _onVillageEnter.Hook(value);
        remove => _onVillageEnter.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onVillageLeave = new();
    public event EventHandler<EventArgs> OnVillageLeave
    {
        add => _onVillageLeave.Hook(value);
        remove => _onVillageLeave.Unhook(value);
    }

    protected readonly SmartEvent<EventArgs> _onAilmentUpdate = new();
    public event EventHandler<EventArgs> OnAilmentUpdate
    {
        add => _onAilmentUpdate.Hook(value);
        remove => _onAilmentUpdate.Unhook(value);
    }

    protected readonly SmartEvent<WeaponChangeEventArgs> _onWeaponChange = new();
    public event EventHandler<WeaponChangeEventArgs> OnWeaponChange
    {
        add => _onWeaponChange.Hook(value);
        remove => _onWeaponChange.Unhook(value);
    }

    protected readonly SmartEvent<IAbnormality> _onAbnormalityStart = new();
    public event EventHandler<IAbnormality> OnAbnormalityStart
    {
        add => _onAbnormalityStart.Hook(value);
        remove => _onAbnormalityStart.Unhook(value);
    }

    protected readonly SmartEvent<IAbnormality> _onAbnormalityEnd = new();
    public event EventHandler<IAbnormality> OnAbnormalityEnd
    {
        add => _onAbnormalityEnd.Hook(value);
        remove => _onAbnormalityEnd.Unhook(value);
    }

    protected readonly SmartEvent<LevelChangeEventArgs> _onLevelChange = new();
    public event EventHandler<LevelChangeEventArgs> OnLevelChange
    {
        add => _onLevelChange.Hook(value);
        remove => _onLevelChange.Unhook(value);
    }

    protected CommonPlayer(IProcessManager process) : base(process) { }

    protected void HandleAbnormality<T, S>(Dictionary<string, IAbnormality> abnormalities, AbnormalitySchema schema, float timer, S newData)
        where T : IAbnormality, IUpdatable<S>
        where S : struct
    {
        if (abnormalities.ContainsKey(schema.Id) && timer <= 0)
        {
            var abnorm = (IUpdatable<S>)abnormalities[schema.Id];

            abnorm.Update(newData);

            _ = abnormalities.Remove(schema.Id);

            this.Dispatch(_onAbnormalityEnd, (IAbnormality)abnorm);

            if (abnorm is IDisposable disposable)
                disposable.Dispose();
        }
        else if (abnormalities.ContainsKey(schema.Id) && timer > 0)
        {

            var abnorm = (IUpdatable<S>)abnormalities[schema.Id];
            abnorm.Update(newData);
        }
        else if (!abnormalities.ContainsKey(schema.Id) && timer > 0)
        {
            if (schema.Icon == "ICON_MISSING")
                Core.Logger.Log.Info($"Missing abnormality: {schema.Id}");

            var abnorm = (IUpdatable<S>)Activator.CreateInstance(typeof(T), schema);

            if (abnorm is null)
                return;

            abnormalities.Add(schema.Id, (IAbnormality)abnorm);
            abnorm.Update(newData);
            this.Dispatch(_onAbnormalityStart, (IAbnormality)abnorm);
        }
    }

    public virtual void Dispose()
    {
        IDisposable[] events =
        {
            _onLogin, _onLogout, _onDeath, _onActionUpdate, _onStageUpdate, _onVillageEnter, _onVillageLeave,
            _onAilmentUpdate, _onWeaponChange, _onAbnormalityStart, _onAbnormalityEnd, _onLevelChange
        };

        if (Health is IDisposable health)
            health.Dispose();

        if (Stamina is IDisposable stamina)
            stamina.Dispose();

        if (Party is IDisposable party)
            party.Dispose();

        Abnormalities.TryCast<IDisposable>()
            .DisposeAll();

        if (Weapon is IDisposable weapon)
            weapon.Dispose();

        events.DisposeAll();
    }
}
