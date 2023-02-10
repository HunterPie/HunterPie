
using HunterPie.Integrations.Poogie.Account.Models;

namespace HunterPie.Features.Account.Model;

internal record UserAccount(
    string Username,
    string Email,
    string AvatarUrl,
    bool IsSupporter
);

internal static class UserAccountExtensions
{
    // TODO: Turn this into a mapper
    public static UserAccount ToModel(this MyUserAccountResponse response)
    {
        return new UserAccount(
            Username: response.Username,
            Email: response.Email,
            AvatarUrl: response.AvatarUrl,
            IsSupporter: response.IsSupporter
        );
    }
}