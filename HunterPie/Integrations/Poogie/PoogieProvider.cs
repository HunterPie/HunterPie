using HunterPie.Core.Client;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Networking.Http;
using HunterPie.Core.Vault;
using System;

namespace HunterPie.Integrations.Poogie;
internal static class PoogieProvider
{
    private const string CLIENT_ID = "X-Client-Id";
    private const string APP_VERSION = "X-App-Version";
    private const string CLIENT_TYPE = "X-HunterPie-Client";
    private const string USER_AGENT = "User-Agent";
    private const string TOKEN = "X-Token";
    private const string SUPPORTER = "X-Supporter-Token";
    private const string CLIENT_TYPE_VALUE = "v2";

    private static readonly string[] Hosts = { "https://api.hunterpie.com", "https://mirror.hunterpie.com/mirror" };


    public static HttpClientBuilder Default()
    {
        string clientId = RegistryConfig.Exists("client_id")
            ? RegistryConfig.Get<string>("client_id")
            : "Unknown";

        bool shouldRedirect = FeatureFlagManager.IsEnabled(
            FeatureFlags.FEATURE_REDIRECT_POOGIE
        );

        HttpClientBuilder builder = shouldRedirect && ClientConfig.Config is { } config
            ? new HttpClientBuilder(config.Development.PoogieApiHost)
            : new HttpClientBuilder(Hosts);

        string token = CredentialVaultService.GetCredential()?.Password;

        return builder.WithTimeout(TimeSpan.FromSeconds(10))
                      .WithRetry(3)
                      .WithHeader(CLIENT_ID, clientId)
                      .WithHeader(SUPPORTER, ClientConfig.Config?.Client.SupporterSecretToken)
                      .WithHeader(APP_VERSION, ClientInfo.Version.ToString())
                      .WithHeader(CLIENT_TYPE, CLIENT_TYPE_VALUE)
                      .WithHeader(USER_AGENT, GetUserAgent())
                      .WithHeader(TOKEN, token);
    }

    private static string GetUserAgent()
    {
        string clientVersion = ClientInfo.Version.ToString();
        string platformVersion = Environment.OSVersion.ToString();
        return $"HunterPie/{clientVersion} ({platformVersion})";
    }
}
