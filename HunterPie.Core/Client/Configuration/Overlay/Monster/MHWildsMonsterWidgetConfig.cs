using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Settings.Types;
using System.Linq;

namespace HunterPie.Core.Client.Configuration.Overlay.Monster;

public class MHWildsMonsterWidgetConfig : MonsterWidgetConfig
{
    public override MonsterDetailsConfiguration Details { get; set; }
        = new()
        {
            AllowedAilments = new(
                collection: MonsterAilmentRepository.FindAllBy(GameType.Wilds)
                    .Select(it => it.Id)
            )
        };
}