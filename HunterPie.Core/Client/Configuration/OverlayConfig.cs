﻿using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;
using System;

namespace HunterPie.Core.Client.Configuration;

public class OverlayConfig : ISettings
{
    public PlayerHudWidgetConfig PlayerHudWidget { get; set; } = new();
    public MonsterWidgetConfig BossesWidget { get; set; } = new();
    public AbnormalityTrayConfig AbnormalityTray { get; set; } = new();
    public DamageMeterWidgetConfig DamageMeterWidget { get; set; } = new();
    public TelemetricsWidgetConfig DebugWidget { get; set; } = new();
    public WirebugWidgetConfig WirebugWidget { get; set; } = new();
    public ActivitiesWidgetConfig ActivitiesWidget { get; set; } = new();
    public InsectGlaiveWidgetConfig InsectGlaiveWidget { get; set; } = new();
    public ChargeBladeWidgetConfig ChargeBladeWidget { get; set; } = new();
    public DualBladesWidgetConfig DualBladesWidget { get; set; } = new();
    public SwitchAxeWidgetConfig SwitchAxeWidget { get; set; } = new();
    public LongSwordWidgetConfig LongSwordWidget { get; set; } = new();
    public ChatWidgetConfig ChatWidget { get; set; } = new();
    public ClockWidgetConfig ClockWidget { get; set; } = new();
    public SpecializedToolWidgetConfig PrimarySpecializedToolWidget { get; set; } = new()
    {
        Position = new(950, 400)
    };
    public SpecializedToolWidgetConfig SecondarySpecializedToolWidget { get; set; } = new()
    {
        Position = new(1000, 375),
        Scale = new(0.9, 2, 0.1, 0.1)
    };

    [Obsolete]
    public Keybinding ToggleDesignMode { get; set; } = "ScrollLock";

    [Obsolete]
    public Observable<bool> HideWhenUnfocus { get; set; } = false;
}
