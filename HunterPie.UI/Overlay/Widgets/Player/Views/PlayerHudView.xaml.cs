using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Player.Views;

/// <summary>
/// Interaction logic for PlayerHudView.xaml
/// </summary>
public partial class PlayerHudView : View<PlayerHudViewModel>, IWidget<PlayerHudWidgetConfig>, IWidgetWindow
{
    public PlayerHudView()
    {
        InitializeComponent();
    }

    public PlayerHudView(PlayerHudWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();
    }

    public PlayerHudWidgetConfig Settings { get; }

    public WidgetType Type => WidgetType.ClickThrough;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public string Title => "Player Widget";

    public event EventHandler<WidgetType> OnWidgetTypeChange;
}
