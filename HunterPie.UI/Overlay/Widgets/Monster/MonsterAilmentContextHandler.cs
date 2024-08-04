using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Settings.Types;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System.Collections.Specialized;

namespace HunterPie.UI.Overlay.Widgets.Monster;

#nullable enable
public class MonsterAilmentContextHandler : MonsterAilmentViewModel
{
    private readonly MonsterDetailsConfiguration _detailsConfiguration;

    public readonly IMonsterAilment Context;

    public MonsterAilmentContextHandler(
        IMonsterAilment context,
        MonsterWidgetConfig config
    ) : base(config)
    {
        _detailsConfiguration = config.Details;
        Context = context;

        Update();
        HookEvents();
    }

    private void HookEvents()
    {
        Context.OnTimerUpdate += OnTimerUpdate;
        Context.OnBuildUpUpdate += OnBuildUpUpdate;
        Context.OnCounterUpdate += OnCounterUpdate;
        _detailsConfiguration.AllowedAilments.CollectionChanged += OnAllowedAilmentsChanged;
    }

    private void UnhookEvents()
    {
        Context.OnTimerUpdate -= OnTimerUpdate;
        Context.OnBuildUpUpdate -= OnBuildUpUpdate;
        Context.OnCounterUpdate -= OnCounterUpdate;
        _detailsConfiguration.AllowedAilments.CollectionChanged -= OnAllowedAilmentsChanged;
    }

    private void OnAllowedAilmentsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        IsEnabled = _detailsConfiguration.AllowedAilments.Contains(Context.Definition.Id);
    }

    private void OnCounterUpdate(object? sender, IMonsterAilment e) => Count = e.Counter;

    private void OnBuildUpUpdate(object? sender, IMonsterAilment e)
    {
        if (e.MaxBuildUp <= 0)
            return;

        MaxBuildup = e.MaxBuildUp;
        Buildup = e.BuildUp;
    }

    private void OnTimerUpdate(object? sender, IMonsterAilment e)
    {
        if (e.MaxTimer <= 0)
            return;

        MaxTimer = e.MaxTimer;
        Timer = e.Timer;
    }

    private void Update()
    {
        Name = Context.Id;
        Count = Context.Counter;
        MaxBuildup = Context.MaxBuildUp;
        Buildup = Context.BuildUp;
        MaxTimer = Context.MaxTimer;
        Timer = Context.Timer;
        IsEnabled = _detailsConfiguration.AllowedAilments.Contains(Context.Definition.Id);
    }

    public override void Dispose()
    {
        UnhookEvents();
        base.Dispose();
    }
}
