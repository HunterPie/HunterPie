using HunterPie.Core.Client;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.UseCase;

internal class AccountPromotionalUseCase
{
    private const string PROMOTIONAL_KEY = "ShouldShowAccountPromotional";

    public static async Task<bool> ShouldShow()
    {
        bool hasSeen = RegistryConfig.Exists(PROMOTIONAL_KEY);
        bool isLoggedIn = await AccountManager.ValidateSessionToken();

        return !hasSeen && !isLoggedIn;
    }

    public static void MarkAsSeen() => RegistryConfig.Set(PROMOTIONAL_KEY, true);
}
