using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Animation;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ResourcesService = HunterPie.UI.Assets.Application.Resources;

namespace HunterPie.UI.Overlay.Widgets.Player.Views;

/// <summary>
/// Interaction logic for PlayerHudView.xaml
/// </summary>
public partial class PlayerHudView : View<PlayerHudViewModel>, IWidget<PlayerHudWidgetConfig>, IWidgetWindow
{
    private Storyboard _currentHealthBarAnimation = new Storyboard();

    private static readonly Brush _defaultHealthBarColor = ResourcesService.Get<Brush>("WIDGET_PLAYER_HEALTH_FOREGROUND");

    private readonly Dictionary<AbnormalityCategory, Brush> _abnormalityColors = new Dictionary<AbnormalityCategory, Brush>()
    {
        { AbnormalityCategory.None, ResourcesService.Get<Brush>("WIDGET_PLAYER_HEALTH_FOREGROUND") },
        { AbnormalityCategory.Fire, ResourcesService.Get<Brush>("WIDGET_PLAYER_HEALTH_FIRE_FOREGROUND") },
        { AbnormalityCategory.Poison, ResourcesService.Get<Brush>("WIDGET_PLAYER_POISON_FOREGROUND") },
        { AbnormalityCategory.Bleed, ResourcesService.Get<Brush>("WIDGET_PLAYER_BLEED_FOREGROUND") },
    };

    public PlayerHudWidgetConfig Settings { get; }

    public WidgetType Type => WidgetType.ClickThrough;

    IWidgetSettings IWidgetWindow.Settings => Settings;

    public string Title => "Player Widget";

    public event EventHandler<WidgetType> OnWidgetTypeChange;

    public PlayerHudView()
    {
        InitializeComponent();
    }

    public PlayerHudView(PlayerHudWidgetConfig config)
    {
        Settings = config;
        InitializeComponent();
    }

    protected override void Initialize()
    {
        HookEvents();
    }

    private void HookEvents()
    {
        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void UnhookEvents()
    {
        ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
    }

    private void HandleAbnormalityCategoryChange() =>
        UIThread.Invoke(() =>
        {
            bool success = _abnormalityColors.TryGetValue(ViewModel.AbnormalityCategory, out Brush brush);

            if (!success)
                return;

            Storyboard newAnimation = BuildBarStoryboard(brush);

            _currentHealthBarAnimation?.Remove(PART_HealthBar);
            _currentHealthBarAnimation = newAnimation;
            PART_HealthBar.BeginStoryboard(_currentHealthBarAnimation);
        });

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(PlayerHudViewModel.AbnormalityCategory):
                HandleAbnormalityCategoryChange();
                break;
            default:
                break;
        }
    }

    private Storyboard BuildBarStoryboard(Brush brush)
    {
        var sb = new Storyboard()
        {
            Children = new TimelineCollection()
            {
                new BrushAnimation()
                {
                    To = brush,
                    Duration = new Duration(TimeSpan.FromMilliseconds(500)),
                },
            },
        };

        Storyboard.SetTargetProperty(sb, new PropertyPath(nameof(Foreground)));

        return sb;
    }

    public override void Dispose()
    {
        UnhookEvents();

        base.Dispose();
    }
}
