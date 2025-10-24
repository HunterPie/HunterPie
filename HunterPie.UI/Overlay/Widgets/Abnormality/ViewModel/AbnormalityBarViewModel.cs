using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;

public class AbnormalityBarViewModel : WidgetViewModel
{
    private ObservableCollection<AbnormalityViewModel> _abnormalities = new();
    public ObservableCollection<AbnormalityViewModel> Abnormalities { get => _abnormalities; set => SetValue(ref _abnormalities, value); }

    public AbnormalityWidgetConfig Config { get; }

    public void SortAbnormalities()
    {
        Abnormalities = Config.SortByAlgorithm.Value switch
        {
            SortBy.Lowest => new(_abnormalities.OrderBy(e => e.Timer)),
            SortBy.Highest => new(_abnormalities.OrderByDescending(e => e.Timer)),
            SortBy.Off => _abnormalities,
            _ => throw new NotImplementedException("unreachable"),
        };
    }

    public AbnormalityBarViewModel(AbnormalityWidgetConfig settings) : base(settings, settings.Name, WidgetType.ClickThrough)
    {
        Config = settings;
    }
}