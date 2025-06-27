using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.Controllers;

internal class MaterialRetrievalController : IContextHandler
{
    private readonly MHWildsContext _context;
    private readonly MaterialRetrievalViewModel _viewModel;
    private readonly Dispatcher _dispatcher;
    private MHWildsPlayer Player => (MHWildsPlayer)_context.Game.Player;
    private readonly Dictionary<MHWildsMaterialRetrievalCollector, MaterialRetrievalCollectorViewModel> _collectorViewModels = new();

    public MaterialRetrievalController(
        MHWildsContext context,
        MaterialRetrievalViewModel viewModel,
        Dispatcher dispatcher)
    {
        _context = context;
        _viewModel = viewModel;
        _dispatcher = dispatcher;
        UpdateData();
    }

    private void UpdateData()
    {
        Player.MaterialRetrieval.Collectors.ForEach(AddCollector);
    }

    public void HookEvents()
    {
        Player.MaterialRetrieval.AddSource += OnMaterialRetrievalSourceCreated;
        Player.MaterialRetrieval.RemoveSource += OnMaterialRetrievalSourceDestroyed;
    }

    public void UnhookEvents()
    {
        Player.MaterialRetrieval.AddSource -= OnMaterialRetrievalSourceCreated;
        Player.MaterialRetrieval.RemoveSource -= OnMaterialRetrievalSourceDestroyed;

        _collectorViewModels.Keys.ForEach(RemoveCollector);
    }

    private void OnMaterialRetrievalSourceCreated(
        object sender,
        ValueCreationEventArgs<MHWildsMaterialRetrievalCollector> e) => _dispatcher.BeginInvoke(() => AddCollector(e.Value));

    private void OnMaterialRetrievalSourceDestroyed(
        object sender,
        ValueCreationEventArgs<MHWildsMaterialRetrievalCollector> e) => _dispatcher.BeginInvoke(() => RemoveCollector(e.Value));

    private void OnCollectorCountChanged(object sender, SimpleValueChangeEventArgs<int> e)
    {
        if (sender is not MHWildsMaterialRetrievalCollector model)
            return;

        if (!_collectorViewModels.TryGetValue(model, out MaterialRetrievalCollectorViewModel viewModel))
            return;

        viewModel.Count = e.NewValue;
    }

    private void AddCollector(MHWildsMaterialRetrievalCollector collector)
    {
        if (_collectorViewModels.ContainsKey(collector))
            return;

        var viewModel = new MaterialRetrievalCollectorViewModel
        {
            Id = collector.Collector,
            MaxCount = collector.MaxCount,
            Count = collector.Count,
        };
        _collectorViewModels.Add(collector, viewModel);
        _viewModel.Collectors.Add(viewModel);

        collector.CountChanged += OnCollectorCountChanged;
    }

    private void RemoveCollector(MHWildsMaterialRetrievalCollector collector)
    {
        collector.CountChanged -= OnCollectorCountChanged;

        if (!_collectorViewModels.TryGetValue(collector, out MaterialRetrievalCollectorViewModel viewModel))
            return;

        _collectorViewModels.Remove(collector);
        _viewModel.Collectors.Remove(viewModel);
    }
}