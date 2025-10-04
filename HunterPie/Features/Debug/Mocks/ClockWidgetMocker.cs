using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Clock.ViewModels;
using System;

namespace HunterPie.Features.Debug.Mocks;

internal class ClockWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockClockWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var config = new ClockWidgetConfig();
        var viewModel = new ClockViewModel(config)
        {
            QuestTimeLeft = TimeSpan.FromSeconds(3000)
        };

        MockBehavior.Run(() =>
        {
            if (viewModel.QuestTimeLeft.HasValue)
                viewModel.QuestTimeLeft -= TimeSpan.FromMilliseconds(16);

            viewModel.WorldTime.Add(TimeSpan.FromSeconds(1));
        }, 0.016f);

        return overlay.Register(viewModel);
    }
}