using System;
using System.ComponentModel;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

internal class DesignModeViewModels
{
    private static readonly Lazy<bool> _isDesignMode = new(() => DesignerProperties.GetIsInDesignMode(new()));
    public static bool IsDesignMode => _isDesignMode.Value;

    public static readonly MockBossMonsterViewModel MockBossMonsterViewModel = new(new());
}