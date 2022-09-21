using HunterPie.Core.API.Entities;
using HunterPie.Core.Client;
using HunterPie.Core.Http;
using System;
using System.Net;
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
    private const string SUPPORTER_HEADER = "X-Supporter-Token";
    private const double DEFAULT_TIMEOUT = 10;
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(DEFAULT_TIMEOUT);

    private static async Task<T?> Get<T>(string path) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Get(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithTimeout(DefaultTimeout)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        return !resp.Success ? null : resp.Status >= HttpStatusCode.BadRequest ? null : await resp.AsJson<T>();
    }

    private static async Task<T?> Post<P, T>(string path, P payload) where T : class
    {
        using Poogie request = PoogieFactory.Default()
            .Post(path)
            .WithHeader(SUPPORTER_HEADER, ClientConfig.Config.Client.SupporterSecretToken)
            .WithJson(payload)
            .WithTimeout(DefaultTimeout)
            .Build();

        using PoogieResponse resp = await request.RequestAsync();

        return !resp.Success ? null : resp.Status >= HttpStatusCode.BadRequest ? null : await resp.AsJson<T>();
    }

    public static async Task<LoginResponse?> Login(LoginRequest request)
    {
        LoginResponse? resp = await Post<LoginRequest, LoginResponse>(LOGIN, request);
        return resp;
    }

    public static async Task<VersionResponse?> GetLatestVersion()
    {
        VersionResponse? resp = await Get<VersionResponse>(VERSION_PATH);
        return resp;
    }

    public static async Task<SupporterValidationResponse?> ValidateSupporterToken()
    {
        SupporterValidationResponse? resp = await Get<SupporterValidationResponse>(SUPPORTER_PATH + "/verify");
        return resp;
    }

    public static async Task<Notification[]> GetNotifications()
    {
        Notification[]? resp = await Get<Notification[]>(NOTIFICATIONS);
        return resp ?? Array.Empty<Notification>();
    }
}
#nullable restore
