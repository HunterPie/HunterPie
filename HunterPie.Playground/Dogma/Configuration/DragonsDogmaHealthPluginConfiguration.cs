using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Plugins.Configuration;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;
using Range = HunterPie.Core.Settings.Types.Range;

namespace HunterPie.Playground.Dogma.Configuration;

internal class DragonsDogmaHealthPluginConfiguration : PluginConfiguration, IWidgetSettings
{
    public override int Version => 1;

    public Observable<TargetModeType> TargetMode { get; set; } = TargetModeType.Infer;

    public Observable<bool> Initialize { get; set; } = true;

    public Observable<bool> Enabled { get; set; } = true;

    public Observable<bool> HideWhenUiOpen { get; set; } = true;

    public Position Position { get; set; } = new Position(500, 100);

    public Range Opacity { get; set; } = new Range(1, 1, 0, 0.1);

    public Range Scale { get; set; } = new Range(1, 1, 0, 0.1);
}
