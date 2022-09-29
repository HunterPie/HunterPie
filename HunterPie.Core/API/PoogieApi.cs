using HunterPie.Core.API.Entities;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using System;
using System.Threading.Tasks;

namespace HunterPie.Core.API;

#nullable enable
public static class PoogieApi
{
    private const string VERSION_PATH = "/v1/version";
    private const string CRASH_PATH = "/v1/report/crash";
    private const string SUPPORTER_PATH = "/v1/supporter";
    private const string NOTIFICATIONS = "/v1/notifications";
    private const string LOGIN = "/v1/login";
    private const string LOGOUT = "/v1/logout";
    private const string ACCOUNT = "/v1/account";
    private const string MY_ACCOUNT = "/v1/user/me";
    private const string SUPPORTER_HEADER = "X-Supporter-Token";
    private const double DEFAULT_TIMEOUT = 10;
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);

    private static async Task<PoogieApiResult<T>?> Get<T>(string path) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Get(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithTimeout(DefaultTimeout)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        if (!resp.Success)
            return null;

        return await resp.AsJson<T>();
    }

    private static async Task<PoogieApiResult<T>?> Post<P, T>(string path, P payload) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Post(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithJson(payload)
            .WithTimeout(DefaultTimeout)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        if (!resp.Success)
            return null;

        return await resp.AsJson<T>();
    }

    public static async Task<PoogieApiResult<LoginResponse>?> Login(LoginRequest request) => await Post<LoginRequest, LoginResponse>(LOGIN, request);

    public static async Task<PoogieApiResult<LogoutResponse>?> Logout() => await Post<object, LogoutResponse>(LOGOUT, new());

    public static async Task<PoogieApiResult<VersionResponse>?> GetLatestVersion() => await Get<VersionResponse>(VERSION_PATH);

    public static async Task<PoogieApiResult<SupporterValidationResponse>?> ValidateSupporterToken() => await Get<SupporterValidationResponse>(SUPPORTER_PATH + "/verify");

    public static async Task<PoogieApiResult<Notification[]>?> GetNotifications() => await Get<Notification[]>(NOTIFICATIONS);

    public static async Task<PoogieApiResult<MyUserAccountResponse>?> GetMyUserAccount() => await Get<MyUserAccountResponse>(MY_ACCOUNT);

    public static async Task<PoogieApiResult<RegisterResponse>?> Register(RegisterRequest request) => await Post<RegisterRequest, RegisterResponse>(ACCOUNT, request);
}
#nullable restore
