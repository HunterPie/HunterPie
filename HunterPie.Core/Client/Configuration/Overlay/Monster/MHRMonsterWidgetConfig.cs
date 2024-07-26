using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Settings.Types;
using System.Linq;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MHRMonsterWidgetConfig : MonsterWidgetConfig
{
    public override MonsterDetails Details { get; set; } = new MonsterDetails
    {
        AllowedAilments = new(MonsterAilmentRepository.FindAllBy(GameType.Rise).Select(it => it.Id))
    };
}