using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using System;

namespace HunterPie.UI.Overlay.Widgets.Monster.Adapters;

public static class MonsterTargetAdapter
{
    public static Target Adapt(
        MonsterWidgetConfig config,
        Target lockOnTarget,
        Target manualTarget
    )
    {
        return config.TargetMode.Value switch
        {
            TargetModeType.LockOn => lockOnTarget,
            TargetModeType.MapPin or TargetModeType.AutoQuest => manualTarget,
            TargetModeType.Infer => throw new NotImplementedException(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}