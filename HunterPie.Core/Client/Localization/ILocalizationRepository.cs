using HunterPie.Core.Client.Localization.Entity;

namespace HunterPie.Core.Client.Localization;

public interface ILocalizationRepository
{
    public LocalizationData FindBy(string path);

    public bool ExistsBy(string path);

    public string FindStringBy(string path);

    public LocalizationData FindByEnum<T>(T value) where T : notnull;

    public IScopedLocalizationRepository WithScope(string scope);
}