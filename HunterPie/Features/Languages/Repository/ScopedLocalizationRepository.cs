using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;

namespace HunterPie.Features.Languages.Repository;

internal sealed class ScopedLocalizationRepository : IScopedLocalizationRepository
{
    private readonly string _scopePath;
    private readonly ILocalizationRepository _localizationRepository;

    public ScopedLocalizationRepository(
        string scopePath,
        ILocalizationRepository localizationRepository)
    {
        _scopePath = scopePath;
        _localizationRepository = localizationRepository;
    }

    public LocalizationData? TryFindBy(string id)
    {
        string path = $"{_scopePath}[@Id='{id}']";

        return _localizationRepository.ExistsBy(path)
            ? _localizationRepository.FindBy(path)
            : null;
    }

    public LocalizationData FindBy(string id) =>
        _localizationRepository.FindBy($"{_scopePath}[@Id='{id}']");

    public string FindStringBy(string id) =>
        _localizationRepository.FindStringBy($"{_scopePath}[@Id='{id}']");
}