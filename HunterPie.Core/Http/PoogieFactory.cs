﻿using HunterPie.Core.Client;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Vault;
using System;

namespace HunterPie.Core.Http;

public static class PoogieFactory
{
    private const string CLIENT_ID = "X-Client-Id";
    private const string APP_VERSION = "X-App-Version";
    private const string CLIENT_TYPE = "X-HunterPie-Client";
    private const string USER_AGENT = "User-Agent";
    private const string TOKEN = "X-Token";

    public static readonly string[] Hosts =
    {
        "https://api.hunterpie.com",
        "https://mirror.hunterpie.com/mirror"
    };

    public static readonly string[] Documentation =
    {
        "https://docs.hunterpie.com"
    };

    /// <summary>
    /// Returns the default Http client with default headers and pointing to the default API endpoint
    /// </summary>
    /// <returns>Default HTTP client</returns>
    public static PoogieBuilder Default()
    {
        string clientId = RegistryConfig.Exists("client_id")
            ? RegistryConfig.Get<string>("client_id")
            : "Unknown";

        bool shouldRedirect = FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_REDIRECT_POOGIE);
        PoogieBuilder builder = shouldRedirect
                                ? new PoogieBuilder(ClientConfig.Config.Development.PoogieApiHost)
                                : new PoogieBuilder(Hosts);

        string token = CredentialVaultService.GetCredential()?.Password;

        return builder.WithTimeout(TimeSpan.FromSeconds(5))
                      .WithHeader(CLIENT_ID, clientId)
                      .WithHeader(APP_VERSION, ClientInfo.Version.ToString())
                      .WithHeader(CLIENT_TYPE, "v2")
                      .WithHeader(USER_AGENT, GetUserAgent())
                      .WithHeader(TOKEN, token);
    }

    public static PoogieBuilder Docs() => new(Documentation);

    private static string GetUserAgent()
    {
        string clientVersion = ClientInfo.Version.ToString();
        string platformVersion = Environment.OSVersion.ToString();
        return $"HunterPie/{clientVersion} ({platformVersion})";
    }
}
