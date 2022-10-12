using HunterPie.Core.API.Entities;

namespace HunterPie.Features.Account.Model;
internal class UserAccount
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string AvatarUrl { get; init; }
    public bool IsSupporter { get; init; }
}

internal static class UserAccountExtensions
{
    // TODO: Turn this into a mapper
    public static UserAccount ToModel(this MyUserAccountResponse response)
    {
        return new UserAccount
        {
            Username = response.Username,
            Email = response.Email,
            AvatarUrl = response.AvatarUrl,
            IsSupporter = response.IsSupporter
        };
    }
}