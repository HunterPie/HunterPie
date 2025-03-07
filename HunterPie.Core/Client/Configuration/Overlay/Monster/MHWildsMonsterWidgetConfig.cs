using HunterPie.Core.Architecture;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MHWildsMonsterWidgetConfig : MonsterWidgetConfig
{
    public override MonsterDetailsConfiguration Details { get; set; }
        = new MonsterDetailsConfiguration { AllowedAilments = new ObservableHashSet<int>() };
}