using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Settings.Types;
using System.Linq;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MHWMonsterWidgetConfig : MonsterWidgetConfig
{
    public override MonsterDetails Details { get; set; } = new MonsterDetails
    {
        AllowedAilments = new(MonsterAilmentRepository.FindAllBy(GameType.World).Select(it => it.Id))
    };
}
