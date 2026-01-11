using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;

public class AbnormalityBarViewModel(AbnormalityWidgetConfig settings) : WidgetViewModel(settings, settings.Name, WidgetType.ClickThrough)
{
    public ObservableCollection<AbnormalityViewModel> Abnormalities { get; set => SetValue(ref field, value); } = new();

    public AbnormalityWidgetConfig Config { get; } = settings;

    public void SortAbnormalities()
    {
        Abnormalities = Config.SortByAlgorithm.Value switch
        {
            SortBy.Lowest => new(Abnormalities.OrderBy(e => e.Timer)),
            SortBy.Highest => new(Abnormalities.OrderByDescending(e => e.Timer)),
            SortBy.Off => Abnormalities,
            _ => throw new NotImplementedException("unreachable"),
        };
    }
}