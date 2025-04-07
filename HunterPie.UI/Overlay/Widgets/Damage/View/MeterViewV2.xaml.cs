using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Damage.View;

#nullable enable
/// <summary>
/// Interaction logic for MeterViewV2.xaml
/// </summary>
public partial class MeterViewV2 : View<MeterViewModel>, IWidget<DamageMeterWidgetConfig>, IWidgetWindow
{
    public MeterViewV2(DamageMeterWidgetConfig config) : base(config)
    {
        Settings = config;

        InitializeComponent();
    }

    public DamageMeterWidgetConfig Settings { get; }

    public string Title => "Damage Meter";

    public WidgetType Type => WidgetType.ClickThrough;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public event EventHandler<WidgetType>? OnWidgetTypeChange;
}
