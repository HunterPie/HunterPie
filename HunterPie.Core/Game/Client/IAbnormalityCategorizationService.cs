using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Client;
public interface IAbnormalityCategorizationService
{
    AbnormalityCategory Categorize(IAbnormality abnormality);
}
