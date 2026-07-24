using HunterPie.Core.Client.Localization;
using HunterPie.Domain.Interfaces;
using HunterPie.Features.Languages.Repository;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ClientLocalizationInitializer(
    LocalizationRepository repository
) : IInitializer
{
    public Task Init()
    {
        _ = Localization.Instance;

        repository.Load();

        return Task.CompletedTask;
    }
}