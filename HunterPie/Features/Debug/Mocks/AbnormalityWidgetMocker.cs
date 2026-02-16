using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Services;
using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Threading;

namespace HunterPie.Features.Debug.Mocks;

internal class AbnormalityWidgetMocker : IWidgetMocker
{
    private DispatcherTimer? _timer;

    public Observable<bool> Setting => ClientConfig.Config.Development.MockAbnormalityWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        _timer?.Stop();

        var mockSettings = new AbnormalityWidgetConfig();
        var viewModel = new AbnormalityBarViewModel(mockSettings);

        CreateAbnormalities(viewModel);

        _timer = MockBehavior.Run(() =>
        {
            foreach (AbnormalityViewModel abnormality in viewModel.Abnormalities)
            {
                double increment = abnormality.IsBuildup ? 1 : -1;

                abnormality.Timer += increment;

                if (abnormality.Timer < 0 || abnormality.Timer > abnormality.MaxTimer)
                    abnormality.Timer = abnormality.IsBuildup ? 0 : abnormality.MaxTimer;
            }

            viewModel.SortAbnormalities();
        });

        return overlay.Register(viewModel);
    }

    private static void CreateAbnormalities(AbnormalityBarViewModel viewModel)
    {
        AbnormalityDefinition[] consumables = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.World, AbnormalityGroup.SONGS);
        AbnormalityDefinition[] debuffs = AbnormalityRepository.FindAllAbnormalitiesBy(GameType.Wilds, AbnormalityGroup.DEBUFFS);

        var abnormalities = consumables
            .TakeLast(5)
            .Concat(debuffs.Take(5))
            .ToImmutableArray();

        foreach (AbnormalityDefinition abnormality in abnormalities)
            viewModel.Abnormalities.Add(new AbnormalityViewModel
            {
                Icon = abnormality.Icon,
                Id = abnormality.Id,
                Name = abnormality.Name,
                Timer = 100,
                MaxTimer = 100,
                IsBuff = abnormality.Group != AbnormalityGroup.DEBUFFS,
                IsBuildup = abnormality.IsBuildup,
                IsInfinite = abnormality.IsInfinite
            });
    }
}