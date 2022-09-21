﻿using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Activities;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise;

internal class CohootContextHandler : IContextHandler
{
    private readonly MHRContext _context;
    private MHRPlayer Player => (MHRPlayer)_context.Game.Player;
    public readonly CohootNestViewModel ViewModel = new();

    public CohootContextHandler(MHRContext context)
    {
        _context = context;
        UpdateData();
    }

    public void HookEvents() => Player.Cohoot.OnCountChange += OnCountChange;

    public void UnhookEvents() => Player.Cohoot.OnCountChange -= OnCountChange;

    private void OnCountChange(object sender, MHRCohoot e)
    {
        ViewModel.Count = e.Count;
        ViewModel.MaxCount = e.MaxCount;
    }

    public void UpdateData()
    {
        ViewModel.Count = Player.Cohoot.Count;
        ViewModel.MaxCount = Player.Cohoot.MaxCount;
    }
}
