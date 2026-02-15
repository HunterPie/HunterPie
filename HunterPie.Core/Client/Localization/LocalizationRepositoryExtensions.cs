using HunterPie.Core.Client.Configuration.Enums;

namespace HunterPie.Core.Client.Localization;

public static class LocalizationRepositoryExtensions
{
    extension(ILocalizationRepository repository)
    {
        public string GetQuestNameBy(GameType game, int questId)
        {
            string localizationQuery = $"//Strings/Quests/{game}/Quest[@Id='{questId}']";

            return repository.ExistsBy(localizationQuery)
                ? repository.FindStringBy(localizationQuery)
                : "Unknown mission";
        }
    }
}